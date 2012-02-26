using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Mvvm;

namespace CDTag.ViewModels.Tools
{
    public class EncodingInspectorViewModel : ViewModelBase, IEncodingInspectorViewModel
    {
        public EncodingInspectorViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
