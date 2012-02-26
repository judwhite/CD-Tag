using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Events;
using CDTag.Common.Mvvm;
using CDTag.Model.Profile;

namespace CDTag.ViewModels.Profile.EditProfile
{
    public class EditProfileViewModel : ViewModelBase<EditProfileViewModel>, IEditProfileViewModel
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

            EnhancedPropertyChanged += EditProfileViewModel_EnhancedPropertyChanged;

            Profile = _profiles.FirstOrDefault();
            Header = "Select Profile";
            UpdateWindowTitle();
        }

        private void EditProfileViewModel_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<EditProfileViewModel> e)
        {
            if (e.IsProperty(p => p.Profile))
            {
                UserProfile oldValue = e.OldValue as UserProfile;
                if (oldValue != null)
                {
                    oldValue.EnhancedPropertyChanged -= Profile_EnhancedPropertyChanged;
                }

                UserProfile newValue = e.NewValue as UserProfile;
                if (newValue != null)
                {
                    newValue.EnhancedPropertyChanged -= Profile_EnhancedPropertyChanged;
                    newValue.EnhancedPropertyChanged += Profile_EnhancedPropertyChanged;
                    newValue.CheckHasChanges();
                }

                UpdateWindowTitle();
            }
        }

        private void Profile_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<UserProfile> e)
        {
            if (e.IsProperty(p => p.HasChanges))
            {
                UpdateWindowTitle();
            }
        }

        private void UpdateWindowTitle()
        {
            var profile = Profile;
            if (profile == null)
            {
                WindowTitle = "Edit Profile";
            }
            else
            {
                WindowTitle = string.Format("Edit Profile - {0}{1}", profile.ProfileName, profile.HasChanges ? "*" : "");
            }
        }

        public string WindowTitle
        {
            get { return Get<string>("WindowTitle"); }
            set { Set("WindowTitle", value); }
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
            set { Set("Profile", value); }
        }
    }
}
