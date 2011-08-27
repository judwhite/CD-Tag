using System;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;

namespace CDTag.Events
{
    public class GetDirectoryControllerEvent : CompositePresentationEvent<GetDirectoryControllerEventArgs>
    {
    }

    public class GetDirectoryControllerEventArgs : EventArgs
    {
        public IDirectoryController DirectoryController { get; set; }
    }
}
