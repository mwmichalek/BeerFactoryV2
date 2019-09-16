using Mwm.BeerFactoryV2.Service.Components;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {

    public class SsrChangeEvent : PubSubEvent<SsrChange> { }

    public class SsrChange : IEventPayload {

        public SsrId Id { get; set; }

        public bool IsEngaged { get; set; }

        public int Percentage { get; set; }

    }

}
