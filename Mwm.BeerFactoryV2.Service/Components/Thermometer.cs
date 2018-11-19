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
        public Thermometer(ThermometerId id) {
            Id = id;
        }

        public ThermometerId Id { get; private set; }

        public decimal Change { get; set; }

        private decimal temperature;

        public decimal Temperature {
            get { return temperature; }
            set {
                Change = value - temperature;
                Timestamp = DateTime.Now;
                temperature = value;
            }
        }

        public DateTime Timestamp { get; set; }
    }

    public static class ThermometerHelper {

        public static Thermometer GetById(this List<Thermometer> thermometers, ThermometerId thermometerId) {
            return thermometers.SingleOrDefault(t => t.Id == thermometerId);
        }

    }
}
