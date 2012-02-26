using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Mvvm;

namespace CDTag.ViewModels.Tools
{
    public class SplitCueViewModel : ViewModelBase, ISplitCueViewModel
    {
        public SplitCueViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
