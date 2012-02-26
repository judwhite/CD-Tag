﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Dispatcher;
using CDTag.Common.Events;
using CDTag.Common.Wpf;
using IdSharp.Common.Events;

namespace CDTag.Common.Mvvm
{
    /// <summary>ViewModelBase</summary>
    /// <typeparam name="T">The view model type.</typeparam>
    public abstract class ViewModelBase<T> : ViewModelBase
    {
        /// <summary>Occurs when a property value changes.</summary>
        public event EnhancedPropertyChangedEventHandler<T> EnhancedPropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        protected ViewModelBase(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        /// <summary>Raises the <see cref="ViewModelBase.PropertyChanged"/> and <see cref="EnhancedPropertyChanged" /> events.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void RaisePropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.RaisePropertyChanged(propertyName, oldValue, newValue);

            var handler = EnhancedPropertyChanged;
            if (handler != null)
                handler(this, new EnhancedPropertyChangedEventArgs<T>(propertyName, oldValue, newValue));
        }
    }

    /// <summary>
    /// ViewModelBase
    /// </summary>
    public abstract class ViewModelBase : IViewModelBase
    {
        /// <summary>Dispatcher service.</summary>
        protected static readonly IDispatcher _dispatcher;

        /// <summary>Path service.</summary>
        protected static readonly IPathService _pathService;

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Occurs when <see cref="ShowMessageBox"/> has been called.</summary>
        public event EventHandler<DataEventArgs<MessageBoxEvent>> ShowMessageBox;

        private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();

        /// <summary>The event aggregator.</summary>
        protected readonly IEventAggregator _eventAggregator;

        static ViewModelBase()
        {
            _dispatcher = IoC.Resolve<IDispatcher>();
            _pathService = IoC.Resolve<IPathService>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        protected ViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>Gets the event aggregator.</summary>
        public IEventAggregator EventAggregator { get { return _eventAggregator; } }

        /// <summary>
        /// Gets or sets the close window action.
        /// </summary>
        /// <value>The close window action.</value>
        public Action CloseWindow { get; set; }

        /// <summary>Gets or sets the error container.</summary>
        /// <value>The error container.</value>
        public IErrorContainer ErrorContainer
        {
            get { return Get<IErrorContainer>("ErrorContainer"); }
            set { Set("ErrorContainer", value); }
        }

        /// <summary>Shows the exception.</summary>
        /// <param name="exception">The exception.</param>
        protected void ShowException(Exception exception)
        {
            IErrorContainer errorContainer = ErrorContainer;
            if (errorContainer == null)
                IoC.Resolve<IDialogService>().ShowError(exception);
            else
                IoC.Resolve<IDialogService>().ShowError(exception, errorContainer);
        }

        /// <summary>Raises the <see cref="PropertyChanged"/> event.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void RaisePropertyChanged(string propertyName, object oldValue, object newValue)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>Gets the specified property value.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The property value.</returns>
        protected T Get<T>(string propertyName)
        {
            object value;
            if (_propertyValues.TryGetValue(propertyName, out value))
                return (T)value;
            else
                return default(T);
        }

        /// <summary>Sets the specified property value.</summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        protected void Set<T>(string propertyName, T value)
        {
            bool keyExists;
            T oldValue;
            object oldValueObject;
            if (_propertyValues.TryGetValue(propertyName, out oldValueObject))
            {
                keyExists = true;
                oldValue = (T)oldValueObject;
            }
            else
            {
                keyExists = false;
                oldValue = default(T);
            }

            bool hasChanged = false;
            if (value != null)
            {
                if (!value.Equals(oldValue))
                    hasChanged = true;
            }
            else if (oldValue != null)
            {
                hasChanged = true;
            }

            if (hasChanged)
            {
                if (keyExists)
                    _propertyValues[propertyName] = value;
                else
                    _propertyValues.Add(propertyName, value);

                RaisePropertyChanged(propertyName, oldValue, value);
            }
        }

        /// <summary>Gets the specified property value.</summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="method">The property method.</param>
        /// <returns>The property value.</returns>
        protected T Get<T>(MethodBase method)
        {
            return Get<T>(method.Name.Substring(4));
        }

        /// <summary>Sets the specified property value.</summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="method">The property method.</param>
        /// <param name="value">The value.</param>
        protected void Set<T>(MethodBase method, T value)
        {
            Set(method.Name.Substring(4), value);
        }

        /// <summary>Gets or sets the current visual state.</summary>
        /// <value>The current visual state.</value>
        public string CurrentVisualState
        {
            get { return Get<string>("CurrentVisualState"); }
            set
            {
                BeginInvoke(() => Set("CurrentVisualState", value));
            }
        }

        /// <summary>Invokes the specified action on the UI thread.</summary>
        /// <param name="action">The action to invoke.</param>
        protected void BeginInvoke(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }

        /// <summary>Determines whether the calling thread is the thread associated with this <see cref="System.Windows.Threading.Dispatcher" />.</summary>
        /// <returns><c>true</c> if the calling thread is the thread associated with this <see cref="System.Windows.Threading.Dispatcher" />; otherwise, <c>false</c>.</returns>
        protected bool CheckAccess()
        {
            return _dispatcher.CheckAccess();
        }

        /// <summary>Messages the box.</summary>
        /// <param name="messageBoxEvent">The message box event.</param>
        /// <returns>The message box result.</returns>
        protected MessageBoxResult MessageBox(MessageBoxEvent messageBoxEvent)
        {
            if (messageBoxEvent == null)
                throw new ArgumentException("messageBoxEvent");

            var handler = ShowMessageBox;
            if (handler != null)
                handler(this, new DataEventArgs<MessageBoxEvent>(messageBoxEvent));
            else
                throw new Exception("'ShowMessageBox' event is not subscribed to.");

            return messageBoxEvent.Result;
        }

        /// <summary>Shows a message box.</summary>
        /// <param name="messageBoxText">The message box text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="image">The image.</param>
        /// <returns>The message box result.</returns>
        protected MessageBoxResult MessageBox(string messageBoxText, string caption, MessageBoxButton buttons, MessageBoxImage image)
        {
            MessageBoxEvent messageBox = new MessageBoxEvent
            {
                MessageBoxText = messageBoxText,
                Caption = caption,
                MessageBoxButton = buttons,
                MessageBoxImage = image
            };

            return MessageBox(messageBox);
        }
    }
}
