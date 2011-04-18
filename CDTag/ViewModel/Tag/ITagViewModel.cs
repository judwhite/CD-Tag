using CDTag.Common;
using CDTag.FileBrowser;

namespace CDTag.ViewModel.Tag
{
    public interface ITagViewModel : IViewModelBase
    {
        DirectoryController DirectoryController { get; set; }
    }
}
