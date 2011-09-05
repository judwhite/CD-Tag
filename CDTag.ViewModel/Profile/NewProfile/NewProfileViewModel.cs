using System;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using CDTag.View.Interfaces;
using CDTag.ViewModel.Events;

namespace CDTag.ViewModel.Profile.NewProfile
{
    public class NewProfileViewModel : ViewModelBase<NewProfileViewModel>, INewProfileViewModel
    {
        private readonly DelegateCommand _nextCommand;
        private readonly DelegateCommand _previousCommand;

        private bool _storedCreateSampleNFO = true;
        private bool _storedHasExistingNFO;

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
        }

        private void Next()
        {
            if (PageIndex == 0)
            {
                if (!ValidateProfileName())
                    return;
            }

            PageIndex += 1;
        }

        private bool ValidateProfileName()
        {
            if (string.IsNullOrWhiteSpace(ProfileName))
            {
                // TODO: Localize
                MessageBoxEvent messageBoxEvent = new MessageBoxEvent
                {
                    Owner = View as IWindow,
                    MessageBoxText = "Please enter a name for your profile.",
                    Caption = "New profile",
                    MessageBoxButton = MessageBoxButton.OK,
                    MessageBoxImage = MessageBoxImage.Information
                };

                _eventAggregator.Publish(messageBoxEvent);
                return false;
            }

            // TODO
            return true;
        }

        private void Previous()
        {
            if (PageIndex > 0)
                PageIndex -= 1;
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
    }
}
