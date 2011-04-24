using System.Collections.ObjectModel;
using System.Windows.Input;
using CDTag.Common;

namespace CDTag.ViewModel.About
{
    public interface IAboutViewModel : IViewModelBase
    {
        ICommand NavigateCommand { get; }
        string ReleaseNotes { get; }
        ObservableCollection<string> ComponentsCollection { get; }
        ICommand CopyComponentCommand { get; }
        ICommand CloseCommand { get; }
    }
}
