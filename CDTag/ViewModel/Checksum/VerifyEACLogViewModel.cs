using CDTag.Common;

namespace CDTag.ViewModel.Checksum
{
    public class VerifyEACLogViewModel : ViewModelBase, IVerifyEACLogViewModel
    {
        public VerifyEACLogViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
