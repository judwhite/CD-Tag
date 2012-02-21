using CDTag.Common;

namespace CDTag.ViewModels.Tag.MassTag
{
    public class MassTagViewModel : ViewModelBase, IMassTagViewModel
    {
        public MassTagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
