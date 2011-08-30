using System;

namespace CDTag.Common
{
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
}
