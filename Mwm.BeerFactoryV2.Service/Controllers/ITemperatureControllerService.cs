using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Controllers {
    public interface ITemperatureControllerService {

        Task Run();
    }

    public abstract class TemperatureControllerService : ITemperatureControllerService {

        //TODO: (Michalek) Move commmon functionality here.

        public abstract Task Run();
    }
}
