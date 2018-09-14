using CommandMessenger;
using CommandMessenger.Transport.Serial;
using Mwm.BeerFactoryV2.Service.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Mwm.BeerFactoryV2.Service {
    public class UsbArduinoControllerService : ArduinoControllerService {
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

        private IEventAggregator _eventAggregator;

        public UsbArduinoControllerService(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<KettleCommandEvent>().Subscribe((kettleCommand) => {
                ExecuteKettleCommand(kettleCommand);
            });
        }

        public async void Run() {
            while (true) {
                var setupResult = await Setup();
                if (setupResult) {
                    while (IsConnected) {
                        Ping();
                        Thread.Sleep(1000);
                    }
                    Exit();

                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        _eventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionStatus { Type = ConnectionStatus.EventType.Disconnected });
                    });
                } else {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        _eventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionStatus { Type = ConnectionStatus.EventType.NotConnected });
                    });
                }
                Thread.Sleep(1000);
            }
        }


        public async Task<bool> Setup() {
            _serialTransport = new SerialTransport {
                CurrentSerialSettings = { PortName = "COM4", BaudRate = 57600, DtrEnable = false } // object initializer
            };

            _cmdMessenger = new CmdMessenger(_serialTransport, BoardType.Bit32);

            _cmdMessenger.Attach(OnUnknownCommand);
            _cmdMessenger.Attach((int)Command.Acknowledge, OnAcknowledge);
            _cmdMessenger.Attach((int)Command.Error, OnError);
            _cmdMessenger.Attach((int)Command.TempChange, OnTempChange);
            _cmdMessenger.Attach((int)Command.SsrChange, OnSsrChange);
            _cmdMessenger.Attach((int)Command.HeaterChange, OnHeaterChange);
            _cmdMessenger.Attach((int)Command.KettleResult, OnKettleResult);

            _cmdMessenger.NewLineReceived += NewLineReceived;
            _cmdMessenger.NewLineSent += NewLineSent;

            IsConnected = _cmdMessenger.Connect();


            if (IsConnected) {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _eventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionStatus { Type = ConnectionStatus.EventType.Connected });
                });
                
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

        public void ExecuteKettleCommand(KettleCommand kettleCommand) {
            Debug.WriteLine($"ExecuteKettleCommand: {kettleCommand.Index} {kettleCommand.Percentage}");

            Task.Run(() => {
                var kettleRequest = new SendCommand((int)Command.KettleRequest, (int)Command.KettleResult, 2000);
                kettleRequest.AddArgument(kettleCommand.Index);
                kettleRequest.AddArgument(kettleCommand.Percentage);
                var kettleResultCommand = _cmdMessenger.SendCommand(kettleRequest);
                var success = kettleRequest.Ok;
            });
            
        }

        // ------------------  C A L L B A C K S ---------------------

        private async void OnSsrChange(ReceivedCommand receivedCommand) {
            int.TryParse(receivedCommand.ReadStringArg(), out int ssrIndex);
            int.TryParse(receivedCommand.ReadStringArg(), out int ssrValue);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                _eventAggregator.GetEvent<SsrResultEvent>().Publish(new SsrResult { Index = ssrIndex, IsEngaged = ssrValue == 1 });
            });
        }

        private async void OnHeaterChange(ReceivedCommand receivedCommand) {
            int.TryParse(receivedCommand.ReadStringArg(), out int heaterIndex);
            int.TryParse(receivedCommand.ReadStringArg(), out int heaterValue);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                _eventAggregator.GetEvent<HeaterResultEvent>().Publish(new HeaterResult { Index = heaterIndex, IsEngaged = heaterIndex == 1 });
            });
        }

        private async void OnTempChange(ReceivedCommand receivedCommand) {
            int.TryParse(receivedCommand.ReadStringArg(), out int probeIndex);
            decimal.TryParse(receivedCommand.ReadStringArg(), out decimal temp);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                _eventAggregator.GetEvent<TemperatureResultEvent>().Publish(new TemperatureResult { Index = probeIndex, Value = temp });
            });
        }

        private async void OnKettleResult(ReceivedCommand receivedCommand) {
            int.TryParse(receivedCommand.ReadStringArg(), out int kettleIndex);
            int.TryParse(receivedCommand.ReadStringArg(), out int percentage);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                _eventAggregator.GetEvent<KettleResultEvent>().Publish(new KettleResult { Index = kettleIndex, Percentage = percentage });
            });
        }

        private void OnUnknownCommand(ReceivedCommand arguments) {
            Console.WriteLine("Command without attached callback received");
        }

        private async void OnAcknowledge(ReceivedCommand arguments) {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                _eventAggregator.GetEvent<ConnectionStatusEvent>().Publish(new ConnectionStatus { Type = ConnectionStatus.EventType.Ready });
            });
        }

        // Callback function that prints that the Arduino has experienced an error
        private void OnError(ReceivedCommand arguments) {
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

