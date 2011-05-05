using System.ComponentModel;

namespace CDTag.Common
{
    /// <summary>
    /// IViewModelBase
    /// </summary>
    public interface IViewModelBase : INotifyPropertyChanged
    {
        /// <summary>Gets or sets the error container.</summary>
        /// <value>The error container.</value>
        IErrorContainer ErrorContainer { get; set; }
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
