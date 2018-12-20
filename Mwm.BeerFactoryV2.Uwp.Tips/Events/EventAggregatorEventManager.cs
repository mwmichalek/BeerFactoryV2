using Mwm.BeerFactoryV2.Service.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mwm.BeerFactoryV2.Uwp.Tips.Events {
    public class EventAggregatorEventManager : IEventManager {

        private IEventAggregator _eventAggregator;

        public EventAggregatorEventManager(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;
        }

        public void Publish<TEventPayload>(TEventPayload payload) where TEventPayload : IEventPayload {
            _eventAggregator.GetEvent<PubSubEvent<TEventPayload>>().Publish(payload);
        }

        public void Subscribe<TEventPayload>(Action<TEventPayload> eventAction) where TEventPayload : IEventPayload {
            _eventAggregator.GetEvent<PubSubEvent<TEventPayload>>().Subscribe(eventAction);
        }
    }
}
