﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace CDTag.Common
{
    /// <summary>ModelBase</summary>
    /// <typeparam name="T">The view model type.</typeparam>
    public abstract class ModelBase<T> : ModelBase
    {
        /// <summary>Occurs when a property value changes.</summary>
        public event EnhancedPropertyChangedEventHandler<T> EnhancedPropertyChanged;

        /// <summary>Sends the <see cref="ModelBase.PropertyChanged"/> and <see cref="EnhancedPropertyChanged" /> events.</summary>
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
    /// ModelBase
    /// </summary>
    public abstract class ModelBase : IModelBase
    {
        private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();

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
    }
}
