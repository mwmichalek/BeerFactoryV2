using Mwm.BeerFactoryV2.Service.Pid;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {

    public class PidChangeEvent : PubSubEvent<PidChange> { }
    public class PidChange {

        public PidControllerId Id { get; set; }

        public PidMode PidMode { get; set; }

        public int Value { get; set; }

    }
}
