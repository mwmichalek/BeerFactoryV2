﻿using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {
    public class TemperatureResult : IEventPayload {

        public int Index { get; set; }

        public decimal Value { get; set; }
    }

    public class TemperatureResultEvent : PubSubEvent<TemperatureResult> {

    }
}
