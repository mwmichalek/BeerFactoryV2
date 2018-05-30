using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Dto {
    public class ConnectionStatusEvent {

        public enum EventType {
            Disconnected,
            Connected,
            NotConnected,
            Ready
        }

        public EventType Type { get; set; }
    }
}
