using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Model.Profile;

namespace CDTag.ViewModels.Profile.EditProfile
{
    public class EditProfileViewModel : ViewModelBase, IEditProfileViewModel
    {
        private readonly DelegateCommand _newProfileCommand;
        private readonly DelegateCommand _renameProfileCommand;
        private readonly DelegateCommand _copyProfileCommand;
        private readonly DelegateCommand _deleteProfileCommand;
        
        private readonly ObservableCollection<UserProfile> _profiles;

        public EditProfileViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _newProfileCommand = new DelegateCommand(NewProfile);
            _renameProfileCommand = new DelegateCommand(RenameProfile);
            _copyProfileCommand = new DelegateCommand(CopyProfile);
            _deleteProfileCommand = new DelegateCommand(DeleteProfile);

            _profiles = new ObservableCollection<UserProfile>();

            string[] files = Directory.GetFiles(_pathService.ProfileDirectory, "*.cfg");
            foreach (string file in files)
            {
                try
                {
                    _profiles.Add(UserProfile.Load(file));
                }
                catch (Exception ex)
                {
                    // TODO
                    Debug.WriteLine(ex);
                }
            }

            Header = "Select Profile";
        }

        public string Header
        {
            get { return Get<string>("Header"); }
            set { Set("Header", value); }
        }

        public ICommand NewProfileCommand
        {
            get { return _newProfileCommand; }
        }

        public ICommand RenameProfileCommand
        {
            get { return _renameProfileCommand; }
        }

        public ICommand CopyProfileCommand
        {
            get { return _copyProfileCommand; }
        }

        public ICommand DeleteProfileCommand
        {
            get { return _deleteProfileCommand; }
        }

        private void NewProfile()
        {
            throw new NotImplementedException();
        }

        private void RenameProfile()
        {
            throw new NotImplementedException();
        }

        private void CopyProfile()
        {
            throw new NotImplementedException();
        }

        private void DeleteProfile()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<UserProfile> Profiles
        {
            get { return _profiles; }
        }

        public UserProfile Profile
        {
            get { return Get<UserProfile>("Profile"); }
            set { Set("UserProfile", value); }
        }
    }
}
