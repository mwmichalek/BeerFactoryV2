using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Phases {
    public class MainPhase : BeerFactoryPhaseBase {

        public MainPhase(IEventAggregator eventAggregator) : base(eventAggregator) {

        }
    }
}
