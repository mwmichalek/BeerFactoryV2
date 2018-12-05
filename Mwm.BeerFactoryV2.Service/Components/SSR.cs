using Microsoft.IoT.DeviceCore.Pwm;
using Microsoft.IoT.Devices.Pwm;
using Mwm.BeerFactoryV2.Service.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Devices.Pwm;
using Windows.UI.Core;

namespace Mwm.BeerFactoryV2.Service.Components {
    public enum SsrId {
        HLT = 4,
        BK = 5
    }

    public class Ssr {

        public SsrId Id { get; set; }

        private IEventAggregator _eventAggregator;

        public int Pin { get; set; }

        public bool IsEngaged { get; set; }

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
                SendNotification();
            }
        }

        private GpioPin pin;
        private GpioPinValue pinValue = GpioPinValue.High;

        private bool isRunning = false;

        private int millisOn = 1000;
        private int millisOff = 1000;

        public Ssr(IEventAggregator eventAggregator, SsrId id) {
            Id = id;
            _eventAggregator = eventAggregator;
            Pin = (int)id;

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
            IsEngaged = true;
            SendNotification();
        }

        private void Off() {
            pin?.Write(GpioPinValue.Low);
            IsEngaged = false;
            SendNotification();
        }

        private void SendNotification() {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                _eventAggregator.GetEvent<SsrChangeEvent>().Publish(new SsrChange { Index = (int)Id, IsEngaged = IsEngaged, Percentage = _percentage });
            });
        }

        public void Stop() {
            isRunning = false;
        }
    }

}
