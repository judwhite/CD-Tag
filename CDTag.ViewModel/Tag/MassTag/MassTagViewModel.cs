using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Mvvm;

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
