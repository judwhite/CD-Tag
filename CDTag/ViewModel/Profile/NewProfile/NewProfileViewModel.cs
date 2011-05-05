using CDTag.Common;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Profile.NewProfile
{
    public class NewProfileViewModel : ViewModelBase, INewProfileViewModel
    {
        public NewProfileViewModel(EventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
