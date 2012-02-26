using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Mvvm;

namespace CDTag.ViewModel.Options
{
    public class OptionsViewModel : ViewModelBase, IOptionsViewModel
    {
        public OptionsViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
