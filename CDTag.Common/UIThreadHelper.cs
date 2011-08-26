using System;
using System.Windows;

namespace CDTag.Common
{
    /// <summary>
    /// UI Thread Helper
    /// </summary>
    public static class UIThreadHelper
    {
        /// <summary>
        /// Invokes the specified action on the UI thread.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void Invoke(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (Application.Current.Dispatcher.CheckAccess())
                action();
            else
                Application.Current.Dispatcher.BeginInvoke(action);
        }

        /// <summary>
        /// Invokes the specified action on the UI thread if not currently on the UI thread.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns><c>true</c> if the action was invoke; otherwise, <c>false</c>.</returns>
        public static bool InvokeIfRequired(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.BeginInvoke(action);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
