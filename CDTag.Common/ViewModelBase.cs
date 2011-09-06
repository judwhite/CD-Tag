using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using CDTag.View;
using CDTag.View.Interfaces;

namespace CDTag.Common
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

        /// <summary>Sends the <see cref="ViewModelBase.PropertyChanged"/> and <see cref="EnhancedPropertyChanged" /> events.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void SendPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.SendPropertyChanged(propertyName, oldValue, newValue);

            var handler2 = EnhancedPropertyChanged;
            if (handler2 != null)
                handler2(this, new EnhancedPropertyChangedEventArgs<T>(propertyName, oldValue, newValue));
        }
    }

    /// <summary>
    /// ViewModelBase
    /// </summary>
    public abstract class ViewModelBase : IViewModelBase
    {
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();

        /// <summary>The event aggregator.</summary>
        protected readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        protected ViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        public IWindow View
        {
            get { return Get<IWindow>("View"); }
            set { Set("View", value); }
        }

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

        /// <summary>Sends the <see cref="PropertyChanged"/> event.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void SendPropertyChanged(string propertyName, object oldValue, object newValue)
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

                SendPropertyChanged(propertyName, oldValue, value);
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
    }
}
