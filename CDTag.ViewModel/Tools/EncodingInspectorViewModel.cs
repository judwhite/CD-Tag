using CDTag.Common;

namespace CDTag.ViewModel.Tools
{
    public class EncodingInspectorViewModel : ViewModelBase, IEncodingInspectorViewModel
    {
        public EncodingInspectorViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
