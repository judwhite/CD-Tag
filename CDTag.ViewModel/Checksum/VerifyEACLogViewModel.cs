using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Mvvm;

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
