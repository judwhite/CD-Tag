using CDTag.Common;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Profile.EditProfile
{
    public class EditProfileViewModel : ViewModelBase, IEditProfileViewModel
    {
        public EditProfileViewModel(EventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
