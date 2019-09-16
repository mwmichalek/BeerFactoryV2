﻿using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {


    //************************************************* CHANGE EVENTS ***********************************************

    public abstract class BeerFactoryEventHandler {

        private IEventAggregator _eventAggregator;

        public BeerFactoryEventHandler(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<ThermometerChangeEvent>().Subscribe(ThermometerChangeOccured);
            _eventAggregator.GetEvent<TemperatureChangeEvent>().Subscribe(TemperatureChangeOccured);
            _eventAggregator.GetEvent<PidRequestEvent>().Subscribe(PidRequestOccured);
            _eventAggregator.GetEvent<PumpRequestEvent>().Subscribe(PumpRequestOccured);
            _eventAggregator.GetEvent<ConnectionStatusEvent>().Subscribe(ConnectionStatusOccured);
        }

        public virtual void TemperatureChangeOccured(TemperatureChange obj) { }

        public virtual void ThermometerChangeOccured(ThermometerChange thermometerChange) { }

        public virtual void PidRequestOccured(PidRequest pidRequest) { }

        public virtual void PumpRequestOccured(PumpRequest pumpRequest) { }

        public virtual void ConnectionStatusOccured(ConnectionStatus connectionStatus) { }

        public void TemperatureChangeFired(TemperatureChange temperatureChange) {
            _eventAggregator.GetEvent<TemperatureChangeEvent>().Publish(temperatureChange);
        }

        public void ThermometerChangeFired(ThermometerChange thermometerChange) {
            _eventAggregator.GetEvent<ThermometerChangeEvent>().Publish(thermometerChange);
        }

        public void PumpChangeFired(PumpChange pumpChange) {
            _eventAggregator.GetEvent<PumpChangeEvent>().Publish(pumpChange);
        }

        public void PidChangeFired(PidChange pidChange) {
            _eventAggregator.GetEvent<PidChangeEvent>().Publish(pidChange);
        }

        public void SsrChangeFired(SsrChange ssrChange) {
            _eventAggregator.GetEvent<SsrChangeEvent>().Publish(ssrChange);
        }
    }



    //************************************************* REQUEST EVENTS ***********************************************

    //public abstract class DisplayEventSubscriber {

    //    private IEventAggregator _eventAggregator;

    //    public DisplayEventSubscriber(IEventAggregator eventAggregator) {
    //        _eventAggregator = eventAggregator;

    //        _eventAggregator.GetEvent<TemperatureChangeEvent>().Subscribe(TemperatureChangeOccured);
    //        _eventAggregator.GetEvent<PumpChangeEvent>().Subscribe(PumpChangeOccured);
    //        _eventAggregator.GetEvent<PidChangeEvent>().Subscribe(PidChangeOccured);
    //        _eventAggregator.GetEvent<SsrChangeEvent>().Subscribe(SsrChangeOccured);
    //        _eventAggregator.GetEvent<ConnectionStatusEvent>().Subscribe(ConnectionStatusOccured);
    //    }

    //    public virtual void TemperatureChangeOccured(TemperatureChange temperatureChange) { }

    //    public virtual void PumpChangeOccured(PumpChange pumpChange) { }

    //    public virtual void PidChangeOccured(PidChange pidChange) { }

    //    public virtual void SsrChangeOccured(SsrChange ssrChange) { }

    //    public virtual void ConnectionStatusOccured(ConnectionStatus connectionStatus) { }



    //}

}
