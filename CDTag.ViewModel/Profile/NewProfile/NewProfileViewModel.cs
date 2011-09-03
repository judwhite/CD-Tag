using System;
using System.Windows.Input;
using CDTag.Common;

namespace CDTag.ViewModel.Profile.NewProfile
{
    public class NewProfileViewModel : ViewModelBase<NewProfileViewModel>, INewProfileViewModel
    {
        private readonly DelegateCommand _nextCommand;
        private readonly DelegateCommand _previousCommand;

        public NewProfileViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _nextCommand = new DelegateCommand(Next, () => PageIndex < 1);
            _previousCommand = new DelegateCommand(Previous, () => PageIndex > 0);

            EnhancedPropertyChanged += new EnhancedPropertyChangedEventHandler<NewProfileViewModel>(NewProfileViewModel_EnhancedPropertyChanged);
        }

        private void NewProfileViewModel_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<NewProfileViewModel> e)
        {
            if (e.IsProperty(p => p.CreateNFO))
            {
                if (!CreateNFO)
                {
                    CreateSampleNFO = false;
                    HasExistingNFO = false;
                }
                else
                {
                    if (!CreateSampleNFO && !HasExistingNFO)
                        CreateSampleNFO = true;
                }
            }
        }

        private void Next()
        {
            PageIndex += 1;
        }

        private void Previous()
        {
            if (PageIndex > 0)
                PageIndex -= 1;
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
