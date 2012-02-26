using System;
using CDTag.Common.Dispatcher;

namespace CDTag.ViewModel.Tests
{
    /// <summary>
    /// UnitTestDispatcher
    /// </summary>
    public class UnitTestDispatcher : IDispatcher
    {
        /// <summary>
        /// Executes the specified delegate with the specified arguments on the thread the <see cref="System.Windows.Threading.Dispatcher"/> was created on.
        /// </summary>
        /// <param name="method">An <see cref="Action"/> which is pushed onto the <see cref="System.Windows.Threading.Dispatcher"/> event queue.</param>
        public void BeginInvoke(Action method)
        {
            if (method == null)
                throw new ArgumentNullException("method");

            method();
        }

        /// <summary>
        /// Determines whether the calling thread is the thread associated with this <see cref="System.Windows.Threading.Dispatcher"/>.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the calling thread is the thread associated with this <see cref="System.Windows.Threading.Dispatcher"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool CheckAccess()
        {
            return true;
        }
    }
}
