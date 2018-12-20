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

    }
    public partial class BeerFactory : IBeerFactory {

        private ILogger Logger { get; set; }

        private List<Phase> _phases = new List<Phase>();

        private PidController _hltPidController;

        private List<Thermometer> _thermometers { get; set; } = new List<Thermometer>();

        public BeerFactory(IEventManager eventManager, Thermometer[] thermometers) {
            _eventManager = eventManager;
            _thermometers = thermometers.ToList();
            Logger = Log.Logger;

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
    }
}
