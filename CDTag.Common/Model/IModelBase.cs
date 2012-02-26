using System.ComponentModel;
using CDTag.Common.Events;
using CDTag.Common.Mvvm;

namespace CDTag.Common.Model
{
    /// <summary>
    /// IViewModelBase
    /// </summary>
    public interface IModelBase : INotifyPropertyChanged
    {
    }

    /// <summary>
    /// IViewModelBase
    /// </summary>
    /// <typeparam name="T">The view model type.</typeparam>
    public interface IModelBase<T> : IViewModelBase
    {
        /// <summary>Occurs when a property value changes.</summary>
        event EnhancedPropertyChangedEventHandler<T> EnhancedPropertyChanged;
    }
}
