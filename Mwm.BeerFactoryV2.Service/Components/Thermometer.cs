using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Components {


    public enum ThermometerId {
        HLT = 1,
        MT_IN = 2,
        MT = 3,
        MT_OUT = 4,
        BK = 5,
        HEX_IN = 6,
        HEX_OUT = 7,
        FERM = 8
    }

    public class Thermometer {

        private IEventAggregator _eventAggregator;

        public Thermometer(ThermometerId id, IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;
            Id = id;
        }

        public ThermometerId Id { get; private set; }

        public decimal Change { get; set; }

        private decimal temperature;

        public decimal Temperature {
            get { return temperature; }
            set {
                var newValue = value;
                Change = newValue - temperature;
                Timestamp = DateTime.Now;
            }
        }

        public DateTime Timestamp { get; set; }
    }
}
