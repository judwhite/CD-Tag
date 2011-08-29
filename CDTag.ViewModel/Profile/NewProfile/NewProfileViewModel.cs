using CDTag.Common;

namespace CDTag.ViewModel.Profile.NewProfile
{
    public class NewProfileViewModel : ViewModelBase, INewProfileViewModel
    {
        public NewProfileViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
