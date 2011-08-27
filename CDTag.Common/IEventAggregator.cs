using System;
using System.Collections.Generic;

namespace CDTag.Common
{
    /// <summary>
    /// IEventAggregator
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// Gets the specified event.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <returns>The specified event.</returns>
        ICompositeEvent GetEvent<T>();
    }

    /// <summary>
    /// ICompositeEvent
    /// </summary>
    public interface ICompositeEvent
    {
        /// <summary>
        /// Subscribes to an event.
        /// </summary>
        /// <param name="handler">The handler.</param>
        void Subscribe(Action<object> handler);

        /// <summary>
        /// Publishes an event.
        /// </summary>
        /// <param name="payload">The payload.</param>
        void Publish(object payload);
    }

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
