using Mwm.BeerFactoryV2.Service.Events;
using Prism.Events;
using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Humpty.ViewModels {
    public abstract class DisplayEventHandlerViewModelBase : ViewModelBase {
        private IEventAggregator _eventAggregator;

        protected DisplayEventHandlerViewModelBase(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;


            _eventAggregator.GetEvent<TemperatureChangeEvent>().Subscribe(TemperatureChangeOccured, ThreadOption.UIThread);
            _eventAggregator.GetEvent<PumpChangeEvent>().Subscribe(PumpChangeOccured, ThreadOption.UIThread);
            _eventAggregator.GetEvent<PidChangeEvent>().Subscribe(PidChangeOccured, ThreadOption.UIThread);
            _eventAggregator.GetEvent<SsrChangeEvent>().Subscribe(SsrChangeOccured, ThreadOption.UIThread);
            _eventAggregator.GetEvent<ConnectionStatusEvent>().Subscribe(ConnectionStatusOccured, ThreadOption.UIThread);
        }

        public virtual void TemperatureChangeOccured(TemperatureChange temperatureChange) { }

        public virtual void PumpChangeOccured(PumpChange pumpChange) { }

        public virtual void PidChangeOccured(PidChange pidChange) { }

        public virtual void SsrChangeOccured(SsrChange ssrChange) { }

        public virtual void ConnectionStatusOccured(ConnectionStatus connectionStatus) { }

        public void PidRequestFire(PidRequest pidRequest) {
            _eventAggregator.GetEvent<PidRequestEvent>().Publish(pidRequest);
        }

        public void PumpRequestFire(PumpRequest pumpRequest) {
            _eventAggregator.GetEvent<PumpRequestEvent>().Publish(pumpRequest);
        }

    }
}
