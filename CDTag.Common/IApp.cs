using System;

namespace CDTag.Common
{
    /// <summary>
    /// IApp
    /// </summary>
    public interface IApp
    {
        /// <summary>Shows the error on the main window.</summary>
        /// <param name="exception">The exception.</param>
        void ShowError(Exception exception);

        /// <summary>Shows the error.</summary>
        /// <param name="exception">The exception.</param>
        /// <param name="errorContainer">The error container.</param>
        void ShowError(Exception exception, IErrorContainer errorContainer);
    }
}
