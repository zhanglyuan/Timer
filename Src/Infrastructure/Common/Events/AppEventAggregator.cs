using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Events
{
    public class AppEventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, EventBase> _events = new Dictionary<Type, EventBase>();

        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        public TEventType GetEvent<TEventType>()
        where TEventType : EventBase, new()
        {
            if (!_events.TryGetValue(typeof(TEventType), out EventBase eventInstance))
            {
                lock (_events)
                {
                    if (!_events.TryGetValue(typeof(TEventType), out eventInstance))
                    {
                        eventInstance = new TEventType()
                        {
                            SynchronizationContext = _synchronizationContext,
                        };
                        _events[typeof(TEventType)] = eventInstance;
                    }
                }
            }

            return (TEventType)eventInstance;
        }
    }
}