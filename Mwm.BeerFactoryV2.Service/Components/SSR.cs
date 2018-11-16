using Microsoft.IoT.DeviceCore.Pwm;
using Microsoft.IoT.Devices.Pwm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Devices.Pwm;

namespace Mwm.BeerFactoryV2.Service.Components {
    public class SSR {

        public int Pin { get; set; }

        public int DutyCycleInMillis { get; set; } = 2000;

        public int Percentage { get; set; } = 50;

        private GpioPin pin;
        private GpioPinValue pinValue = GpioPinValue.High;

        private bool isRunning = false;

        private int millisOn = 1000;
        private int millisOff = 1000;

        public SSR(int pinNumber) {
            Pin = pinNumber;

            var gpio = GpioController.GetDefault();
            if (gpio != null) {
                pin = gpio.OpenPin(Pin);
                pin.SetDriveMode(GpioPinDriveMode.Output);
            }
        }

        public void Start() {
            isRunning = true;

            // Calculate On and Off durations
            decimal fraction = ((decimal)Percentage / 100.0m);
            millisOn = (int)(fraction * (decimal)DutyCycleInMillis);
            millisOff = DutyCycleInMillis - millisOn;

            // Call new thread to run
            Task.Run(() => Run());
        }

        private void Run() {
            while (isRunning) {
                On();
                Thread.Sleep(millisOn);
                Off();
                Thread.Sleep(millisOff);
            }
        }

        private void On() {
            pin?.Write(GpioPinValue.High);
        }

        private void Off() {
            pin?.Write(GpioPinValue.Low);
        }

        public void Stop() {
            isRunning = false;
        }
    }

}
