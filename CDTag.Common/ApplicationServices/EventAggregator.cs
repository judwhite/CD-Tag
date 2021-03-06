﻿using System;
using System.Collections.Generic;

namespace CDTag.Common.ApplicationServices
{
    internal class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, List<Action<object>>> _handlers = new Dictionary<Type, List<Action<object>>>();
        private readonly object _handlersLocker = new object();

        public void Subscribe<T>(Action<T> handler)
        {
            lock (_handlersLocker)
            {
                List<Action<object>> list;
                if (!_handlers.TryGetValue(typeof(T), out list))
                {
                    list = new List<Action<object>>();
                    _handlers.Add(typeof(T), list);
                }

                list.Add(o => handler((T)o));
            }
        }

        public void Publish<T>(T payload)
        {
            lock (_handlersLocker)
            {
                List<Action<object>> list;
                if (!_handlers.TryGetValue(typeof(T), out list))
                    return;

                foreach (var handler in list)
                {
                    handler(payload);
                }
            }
        }
    }
}
