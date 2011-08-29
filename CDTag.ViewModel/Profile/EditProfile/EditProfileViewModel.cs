using CDTag.Common;

namespace CDTag.ViewModel.Profile.EditProfile
{
    public class EditProfileViewModel : ViewModelBase, IEditProfileViewModel
    {
        public EditProfileViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
