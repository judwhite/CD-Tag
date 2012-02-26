using System.Reflection;
using System.Windows;
using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Events;
using CDTag.Common.Mvvm;
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
using CDTag.ViewModels.Events;

namespace CDTag.ViewModels.Tag
{
    public partial class TagViewModel : ViewModelBase<ITagViewModel>, ITagViewModel
    {
        public TagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            IDialogService app = IoC.Resolve<IDialogService>();

            AboutCommand = new DelegateCommand(() => app.ShowWindow<IAboutWindow>());
            ExitCommand = new DelegateCommand(() => Application.Current.MainWindow.Close());
            TagAlbumCommand = new DelegateCommand(() => app.ShowWindow<ITagAlbumWindow>());
            EditTagsCommand = new DelegateCommand(() => app.ShowWindow<IEditTagWindow>());
            MassTagCommand = new DelegateCommand(() => app.ShowWindow<IMassTagWindow>());
            NewProfileCommand = new DelegateCommand(() => app.ShowWindow<INewProfileWindow>());
            EditProfileCommand = new DelegateCommand(() => app.ShowWindow<IEditProfileWindow>());
            SplitCueSheetCommand = new DelegateCommand(() => app.ShowWindow<ISplitCueWindow>());
            EncodingInspectorCommand = new DelegateCommand(() => app.ShowWindow<IEncodingInspectorWindow>());
            OptionsCommand = new DelegateCommand(() => app.ShowWindow<IOptionsWindow>());
            CreateChecksumCommand = new DelegateCommand(() => app.ShowWindow<IChecksumWindow>());
            VerifyChecksumCommand = new DelegateCommand(() => app.ShowWindow<IChecksumWindow>());
            VerifyEACLogCommand = new DelegateCommand(() => app.ShowWindow<IVerifyEACLogWindow>());

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
