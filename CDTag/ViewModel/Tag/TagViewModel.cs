using CDTag.Common;
using CDTag.Events;
using CDTag.FileBrowser;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tag
{
    public class TagViewModel : ViewModelBase, ITagViewModel
    {
        private DirectoryController _directoryController;

        public TagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            eventAggregator.GetEvent<GoBackEvent>().Subscribe((o) => _directoryController.GoBack());
            eventAggregator.GetEvent<GoForwardEvent>().Subscribe((o) => _directoryController.GoForward());
            eventAggregator.GetEvent<GoUpEvent>().Subscribe((o) => _directoryController.GoUp());
        }

        public DirectoryController DirectoryController
        {
            get { return _directoryController; }
            set 
            {
                if (_directoryController != value)
                {
                    _directoryController = value;
                    SendPropertyChanged("DirectoryController");
                }
            }
        }
    }
}
