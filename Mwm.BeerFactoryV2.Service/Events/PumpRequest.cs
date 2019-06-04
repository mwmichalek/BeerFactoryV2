using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {

    public class PumpRequestEvent : PubSubEvent<PumpRequest> { }

    public class PumpRequest : IEventPayload {

        public int Index { get; set; }

        public bool IsEngaged { get; set; }
    }
}
