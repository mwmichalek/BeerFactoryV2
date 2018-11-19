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
    public class Ssr {

        public int Pin { get; set; }

        private int _dutyCycleInMillis = 2000;
        public int DutyCycleInMillis {
            get {
                return _dutyCycleInMillis;
            }
            set {
                _dutyCycleInMillis = value;
                CalculateDurations();
            }
        }

        private int _percentage = 50;

        public int Percentage {
            get {
                return _percentage;
            }
            set {
                _percentage = value;
                CalculateDurations();
            }
        }

        private GpioPin pin;
        private GpioPinValue pinValue = GpioPinValue.High;

        private bool isRunning = false;

        private int millisOn = 1000;
        private int millisOff = 1000;

        public Ssr(int pinNumber) {
            Pin = pinNumber;

            var gpio = GpioController.GetDefault();
            if (gpio != null) {
                pin = gpio.OpenPin(Pin);
                pin.SetDriveMode(GpioPinDriveMode.Output);
            }
        }

        public void Start() {
            isRunning = true;
            CalculateDurations();

            // Call new thread to run
            Task.Run(() => Run());
        }

        private void CalculateDurations() {
            // Calculate On and Off durations
            decimal fraction = ((decimal)_percentage / 100.0m);
            millisOn = (int)(fraction * (decimal)_dutyCycleInMillis);
            millisOff = _dutyCycleInMillis - millisOn;
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
