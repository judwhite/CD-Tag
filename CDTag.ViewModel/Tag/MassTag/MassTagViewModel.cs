using CDTag.Common;

namespace CDTag.ViewModel.Tag.MassTag
{
    public class MassTagViewModel : ViewModelBase, IMassTagViewModel
    {
        public MassTagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
