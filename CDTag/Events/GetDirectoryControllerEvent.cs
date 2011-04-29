using System;
using CDTag.FileBrowser.ViewModel;
using Microsoft.Practices.Prism.Events;

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
