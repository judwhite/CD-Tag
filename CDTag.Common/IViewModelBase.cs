﻿using System;
using System.ComponentModel;
using System.Windows;
using CDTag.ViewModel.Events;
using IdSharp.Common.Events;

namespace CDTag.Common
{
    /// <summary>
    /// IViewModelBase
    /// </summary>
    public interface IViewModelBase : INotifyPropertyChanged
    {
        /// <summary>Occurs when <see cref="ShowMessageBox" /> has been called.</summary>
        event EventHandler<DataEventArgs<MessageBoxEvent>> ShowMessageBox;

        /// <summary>Gets or sets the error container.</summary>
        /// <value>The error container.</value>
        IErrorContainer ErrorContainer { get; set; }

        /// <summary>Gets or sets the close window action.</summary>
        /// <value>The close window action.</value>
        Action CloseWindow { get; set; }

        /// <summary>Gets or sets the current visual state.</summary>
        /// <value>The current visual state.</value>
        string CurrentVisualState { get; set; }

        /// <summary>Gets the event aggregator.</summary>
        IEventAggregator EventAggregator { get; }
    }

    /// <summary>
    /// IViewModelBase
    /// </summary>
    /// <typeparam name="T">The view model type.</typeparam>
    public interface IViewModelBase<T> : IViewModelBase
    {
        /// <summary>Occurs when a property value changes.</summary>
        event EnhancedPropertyChangedEventHandler<T> EnhancedPropertyChanged;
    }
}
