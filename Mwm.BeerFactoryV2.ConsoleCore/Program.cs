using Mwm.BeerFactoryV2.Service;
using Mwm.BeerFactoryV2.Service.Dto;
using System;

using System.Threading;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.ConsoleCore {
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
            controller.SsrResultEventHandler += HandleSsrResultEvent;
            controller.HeaterResultEventHandler += HandleHeaterResultEvent;

            Task.Run(() => controller.Run());
        }

        public void HandleTemperatureResultEvent(object sender, TemperatureResult tempertureResult) {
            System.Console.WriteLine($"TemperatureResult: Index[{tempertureResult.Index}] Value[{tempertureResult.Value}]");
        }

        public void HandleConnectionStatusEvent(object sender, ConnectionStatusEvent connectionStatusEvent) {
            System.Console.WriteLine($"ConnectionStatus: {connectionStatusEvent.Type}");
        }

        public void HandleSsrResultEvent(object sender, SsrResult ssrResult) {
            System.Console.WriteLine($"SsrResult: Index[{ssrResult.Index}] IsEnaged[{ssrResult.IsEngaged}]");
        }

        public void HandleHeaterResultEvent(object sender, HeaterResult heaterResult) {
            System.Console.WriteLine($"HeaterResult: Index[{heaterResult.Index}] IsEnaged[{heaterResult.IsEngaged}]");
        }

    }
}
