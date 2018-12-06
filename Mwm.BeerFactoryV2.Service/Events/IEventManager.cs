using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Service.Events {

    public interface IEventManager {

        void Subscribe<TEventPayload>(Action<TEventPayload> eventAction) where TEventPayload : IEventPayload;

        void Publish<TEventPayload>(TEventPayload payload) where TEventPayload : IEventPayload;

    }

}
