using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Events;
using CDTag.Common.Mvvm;
using CDTag.Model.Profile;
using CDTag.Views.Interfaces.Profile.NewProfile;

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

            string[] files = Directory.GetFiles(_pathService.ProfileDirectory, "*.cfg").OrderBy(p => p.ToLower()).ToArray();
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

            if (_profiles.Count == 0)
            {
                CreateDefaultProfile();
            }

            EnhancedPropertyChanged += EditProfileViewModel_EnhancedPropertyChanged;

            Profile = _profiles.First();
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
            var currentProfile = Profile;
            if (currentProfile != null && currentProfile.HasChanges)
            {
                var msgBoxResult = MessageBox(string.Format("Do you want to save changes to {0}?", currentProfile.ProfileName), "Save changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (msgBoxResult == MessageBoxResult.Yes)
                {
                    currentProfile.Save();
                }
                else if (msgBoxResult != MessageBoxResult.No)
                {
                    return;
                }
            }

            bool? result = ShowWindow<INewProfileWindow>();

            if (result == true)
            {
                // TODO: What if the user overwrites an existing profile?

                UserProfile newProfile = null;

                string[] files = Directory.GetFiles(_pathService.ProfileDirectory, "*.cfg").OrderBy(p => p.ToLower()).ToArray();
                foreach (string file in files)
                {
                    bool exists = false;
                    foreach (var profile in Profiles)
                    {
                        if (string.Compare(profile.GetProfileFileName(), file, ignoreCase: true) == 0)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (exists)
                    {
                        continue;
                    }

                    try
                    {
                        var profile = UserProfile.Load(file);
                        _profiles.Add(profile);

                        if (newProfile == null)
                            newProfile = profile;
                    }
                    catch (Exception ex)
                    {
                        // TODO
                        Debug.WriteLine(ex);
                    }
                }

                if (newProfile != null)
                {
                    Profile = newProfile;
                }
            }
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
            var profile = Profile;
            if (profile == null)
                return;

            string messageBoxText = string.Format("Do you want to delete {0}?", profile.ProfileName);
            if (_profiles.Count == 1)
            {
                messageBoxText += string.Format("{0}{0}If you delete this profile a Default profile will be created.", Environment.NewLine);
            }

            var result = MessageBox(messageBoxText, "Delete profile", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            File.Delete(profile.GetProfileFileName());

            Profile = null;

            int index = _profiles.IndexOf(profile);
            _profiles.RemoveAt(index);

            if (_profiles.Count > 0)
            {
                if (index >= _profiles.Count)
                    index = _profiles.Count - 1;

                Profile = _profiles[index];
            }
            else
            {
                Profile = CreateDefaultProfile();
            }
        }

        private UserProfile CreateDefaultProfile()
        {
            UserProfile profile = new UserProfile();
            profile.ProfileName = "Default";
            profile.Save();

            _profiles.Add(profile);

            return profile;
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
