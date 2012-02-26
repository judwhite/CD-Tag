using System;
using System.Collections.Generic;
using System.ComponentModel;
using CDTag.Common.Events;

namespace CDTag.Common.Model
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
        protected override void RaisePropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.RaisePropertyChanged(propertyName, oldValue, newValue);

            var handler = EnhancedPropertyChanged;
            if (handler != null)
                handler(this, new EnhancedPropertyChangedEventArgs<T>(propertyName, oldValue, newValue));
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

        /// <summary>Occurs when a property or sub-property has changed on a property which inherits from <see cref="ModelBase" />.</summary>
        public event EventHandler SubPropertyChanged;

        /// <summary>Sends the <see cref="PropertyChanged"/> event.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void RaisePropertyChanged(string propertyName, object oldValue, object newValue)
        {
            ModelBase oldModel = oldValue as ModelBase;
            ModelBase newModel = newValue as ModelBase;
            if (oldModel != null)
            {
                oldModel.PropertyChanged -= ModelProperty_PropertyChanged;
                oldModel.SubPropertyChanged -= ModelProperty_SubPropertyChanged;
            }

            if (newModel != null)
            {
                newModel.PropertyChanged += ModelProperty_PropertyChanged;
                newModel.SubPropertyChanged += ModelProperty_SubPropertyChanged;
            }

            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ModelProperty_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = SubPropertyChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void ModelProperty_SubPropertyChanged(object sender, EventArgs e)
        {
            var handler = SubPropertyChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
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
    }
}
