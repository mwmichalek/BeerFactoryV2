using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {

    public class PidRequestEvent : PubSubEvent<PidRequest> {

    }


    public enum PidMode {
        Temperature,
        Percentage
    }


    public class PidRequest {

        public bool IsEngaged { get; set; }

        public PidMode PidMode { get; set; }

        public int Value { get; set; }

    }
}
