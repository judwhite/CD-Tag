using CDTag.FileBrowser.ViewModel;

namespace CDTag.ViewModels.Events
{
    public class GetDirectoryControllerEvent
    {
        public IDirectoryController DirectoryController { get; set; }
    }
}
