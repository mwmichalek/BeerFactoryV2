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
using Prism.Mvvm;

namespace Mwm.BeerFactoryV2.Service {

    public interface IBeerFactory {
        decimal TemperatureOne { get; set; }
    }
    public partial class BeerFactory : BindableBase, IBeerFactory {

        private ILogger Logger { get; set; }

        private List<Phase> _phases = new List<Phase>();

        private PidController _hltPidController;

        private IEventAggregator _eventAggregator;

        private List<Thermometer> _thermometers { get; set; } = new List<Thermometer>();

        private decimal _temperatureOne = 0;
        public decimal TemperatureOne {
            get { return _temperatureOne; }
            set { SetProperty(ref _temperatureOne, value); }
        }

        public BeerFactory(IEventAggregator eventAggregator, Thermometer[] thermometers) {
            Logger = Log.Logger;

            _eventAggregator = eventAggregator;

            //_eventAggregator.GetEvent<ThermometerChangeEvent>().Subscribe(PrintStatusPub, ThreadOption.PublisherThread);
            //_eventAggregator.GetEvent<ThermometerChangeEvent>().Subscribe(PrintStatusBack, ThreadOption.BackgroundThread);
            _eventAggregator.GetEvent<ThermometerChangeEvent>().Subscribe(PrintStatusUi, ThreadOption.UIThread);

            _thermometers = thermometers.ToList();
            

            //for (int index = 1; index <= (int)ThermometerId.FERM; index++ )
            //    _thermometers.Add(new Thermometer((ThermometerId)index));

            _phases.Add(new Phase(PhaseId.FillStrikeWater, 20));
            _phases.Add(new Phase(PhaseId.HeatStrikeWater, 40));
            _phases.Add(new Phase(PhaseId.Mash, 90));
            _phases.Add(new Phase(PhaseId.MashOut, 90));
            _phases.Add(new Phase(PhaseId.Sparge, 60));
            _phases.Add(new Phase(PhaseId.Boil, 90));
            _phases.Add(new Phase(PhaseId.Chill, 30));

            //var hltSsr = new Ssr(eventManager, SsrId.HLT);
            //hltSsr.Percentage = 25;
            //hltSsr.Start();

            //var bkSsr = new Ssr(eventManager, SsrId.BK);
            //bkSsr.Percentage = 5;
            //bkSsr.Start();

            ConfigureEvents();

            //_hltPidController = new PidController(hltSsr, _thermometers.GetById(ThermometerId.MT_IN));
            //_hltPidController.GainProportional = 18;
            //_hltPidController.GainIntegral = 1.5;
            //_hltPidController.GainDerivative = 22.5;

            //_hltPidController.SetPoint = 120;
            //_hltPidController.Start();

        }

        private void PrintStatusUi(ThermometerChange thermometerChange) {
            Logger.Information($"TemperatureChangeUi: {thermometerChange.Index} {thermometerChange.Value}");
            if (thermometerChange.Index == 1)
                TemperatureOne = thermometerChange.Value;
        }
    }
}
