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
}
