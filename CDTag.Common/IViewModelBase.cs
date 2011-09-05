using System;
using System.ComponentModel;
using CDTag.View.Interfaces;

namespace CDTag.Common
{
    /// <summary>
    /// IViewModelBase
    /// </summary>
    public interface IViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        IWindow View { get; set; }

        /// <summary>Gets or sets the error container.</summary>
        /// <value>The error container.</value>
        IErrorContainer ErrorContainer { get; set; }

        /// <summary>
        /// Gets or sets the close window action.
        /// </summary>
        /// <value>The close window action.</value>
        Action CloseWindow { get; set; }
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
