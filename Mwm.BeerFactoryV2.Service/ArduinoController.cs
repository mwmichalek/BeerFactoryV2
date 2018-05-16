using Mwm.BeerFactoryV2.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandMessenger;
using CommandMessenger.Transport.Serial;
using System.Threading;

namespace Mwm.BeerFactoryV2.Service {
    public class ArduinoController {

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
            HeaterChange,
            PumpChange
        };

        private SerialTransport _serialTransport;
        private CmdMessenger _cmdMessenger;
        public bool IsConnected { get; set; }

        private static ArduinoController _controllerInstance;

        //public event TemperatureChangeHandler TemperatureChange { get; set; }



        public EventHandler<ConnectionStatusEvent> ConnectionStatusEventHandler { get; set; }
        public EventHandler<TemperatureResult> TemperatureResultEventHandler { get; set; }

        

        private ArduinoController() {

        }

        public static ArduinoController Current {
            get {
                if (_controllerInstance == null)
                    _controllerInstance = new ArduinoController();
                return _controllerInstance;
            }
        }

        public void Run() {
            while (true) {
                if (Setup()) {
                    while (IsConnected) {
                        Ping();
                        Thread.Sleep(1000);
                    }
                    Exit();
                    ConnectionStatusEventHandler.Invoke(this, new ConnectionStatusEvent { Type = ConnectionStatusEvent.EventType.Disconnected });
                } else {
                    ConnectionStatusEventHandler.Invoke(this, new ConnectionStatusEvent { Type = ConnectionStatusEvent.EventType.NotConnected });
                }

                Thread.Sleep(1000);
            }
        }


