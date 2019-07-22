using Mwm.BeerFactoryV2.Service.Events;
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

    public class Thermometer : BeerFactoryEventHandler {

        private List<ThermometerChange> _thermometerChange = new List<ThermometerChange>();

        public ThermometerId Id { get; private set; }

        public decimal Change { get; set; }

        private decimal _temperature;

        private decimal _changeThreshold = 0.05m;

        private int _changeWindowInMillis = 1000;

        private int _changeEventRetentionInMins = 60 * 6;

        public Thermometer(IEventAggregator eventAggregator, ThermometerId id) : base(eventAggregator) {
            Id = id;
        }

        public Thermometer(IEventAggregator eventAggregator, ThermometerId id, int changeThreshold, int changeWindowInMillis, int changeEventRetentionInMins) : base(eventAggregator) {
            _changeThreshold = changeThreshold;
            _changeWindowInMillis = changeWindowInMillis;
            _changeWindowInMillis = changeWindowInMillis;
            Id = id;
        }

        public decimal Temperature {
            get { return _temperature; }
            set {
                //Change = value - _temperature;
                Timestamp = DateTime.Now;
                _temperature = value;
            }
        }

        public DateTime Timestamp { get; set; }

        public override void ThermometerChangeOccured(ThermometerChange thermometerChange) {
            if (thermometerChange.Id == Id) {
                // Determin Change - Get all changes at least this old, order by newest, take first
                var earliestTimeOfChange = DateTime.Now.AddMilliseconds(-_changeWindowInMillis);
                var previousChange = _thermometerChange.Where(tc => tc.Timestamp < earliestTimeOfChange).OrderByDescending(tc => tc.Timestamp).FirstOrDefault();
                if (previousChange != null)
                    Change = thermometerChange.Value - previousChange.Value;

                // Determine Retention
                var oldestTimeOfChange = DateTime.Now.AddMinutes(-_changeEventRetentionInMins);
                var changesToRemove = _thermometerChange.RemoveAll(tc => tc.Timestamp < oldestTimeOfChange);

                _thermometerChange.Add(thermometerChange);

                // If change is big enough, broadcast Temperature Change
                if (Math.Abs(Change) > _changeThreshold) {
                    TemperatureChangeFired(new TemperatureChange {
                        Id = thermometerChange.Id,
                        Change = Change,
                        PercentChange = Change / previousChange.Value * 100,
                        Timestamp = thermometerChange.Timestamp
                    });
                }
            }
        }

    }

    public static class ThermometerHelper {

        public static Thermometer GetById(this List<Thermometer> thermometers, ThermometerId thermometerId) {
            return thermometers.SingleOrDefault(t => t.Id == thermometerId);
        }

    }
}
