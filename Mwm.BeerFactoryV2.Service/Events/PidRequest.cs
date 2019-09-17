using Mwm.BeerFactoryV2.Service.Pid;
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

        public PidControllerId Id { get; set; }

        public bool IsEngaged { get; set; }

        public PidMode PidMode { get; set; }

        public int SetPoint { get; set; }

    }
}