        public bool Setup() {
            _serialTransport = new SerialTransport {
                CurrentSerialSettings = { PortName = "COM4", BaudRate = 57600, DtrEnable = false } // object initializer
            };

            _cmdMessenger = new CmdMessenger(_serialTransport, BoardType.Bit32);

            _cmdMessenger.Attach(OnUnknownCommand);
            _cmdMessenger.Attach((int)Command.Acknowledge, OnAcknowledge);
            _cmdMessenger.Attach((int)Command.Error, OnError);
            _cmdMessenger.Attach((int)Command.TempChange, OnTempChange);
            _cmdMessenger.Attach((int)Command.HeaterChange, OnHeaterChange);

            _cmdMessenger.NewLineReceived += NewLineReceived;
            _cmdMessenger.NewLineSent += NewLineSent;

            IsConnected = _cmdMessenger.Connect();

            if (IsConnected) {
                ConnectionStatusEventHandler.Invoke(this, new ConnectionStatusEvent { Type = ConnectionStatusEvent.EventType.Connected });
                
                //TODO: Request Status
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

        public StatusResult RequestStatus() {
            return new StatusResult();
        }

        // ------------------  C A L L B A C K S ---------------------

        private void OnHeaterChange(ReceivedCommand receivedCommand) {
            Console.WriteLine(@"Received HeaterChange > " + receivedCommand.CommandString());
        }

        private void OnTempChange(ReceivedCommand receivedCommand) {
            int.TryParse(receivedCommand.ReadStringArg(), out int probeNumber);
            decimal.TryParse(receivedCommand.ReadStringArg(), out decimal temp);

            TemperatureResultEventHandler.Invoke(this, new TemperatureResult { Number = probeNumber, Value = temp });

            //Console.WriteLine($"Received TempChange > num[{probeNumber}] temp[{temp}]");
        }

        // Called when a received command has no attached function.
        void OnUnknownCommand(ReceivedCommand arguments) {
            Console.WriteLine("Command without attached callback received");
        }

        // Callback function that prints that the Arduino has acknowledged
        void OnAcknowledge(ReceivedCommand arguments) {
            Console.WriteLine(" Arduino is ready");
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


















// *** SendandReceive ***

// This example expands the previous SendandReceive example. The PC will now send multiple float values
// and wait for a response from the Arduino. 
// It adds a demonstration of how to:
// - Send multiple parameters, and wait for response
// - Receive multiple parameters
// - Add logging events on data that has been sent or received

//using System;
//using CommandMessenger;
//using CommandMessenger.Transport.Serial;

//namespace SendAndReceiveArguments {
//    // This is the list of recognized commands. These can be commands that can either be sent or received. 
//    // In order to receive, attach a callback function to these events
//    enum Command {
//        Acknowledge,
//        Error,
//        Echo,
//        EchoResult,
//        Status,
//        StatusResult,
//        TempChange,
//        HeaterChange
//    };

//    public class Echo {
//        public bool RunLoop { get; set; }
//        private SerialTransport _serialTransport;
//        private CmdMessenger _cmdMessenger;

//        // ------------------ M A I N  ----------------------

//        // Setup function
//        public bool Setup() {
//            // Create Serial Port object
//            // Note that for some boards (e.g. Sparkfun Pro Micro) DtrEnable may need to be true.
//            _serialTransport = new SerialTransport {
//                CurrentSerialSettings = { PortName = "COM4", BaudRate = 57600, DtrEnable = false } // object initializer
//            };

//            // Initialize the command messenger with the Serial Port transport layer
//            // Set if it is communicating with a 16- or 32-bit Arduino board
//            _cmdMessenger = new CmdMessenger(_serialTransport, BoardType.Bit32);

//            // Attach the callbacks to the Command Messenger
//            AttachCommandCallBacks();

//            // Attach to NewLinesReceived for logging purposes
//            _cmdMessenger.NewLineReceived += NewLineReceived;

//            // Attach to NewLineSent for logging purposes
//            _cmdMessenger.NewLineSent += NewLineSent;

//            // Start listening
//            return _cmdMessenger.Connect();
//        }

//        // Loop function
//        public void Loop() {
//            // Create command FloatAddition, which will wait for a return command FloatAdditionResult
//            var command = new SendCommand((int)Command.Echo, (int)Command.EchoResult, 1000);


//            command.AddArgument(DateTime.Now.ToString("mm:ss"));

//            // Send command
//            var echoResultCommand = _cmdMessenger.SendCommand(command);

//            // Check if received a (valid) response
//            if (echoResultCommand.Ok) {
//                // Read returned argument
//                var msg = echoResultCommand.ReadStringArg();
//                //Console.WriteLine($"Received {msg}");
//                RunLoop = true;

//            } else {
//                Console.WriteLine("No response!");

//                // Stop running loop
//                RunLoop = false;
//            }
//        }

//        // Exit function
//        public void Exit() {
//            // Stop listening
//            _cmdMessenger.Disconnect();

//            // Dispose Command Messenger
//            _cmdMessenger.Dispose();

//            // Dispose Serial Port object
//            _serialTransport.Dispose();
//        }

//        /// Attach command call backs. 
//        private void AttachCommandCallBacks() {
//            _cmdMessenger.Attach(OnUnknownCommand);
//            _cmdMessenger.Attach((int)Command.Acknowledge, OnAcknowledge);
//            _cmdMessenger.Attach((int)Command.Error, OnError);
//            _cmdMessenger.Attach((int)Command.TempChange, OnTempChange);
//            _cmdMessenger.Attach((int)Command.HeaterChange, OnHeaterChange);
//        }

//        private void OnHeaterChange(ReceivedCommand receivedCommand) {
//            Console.WriteLine(@"Received HeaterChange > " + receivedCommand.CommandString());
//        }

//        private void OnTempChange(ReceivedCommand receivedCommand) {
//            var msg = receivedCommand.ReadStringArg();
//            int tempNumber = receivedCommand.ReadBinInt32Arg();
//            double temp = receivedCommand.ReadDoubleArg();

//            Console.WriteLine($"Received TempChange > {msg} {tempNumber} {temp}");
//        }

//        // ------------------  C A L L B A C K S ---------------------

//        // Called when a received command has no attached function.
//        void OnUnknownCommand(ReceivedCommand arguments) {
//            Console.WriteLine("Command without attached callback received");
//        }

//        // Callback function that prints that the Arduino has acknowledged
//        void OnAcknowledge(ReceivedCommand arguments) {
//            Console.WriteLine(" Arduino is ready");
//        }

//        // Callback function that prints that the Arduino has experienced an error
//        void OnError(ReceivedCommand arguments) {
//            Console.WriteLine(" Arduino has experienced an error");
//        }

//        // Log received line to console
//        private void NewLineReceived(object sender, CommandEventArgs e) {
//            Console.WriteLine(@"Received > " + e.Command.CommandString());
//        }

//        // Log sent line to console
//        private void NewLineSent(object sender, CommandEventArgs e) {
//            Console.WriteLine(@"Sent > " + e.Command.CommandString());
//        }
//    }
//}
