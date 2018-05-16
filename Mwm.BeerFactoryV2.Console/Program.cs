using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Dto;
using System;

using System.Threading;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Console {
    public class Program {
        static void Main(string[] args) {

            var runner = new ProgramRunner();
            runner.Run();

            System.Console.ReadLine();

        }

        
    }

    public class ProgramRunner {

 

        public void Run() {
            var controller = ArduinoController.Current;

            controller.ConnectionStatusEventHandler += HandleConnectionStatusEvent;
            controller.TemperatureResultEventHandler += HandleTemperatureResultEvent;

            Task.Run(() => controller.Run());
        }

        public void HandleTemperatureResultEvent(object sender, TemperatureResult tempertureResult) {
            System.Console.WriteLine($"TemperatureResult: Number[{tempertureResult.Number}] Value[{tempertureResult.Value}]");
        }

        public void HandleConnectionStatusEvent(object sender, ConnectionStatusEvent connectionStatusEvent) {
            System.Console.WriteLine($"ConnectionStatus: {connectionStatusEvent.Type}");
        }

    }
}
