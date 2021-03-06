﻿using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {
    public class PumpResult : IEventPayload {

        public int Index { get; set;}

        public bool IsEngaged { get; set; }
    }

    public class PumpResultEvent : PubSubEvent<PumpResult> {

    }
}
