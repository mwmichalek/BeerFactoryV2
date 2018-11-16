using Mwm.BeerFactoryV2.Service.Components;
using Mwm.BeerFactoryV2.Service.Events;
using Mwm.BeerFactoryV2.Service.Pid;
using Prism.Events;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.Devices.Pwm;
using Windows.Devices.Gpio;
using Microsoft.IoT.DeviceCore.Pwm;
using Microsoft.IoT.Devices.Pwm;

namespace Mwm.BeerFactoryV2.Service {

    public interface IBeerFactory {

        Task UpdateTemperatureAsync(ThermometerId id, decimal temperature);

    }
    public partial class BeerFactory : IBeerFactory {

        private ILogger Logger { get; set; }

        private List<Phase> _phases = new List<Phase>();

        private PidController _hltPidController = new PidController(1, 1, 1, 0, 100, 80);

        private List<Thermometer> _thermometers { get; set; } = new List<Thermometer>();

        

        public BeerFactory(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;

            Logger = Log.Logger;

            for (int index = 1; index <= (int)ThermometerId.FERM; index++ )
                _thermometers.Add(new Thermometer((ThermometerId)index));

            _phases.Add(new Phase(PhaseId.FillStrikeWater, 20));
            _phases.Add(new Phase(PhaseId.HeatStrikeWater, 40));
            _phases.Add(new Phase(PhaseId.Mash, 90));
            _phases.Add(new Phase(PhaseId.MashOut, 90));
            _phases.Add(new Phase(PhaseId.Sparge, 60));
            _phases.Add(new Phase(PhaseId.Boil, 90));
            _phases.Add(new Phase(PhaseId.Chill, 30));

            var ssr = new SSR(4);
            ssr.Start();
            

            

            Task.Run(() => {
                FakePidShit();
            });

            //var pwm = new Pwm();
            //pwm.Setup(4, 50, .01);


            ConfigureEvents();
        }

        

        private double Bullshit = 70;

        private void FakePidShit() {

            while (true) {

                Debug.WriteLine($"PID: {_hltPidController.Process(Bullshit)}, Temp: {Bullshit}");

                Bullshit += .5;
                //pinValue = (pinValue == GpioPinValue.High) ? GpioPinValue.Low : GpioPinValue.High;
                //pin.Write(pinValue);
                Thread.Sleep(2000);
            }

        }

        public async Task UpdateTemperatureAsync(ThermometerId id, decimal temperature) {
            var thermometer = _thermometers.SingleOrDefault(t => t.Id == id);
            if (thermometer != null) {
                thermometer.Temperature = temperature;

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    _eventAggregator.GetEvent<TemperatureChangeEvent>().Publish(new TemperatureChange { Index = (int)thermometer.Id, Value = thermometer.Temperature });
                });
            }

        }

    }
}
