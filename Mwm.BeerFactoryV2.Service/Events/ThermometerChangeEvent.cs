﻿using Common.Events;
using Common.Ids;
using Mwm.BeerFactoryV2.Service.Components;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {

    public class ThermometerChangeEvent : PubSubEvent<ThermometerChange> { }
    
}
