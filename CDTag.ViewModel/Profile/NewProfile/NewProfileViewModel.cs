using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using CDTag.View;
using CDTag.ViewModel.Events;

namespace CDTag.ViewModel.Profile.NewProfile
{
    public class NewProfileViewModel : ViewModelBase<NewProfileViewModel>, INewProfileViewModel
    {
        private readonly DelegateCommand _nextCommand;
        private readonly DelegateCommand _previousCommand;

        private bool _storedCreateSampleNFO = true;
        private bool _storedHasExistingNFO;

        private bool _confirmedOverwrite;

        public NewProfileViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _nextCommand = new DelegateCommand(Next, () => PageIndex < 1);
            _previousCommand = new DelegateCommand(Previous, () => PageIndex > 0);

            EnhancedPropertyChanged += NewProfileViewModel_EnhancedPropertyChanged;
        }

        private void NewProfileViewModel_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<NewProfileViewModel> e)
        {
            if (e.IsProperty(p => p.CreateNFO))
            {
                if (!CreateNFO)
                {
                    _storedCreateSampleNFO = CreateSampleNFO;
                    _storedHasExistingNFO = HasExistingNFO;

                    CreateSampleNFO = false;
                    HasExistingNFO = false;
                }
                else
                {
                    CreateSampleNFO = _storedCreateSampleNFO;
                    HasExistingNFO = _storedHasExistingNFO;
                }
            }
            else if (e.IsProperty(p => p.ProfileName))
            {
                _confirmedOverwrite = false;
            }
        }

        private void Next()
        {
            if (PageIndex == 0)
            {
                if (!ValidateProfileName())
                {
                    IsProfileNameFocused = true;
                    return;
                }
            }

            PageIndex += 1;
        }

        private bool ValidateProfileName()
        {
            // No profile name entered
            if (string.IsNullOrWhiteSpace(ProfileName))
            {
                // TODO: Localize
                MessageBoxEvent messageBoxEvent = new MessageBoxEvent
                {
                    Owner = View,
                    MessageBoxText = "Please enter a name for your profile.",
                    Caption = "New profile",
                    MessageBoxButton = MessageBoxButton.OK,
                    MessageBoxImage = MessageBoxImage.Information
                };

                _eventAggregator.Publish(messageBoxEvent);
                return false;
            }

            // Check if file exists
            string fileName = GetProfileFileName();
            if (File.Exists(fileName) && !_confirmedOverwrite)
            {
                // TODO: Localize
                MessageBoxEvent messageBoxEvent = new MessageBoxEvent
                {
                    Owner = View,
                    MessageBoxText = string.Format("'{0}' exists. Overwrite?", fileName),
                    Caption = "New profile",
                    MessageBoxButton = MessageBoxButton.YesNo,
                    MessageBoxImage = MessageBoxImage.Question
                };

                _eventAggregator.Publish(messageBoxEvent);
                if (messageBoxEvent.Result != MessageBoxResult.Yes)
                    return false;

                _confirmedOverwrite = true;
            }

            // Check if able to create file
            if (!File.Exists(fileName))
            {
                try
                {
                    File.WriteAllBytes(fileName, new byte[0]);
                }
                catch
                {
                    // TODO: Localize
                    MessageBoxEvent messageBoxEvent = new MessageBoxEvent
                    {
                        Owner = View,
                        MessageBoxText = string.Format("'{0}' cannot be created.", fileName),
                        Caption = "New profile",
                        MessageBoxButton = MessageBoxButton.OK,
                        MessageBoxImage = MessageBoxImage.Information
                    };

                    _eventAggregator.Publish(messageBoxEvent);
                    return false;
                }

                File.Delete(fileName);
            }

            return true;
        }

        private string GetProfileFileName()
        {
            string fileName = Path.Combine(IoC.Resolve<IDialogService>().ProfileDirectory, ProfileName);

            string ext = Path.GetExtension(fileName);
            if (string.Compare(ext, ".cfg", ignoreCase: true) != 0)
                fileName += ".cfg";

            return fileName;
        }

        private void Previous()
        {
            if (PageIndex > 0)
                PageIndex -= 1;

            if (PageIndex == 0)
                IsProfileNameFocused = true;
        }

        public string ProfileName
        {
            get { return Get<string>("ProfileName"); }
            set { Set("ProfileName", value); }
        }

        public bool CreateNFO
        {
            get { return Get<bool>("CreateNFO"); }
            set { Set("CreateNFO", value); }
        }

        public bool CreateSampleNFO
        {
            get { return Get<bool>("CreateSampleNFO"); }
            set { Set("CreateSampleNFO", value); }
        }

        public bool HasExistingNFO
        {
            get { return Get<bool>("HasExistingNFO"); }
            set { Set("HasExistingNFO", value); }
        }

        public ICommand PreviousCommand
        {
            get { return _previousCommand; }
        }

        public ICommand NextCommand
        {
            get { return _nextCommand; }
        }

        public int PageIndex
        {
            get { return Get<int>("PageIndex"); }
            private set { Set("PageIndex", value); }
        }

        public bool UseUnderscores
        {
            get { return Get<bool>("UseUnderscores"); }
            set { Set("UseUnderscores", value); }
        }

        public bool UseStandardCharactersOnly
        {
            get { return Get<bool>("UseStandardCharactersOnly"); }
            set { Set("UseStandardCharactersOnly", value); }
        }

        public bool UseLatinCharactersOnly
        {
            get { return Get<bool>("UseLatinCharactersOnly"); }
            set { Set("UseLatinCharactersOnly", value); }
        }

        public bool IsProfileNameFocused
        {
            get { return Get<bool>("IsProfileNameFocused"); }
            set
            {
                if (value) // force change for Binding (forcing PropertyChanged isn't sufficient)
                    Set("IsProfileNameFocused", false);
                Set("IsProfileNameFocused", value);
            }
        }
    }
}
