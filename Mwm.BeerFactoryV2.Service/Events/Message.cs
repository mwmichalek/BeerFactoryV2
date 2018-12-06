using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {
    public class Message : IEventPayload {

        public int Index { get; set; }

        public string Body { get; set; }

        public int Percentage { get; set; }
    }

   
}
