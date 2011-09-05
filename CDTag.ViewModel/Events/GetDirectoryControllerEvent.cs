using CDTag.FileBrowser.ViewModel;

namespace CDTag.ViewModel.Events
{
    public class GetDirectoryControllerEvent
    {
        public IDirectoryController DirectoryController { get; set; }
    }
}
