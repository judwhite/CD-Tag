using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;

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
        private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();
        private readonly CommandBindingCollection _commandBindings;

        /// <summary>The event aggregator.</summary>
        protected readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        protected ViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _commandBindings = new CommandBindingCollection();
        }

        /// <summary>Gets the command bindings.</summary>
        public CommandBindingCollection CommandBindings
        {
            get { return _commandBindings; }
        }

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// <typeparam name="T"></typeparam>
        /// <returns>The property value.</returns>
        protected T Get<T>()
        {
            StackFrame stackFrame = new StackFrame(skipFrames: 1);
            string propertyName = stackFrame.GetMethod().Name.Substring(startIndex: 4);
            return Get<T>(propertyName);
        }

        /// <summary>Sets the specified property value.</summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="value">The value.</param>
        protected void Set<T>(T value)
        {
            StackFrame stackFrame = new StackFrame(skipFrames: 1);
            string propertyName = stackFrame.GetMethod().Name.Substring(startIndex: 4);
            Set(propertyName, value);
        }

        /// <summary>Registers a key combination to the specified <paramref name="command"/>.</summary>
        /// <param name="modifierKeys">The modifier keys.</param>
        /// <param name="key">The key.</param>
        /// <param name="command">The command.</param>
        protected void RegisterCommandBinding(ModifierKeys modifierKeys, Key key, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            RoutedUICommand routedUICommand = new RoutedUICommand();
            string displayString = string.Format("{0}+{1}", modifierKeys, key);
            var keyGesture = new KeyGesture(key, modifierKeys, displayString);
            routedUICommand.InputGestures.Add(keyGesture);

            CommandBinding commandBinding = new CommandBinding(routedUICommand, (s, e) => command.Execute(null), (s, e) => { e.CanExecute = command.CanExecute(null); });
            CommandManager.RegisterClassCommandBinding(GetType(), commandBinding);

            CommandBindings.Add(commandBinding);
        }
    }
}
