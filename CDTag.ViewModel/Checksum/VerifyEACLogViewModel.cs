using CDTag.Common;

namespace CDTag.ViewModels.Checksum
{
    public class VerifyEACLogViewModel : ViewModelBase, IVerifyEACLogViewModel
    {
        public VerifyEACLogViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
