using CDTag.Common;

namespace CDTag.ViewModels.Checksum
{
    public class ChecksumViewModel : ViewModelBase, IChecksumViewModel
    {
        public ChecksumViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
