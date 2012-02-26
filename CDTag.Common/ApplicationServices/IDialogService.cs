using System;
using CDTag.Common.Wpf;
using CDTag.View.Interfaces;

namespace CDTag.Common.ApplicationServices
{
    /// <summary>
    /// IDialogService
    /// </summary>
    public interface IDialogService
    {
        /// <summary>Shows the error on the main window.</summary>
        /// <param name="exception">The exception.</param>
        void ShowError(Exception exception);

        /// <summary>Shows the error.</summary>
        /// <param name="exception">The exception.</param>
        /// <param name="errorContainer">The error container.</param>
        void ShowError(Exception exception, IErrorContainer errorContainer);

        /// <summary>Shows a window.</summary>
        /// <typeparam name="T">The window type.</typeparam>
        /// <returns>The result of <see cref="IWindow.ShowDialog()" />.</returns>
        bool? ShowWindow<T>()
            where T : IWindow;

        /// <summary>Closes the address bar text box.</summary>
        void CloseAddressTextBox();

        /// <summary>Shows the open file dialog.</summary>
        /// <param name="title">The dialog title.</param>
        /// <param name="filter">The file filter.</param>
        /// <param name="fileName">Name of the file opened.</param>
        /// <returns><c>true</c> if a file is selected.</returns>
        bool? ShowOpenFileDialog(string title, string filter, out string fileName);
    }
}
