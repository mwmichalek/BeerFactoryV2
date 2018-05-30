using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Dto {
    public class StatusResult {

        public List<PumpResult> PumpResults { get; set; }

        public List<TemperatureResult> TemperatureResults { get; set; }

        public List<KettleResult> KettleResults { get; set; }


    }

}
