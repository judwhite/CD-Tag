using System;
using System.Collections.Generic;

namespace CDTag.Common
{
    internal class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, ICompositeEvent> _events = new Dictionary<Type, ICompositeEvent>();
        private readonly object _eventsLocker = new object();

        public ICompositeEvent GetEvent<T>()
        {
            Type type = typeof(T);

            lock (_eventsLocker)
            {
                if (_events.ContainsKey(type))
                    return _events[type];

                var compositeEvent = new CompositeEvent();
                _events.Add(type, compositeEvent);
                return compositeEvent;
            }
        }
    }
}
