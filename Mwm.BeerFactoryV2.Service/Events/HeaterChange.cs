using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {

    public class HeaterChangeEvent : PubSubEvent<HeaterChange> { }

    public class HeaterChange : IEventPayload {

        public int Index { get; set; }

        public bool IsEngaged { get; set; }
    }

    
}
