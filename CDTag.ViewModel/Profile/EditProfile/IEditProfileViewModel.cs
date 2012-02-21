using System.Collections.ObjectModel;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Model.Profile;

namespace CDTag.ViewModels.Profile.EditProfile
{
    public interface IEditProfileViewModel : IViewModelBase
    {
        string Header { get; }
        ICommand NewProfileCommand { get; }
        ICommand RenameProfileCommand { get; }
        ICommand CopyProfileCommand { get; }
        ICommand DeleteProfileCommand { get; }
        ObservableCollection<UserProfile> Profiles { get; }
        UserProfile Profile { get; set; }
    }
}
