using System;
using System.Collections.Generic;

namespace CDTag.Common
{
    internal class CompositeEvent : ICompositeEvent
    {
        private readonly List<Action<object>> _handlers = new List<Action<object>>();
        private readonly object _handlersLocker = new object();

        public void Subscribe(Action<object> handler)
        {
            lock (_handlersLocker)
            {
                _handlers.Add(handler);
            }
        }

        public void Publish(object payload)
        {
            lock (_handlersLocker)
            {
                foreach (var handler in _handlers)
                {
                    handler(payload);
                }
            }
        }
    }
}
