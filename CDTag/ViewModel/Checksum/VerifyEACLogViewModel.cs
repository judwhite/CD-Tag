using CDTag.Common;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Checksum
{
    public class VerifyEACLogViewModel : ViewModelBase, IVerifyEACLogViewModel
    {
        public VerifyEACLogViewModel(EventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
