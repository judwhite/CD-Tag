using System.Reflection;
using System.Windows;
using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Events;
using CDTag.Common.Mvvm;
using CDTag.FileBrowser.ViewModel;
using CDTag.ViewModels.Events;
using CDTag.Views.Interfaces.About;
using CDTag.Views.Interfaces.Checksum;
using CDTag.Views.Interfaces.Options;
using CDTag.Views.Interfaces.Profile.EditProfile;
using CDTag.Views.Interfaces.Profile.NewProfile;
using CDTag.Views.Interfaces.Tag.EditTag;
using CDTag.Views.Interfaces.Tag.MassTag;
using CDTag.Views.Interfaces.Tag.TagAlbum;
using CDTag.Views.Interfaces.Tools;

namespace CDTag.ViewModels.Tag
{
    public partial class TagViewModel : ViewModelBase<ITagViewModel>, ITagViewModel
    {
        public TagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            AboutCommand = new DelegateCommand(() => ShowWindow<IAboutWindow>());
            ExitCommand = new DelegateCommand(() => Application.Current.MainWindow.Close());
            TagAlbumCommand = new DelegateCommand(() => ShowWindow<ITagAlbumWindow>());
            EditTagsCommand = new DelegateCommand(() => ShowWindow<IEditTagWindow>());
            MassTagCommand = new DelegateCommand(() => ShowWindow<IMassTagWindow>());
            NewProfileCommand = new DelegateCommand(() => ShowWindow<INewProfileWindow>());
            EditProfileCommand = new DelegateCommand(() => ShowWindow<IEditProfileWindow>());
            SplitCueSheetCommand = new DelegateCommand(() => ShowWindow<ISplitCueWindow>());
            EncodingInspectorCommand = new DelegateCommand(() => ShowWindow<IEncodingInspectorWindow>());
            OptionsCommand = new DelegateCommand(() => ShowWindow<IOptionsWindow>());
            CreateChecksumCommand = new DelegateCommand(() => ShowWindow<IChecksumWindow>());
            VerifyChecksumCommand = new DelegateCommand(() => ShowWindow<IChecksumWindow>());
            VerifyEACLogCommand = new DelegateCommand(() => ShowWindow<IVerifyEACLogWindow>());

            EnhancedPropertyChanged += TagViewModel_EnhancedPropertyChanged;

            eventAggregator.Subscribe<GetDirectoryControllerEvent>(OnGetDirectoryController);
        }

        private void OnGetDirectoryController(GetDirectoryControllerEvent e)
        {
            e.DirectoryController = DirectoryViewModel;
        }

        private void TagViewModel_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<ITagViewModel> e)
        {
            if (e.IsProperty(p => p.DirectoryViewModel))
            {
                DirectoryViewModel.InitialDirectory = @"C:\";
            }
        }

        public IDirectoryController DirectoryViewModel
        {
            get { return Get<IDirectoryController>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public bool IsShowStatusBarChecked
        {
            get { return Get<bool>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public bool IsShowNavigationPaneChecked
        {
            get { return Get<bool>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }
    }
}
