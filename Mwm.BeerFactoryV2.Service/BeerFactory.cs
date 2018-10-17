using Mwm.BeerFactoryV2.Service.Components;
using Mwm.BeerFactoryV2.Service.Phases;
using Prism.Events;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service {

    public interface IBeerFactory {

        void AddBeerFactoryPhase(BeerFactoryPhase beerFactoryPhase);

    }
    public partial class BeerFactory : IBeerFactory {

        private ILogger Logger { get; set; }

        private List<BeerFactoryPhase> _beerFactoryPhases = new List<BeerFactoryPhase>();

        private List<Thermometer> _thermometers { get; set; } = new List<Thermometer>();

        public BeerFactory(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;

            Logger = Log.Logger;

            foreach (var index in Enumerable.Range((int)ThermometerId.BK, (int)ThermometerId.FERM)) {
                _thermometers.Add(new Thermometer((ThermometerId)index, _eventAggregator));
            }

            ConfigureEvents();
        }

        public void AddBeerFactoryPhase(BeerFactoryPhase beerFactoryPhase) {
            _beerFactoryPhases.Add(beerFactoryPhase);
        }

    }
}
