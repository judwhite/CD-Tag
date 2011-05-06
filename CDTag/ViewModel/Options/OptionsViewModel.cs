using CDTag.Common;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Options
{
    public class OptionsViewModel : ViewModelBase, IOptionsViewModel
    {
        public OptionsViewModel(EventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
