using CDTag.Common;

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
