﻿using System;
using System.Windows;
using CDTag.Common;
using CDTag.View.Interfaces;

namespace CDTag.View
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

        /// <summary>Gets the local application directory.</summary>
        string LocalApplicationDirectory { get; }

        /// <summary>Shows the window.</summary>
        /// <typeparam name="T">The window type.</typeparam>
        /// <returns>The result of <see cref="Window.ShowDialog()" />.</returns>
        bool? ShowWindow<T>()
            where T : IWindow;

        /*/// <summary>Gets the main window.</summary>
        Window MainWindow { get; }*/

        /// <summary>Closes the address bar text box.</summary>
        void CloseAddressTextBox();
    }
}