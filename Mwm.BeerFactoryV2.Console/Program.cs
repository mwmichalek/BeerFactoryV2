using Mwm.BeerFactoryV2.Service;
using System;

using System.Threading;


namespace Mwm.BeerFactoryV2.Console {
    public class Program {
        static void Main(string[] args) {
            while (true) {
                var controller = new ArduinoController { RunLoop = true };
                if (controller.Setup()) {
                    while (controller.RunLoop) {
                        controller.Loop();
                        Thread.Sleep(1000);
                    }
                    controller.Exit();
                    System.Console.WriteLine("Disconnected!");
                } else {
                    System.Console.WriteLine("Not connected!");
                }

                Thread.Sleep(1000);
            }

        }
    }
}
