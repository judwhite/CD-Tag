using CDTag.Common;

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
