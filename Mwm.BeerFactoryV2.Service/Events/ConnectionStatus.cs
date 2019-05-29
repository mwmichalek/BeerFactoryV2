using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {

    public class ConnectionStatusEvent : PubSubEvent<ConnectionStatus> { }

    public enum Status {
        Disconnected,
        Connected,
        NotConnected,
        Ready
    }

    public class ConnectionStatus : IEventPayload {

        public Status Status { get; set; }
    }

    
}
