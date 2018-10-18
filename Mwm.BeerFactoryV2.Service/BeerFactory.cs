using Mwm.BeerFactoryV2.Service.Components;
using Mwm.BeerFactoryV2.Service.Events;
using Mwm.BeerFactoryV2.Service.Phases;
using Prism.Events;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Mwm.BeerFactoryV2.Service {

    public interface IBeerFactory {

        void AddBeerFactoryPhase(BeerFactoryPhase beerFactoryPhase);

        Task UpdateTemperatureAsync(ThermometerId id, decimal temperature);

    }
    public partial class BeerFactory : IBeerFactory {

        private ILogger Logger { get; set; }

        private List<BeerFactoryPhase> _beerFactoryPhases = new List<BeerFactoryPhase>();

        private List<Thermometer> _thermometers { get; set; } = new List<Thermometer>();

        public BeerFactory(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;

            Logger = Log.Logger;

            for (int index = 1; index <= (int)ThermometerId.FERM; index++ )
                _thermometers.Add(new Thermometer((ThermometerId)index));
   
            ConfigureEvents();
        }

        public void AddBeerFactoryPhase(BeerFactoryPhase beerFactoryPhase) {
            _beerFactoryPhases.Add(beerFactoryPhase);
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
