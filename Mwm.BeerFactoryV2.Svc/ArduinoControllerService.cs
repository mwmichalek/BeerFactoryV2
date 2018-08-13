using CommandMessenger;
using CommandMessenger.Transport.Serial;
using Mwm.BeerFactoryV2.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Mwm.BeerFactoryV2.Svc {
    public class ArduinoControllerService {
        enum Command {
            Acknowledge,
            Error,
            PingRequest,
            PingResult,
            StatusRequest,
            StatusResult,
            KettleRequest,
            KettleResult,
            TempChange,
            SsrChange,
            HeaterChange,
            PumpChange
        };

        private SerialTransport _serialTransport;
        private CmdMessenger _cmdMessenger;
        public bool IsConnected { get; set; }

        public EventHandler<ConnectionStatusEvent> ConnectionStatusEventHandler { get; set; }
        public EventHandler<TemperatureResult> TemperatureResultEventHandler { get; set; }

        public EventHandler<SsrResult> SsrResultEventHandler { get; set; }
        public EventHandler<HeaterResult> HeaterResultEventHandler { get; set; }

        public ArduinoControllerService() {

        }

        public void Run() {
            
            if (Setup()) {
                while (IsConnected) {
                    Ping();
                     
                }
                Exit();
                ConnectionStatusEventHandler?.Invoke(this, new ConnectionStatusEvent { Type = ConnectionStatusEvent.EventType.Disconnected });
            } else {
                ConnectionStatusEventHandler?.Invoke(this, new ConnectionStatusEvent { Type = ConnectionStatusEvent.EventType.NotConnected });
            }
        }


        public bool Setup() {
            _serialTransport = new SerialTransport {
                CurrentSerialSettings = { PortName = "COM3", BaudRate = 57600, DtrEnable = false } // object initializer
            };

            _cmdMessenger = new CmdMessenger(_serialTransport, BoardType.Bit32);

            _cmdMessenger.Attach(OnUnknownCommand);
            _cmdMessenger.Attach((int)Command.Acknowledge, OnAcknowledge);
            _cmdMessenger.Attach((int)Command.Error, OnError);
            _cmdMessenger.Attach((int)Command.TempChange, OnTempChange);
            _cmdMessenger.Attach((int)Command.SsrChange, OnSsrChange);
            _cmdMessenger.Attach((int)Command.HeaterChange, OnHeaterChange);

            _cmdMessenger.NewLineReceived += NewLineReceived;
            _cmdMessenger.NewLineSent += NewLineSent;

            IsConnected = _cmdMessenger.Connect();


            if (IsConnected) {
                ConnectionStatusEventHandler?.Invoke(this, new ConnectionStatusEvent { Type = ConnectionStatusEvent.EventType.Connected });

                RequestStatus();
            }

            return IsConnected;
        }

        public void Ping() {
            var pingCommand = new SendCommand((int)Command.PingRequest, (int)Command.PingResult, 2000);
            var pingResultCommand = _cmdMessenger.SendCommand(pingCommand);
            IsConnected = pingResultCommand.Ok;
        }

        public void Exit() {
            _cmdMessenger.Disconnect();
            _cmdMessenger.Dispose();
            _serialTransport.Dispose();
        }

        public void RequestStatus() {
            var statusCommand = new SendCommand((int)Command.StatusRequest, (int)Command.StatusResult, 2000);
            var statusResultCommand = _cmdMessenger.SendCommand(statusCommand);
            var success = statusResultCommand.Ok;
        }

        // ------------------  C A L L B A C K S ---------------------

        private void OnSsrChange(ReceivedCommand receivedCommand) {
            int.TryParse(receivedCommand.ReadStringArg(), out int ssrIndex);
            int.TryParse(receivedCommand.ReadStringArg(), out int ssrValue);

            SsrResultEventHandler?.Invoke(this, new SsrResult { Index = ssrIndex, IsEngaged = ssrValue == 1 });
        }

        private void OnHeaterChange(ReceivedCommand receivedCommand) {
            int.TryParse(receivedCommand.ReadStringArg(), out int heaterIndex);
            int.TryParse(receivedCommand.ReadStringArg(), out int heaterValue);

            HeaterResultEventHandler?.Invoke(this, new HeaterResult { Index = heaterIndex, IsEngaged = heaterIndex == 1 });
        }

        private void OnTempChange(ReceivedCommand receivedCommand) {
            int.TryParse(receivedCommand.ReadStringArg(), out int probeIndex);
            decimal.TryParse(receivedCommand.ReadStringArg(), out decimal temp);

            TemperatureResultEventHandler?.Invoke(this, new TemperatureResult { Index = probeIndex, Value = temp });
        }

        void OnUnknownCommand(ReceivedCommand arguments) {
            Console.WriteLine("Command without attached callback received");
        }

        void OnAcknowledge(ReceivedCommand arguments) {
            ConnectionStatusEventHandler?.Invoke(this, new ConnectionStatusEvent { Type = ConnectionStatusEvent.EventType.Ready });
        }

        // Callback function that prints that the Arduino has experienced an error
        void OnError(ReceivedCommand arguments) {
            Console.WriteLine(" Arduino has experienced an error");
        }

        // Log received line to console
        private void NewLineReceived(object sender, CommandEventArgs e) {
            //Console.WriteLine(@"Received > " + e.Command.CommandString());
            //foreach (var arg in e.Command.Arguments) {
            //    Console.WriteLine($"Arg: {arg}");
            //}
        }

        // Log sent line to console
        private void NewLineSent(object sender, CommandEventArgs e) {
            //Console.WriteLine(@"Sent > " + e.Command.CommandString());
        }

    }
}

