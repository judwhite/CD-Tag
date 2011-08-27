using CDTag.Common;

namespace CDTag.ViewModel.Checksum
{
    public class ChecksumViewModel : ViewModelBase, IChecksumViewModel
    {
        public ChecksumViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
