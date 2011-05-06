using CDTag.Common;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Checksum
{
    public class ChecksumViewModel : ViewModelBase, IChecksumViewModel
    {
        public ChecksumViewModel(EventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
