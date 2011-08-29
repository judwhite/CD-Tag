using System.Reflection;
using System.Windows;
using CDTag.Common;
using CDTag.Events;
using CDTag.FileBrowser.ViewModel;
using CDTag.View.Interfaces.About;
using CDTag.View.Interfaces.Checksum;
using CDTag.View.Interfaces.Options;
using CDTag.View.Interfaces.Profile.EditProfile;
using CDTag.View.Interfaces.Profile.NewProfile;
using CDTag.View.Interfaces.Tag.EditTag;
using CDTag.View.Interfaces.Tag.MassTag;
using CDTag.View.Interfaces.Tag.TagAlbum;
using CDTag.View.Interfaces.Tools;

namespace CDTag.ViewModel.Tag
{
    public partial class TagViewModel : ViewModelBase<ITagViewModel>, ITagViewModel
    {
        public TagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            AboutCommand = new DelegateCommand(() => Unity.App.ShowWindow<IAboutWindow>());
            ExitCommand = new DelegateCommand(() => Application.Current.MainWindow.Close());
            TagAlbumCommand = new DelegateCommand(() => Unity.App.ShowWindow<ITagAlbumWindow>());
            EditTagsCommand = new DelegateCommand(() => Unity.App.ShowWindow<IEditTagWindow>());
            MassTagCommand = new DelegateCommand(() => Unity.App.ShowWindow<IMassTagWindow>());
            NewProfileCommand = new DelegateCommand(() => Unity.App.ShowWindow<INewProfileWindow>());
            EditProfileCommand = new DelegateCommand(() => Unity.App.ShowWindow<IEditProfileWindow>());
            SplitCueSheetCommand = new DelegateCommand(() => Unity.App.ShowWindow<ISplitCueWindow>());
            EncodingInspectorCommand = new DelegateCommand(() => Unity.App.ShowWindow<IEncodingInspectorWindow>());
            OptionsCommand = new DelegateCommand(() => Unity.App.ShowWindow<IOptionsWindow>());
            CreateChecksumCommand = new DelegateCommand(() => Unity.App.ShowWindow<IChecksumWindow>());
            VerifyChecksumCommand = new DelegateCommand(() => Unity.App.ShowWindow<IChecksumWindow>());
            VerifyEACLogCommand = new DelegateCommand(() => Unity.App.ShowWindow<IVerifyEACLogWindow>());

            EnhancedPropertyChanged += TagViewModel_EnhancedPropertyChanged;

            eventAggregator.GetEvent<GetDirectoryControllerEvent>().Subscribe(o => OnGetDirectoryController((GetDirectoryControllerEventArgs)o));
        }

        private void OnGetDirectoryController(GetDirectoryControllerEventArgs e)
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
