using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Mvvm;

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
