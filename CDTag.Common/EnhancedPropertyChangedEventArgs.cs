﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace CDTag.Common
{
    /// <summary>EnhancedPropertyChangedEventArgs</summary>
    /// <typeparam name="T">The view model type.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="CDTag.Common.EnhancedPropertyChangedEventArgs&lt;T&gt;"/> instance containing the event data.</param>
    public delegate void EnhancedPropertyChangedEventHandler<T>(object sender, EnhancedPropertyChangedEventArgs<T> e);

    /// <summary>EnhancedPropertyChangedEventArgs</summary>
    /// <typeparam name="T">The view model type.</typeparam>
    public class EnhancedPropertyChangedEventArgs<T> : PropertyChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedPropertyChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public EnhancedPropertyChangedEventArgs(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>Gets the old value.</summary>
        public object OldValue { get; private set; }
        
        /// <summary>Gets the new value.</summary>
        public object NewValue { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public delegate object PropertyDelegate(T viewModel);

        /// <summary>Determines whether the specified expression is the property which changed.</summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>
        ///   <c>true</c> if the specified expression is the property which changed; otherwise, <c>false</c>.
        /// </returns>
        public bool IsProperty(Expression<PropertyDelegate> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");

            MemberInfo property = ((MemberExpression)propertyExpression.Body).Member;
            return (PropertyName == property.Name);
        }
    }
}
