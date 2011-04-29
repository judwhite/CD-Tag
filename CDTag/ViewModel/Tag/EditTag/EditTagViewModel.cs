using CDTag.Common;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tag.EditTag
{
    public class EditTagViewModel : ViewModelBase, IEditTagViewModel
    {
        public EditTagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
