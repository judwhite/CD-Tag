using System.Windows.Input;
using CDTag.Common;

namespace CDTag.ViewModel.Tag
{
    public interface ITagToolbarViewModel : IViewModelBase
    {
        ICommand BackCommand { get; }
        ICommand ForwardCommand { get; }
        ICommand UpCommand { get; }
        ICommand HelpCommand { get; }
    }
}
