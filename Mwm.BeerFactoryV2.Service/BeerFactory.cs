using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service {

    public interface IBeerFactory {

    }
    public class BeerFactory : IBeerFactory {

        private ILogger Logger { get; set; }

        public BeerFactory() {
            Logger = Log.Logger;

            Logger.Information("Suck it");
        }



    }
}
