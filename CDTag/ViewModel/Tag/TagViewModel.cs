using System.Windows;
using CDTag.Common;
using CDTag.Events;
using CDTag.FileBrowser.ViewModel;
using CDTag.Views.About;
using CDTag.Views.Checksum;
using CDTag.Views.Options;
using CDTag.Views.Profile.EditProfile;
using CDTag.Views.Profile.NewProfile;
using CDTag.Views.Tag.EditTag;
using CDTag.Views.Tag.MassTag;
using CDTag.Views.Tag.TagAlbum;
using CDTag.Views.Tools;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tag
{
    public partial class TagViewModel : ViewModelBase<ITagViewModel>, ITagViewModel
    {
        public TagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            AboutCommand = new DelegateCommand(() => Unity.App.ShowWindow<AboutWindow>());
            ExitCommand = new DelegateCommand(() => Application.Current.MainWindow.Close());
            TagAlbumCommand = new DelegateCommand(() => Unity.App.ShowWindow<TagAlbumWindow>());
            EditTagsCommand = new DelegateCommand(() => Unity.App.ShowWindow<EditTagWindow>());
            MassTagCommand = new DelegateCommand(() => Unity.App.ShowWindow<MassTagWindow>());
            NewProfileCommand = new DelegateCommand(() => Unity.App.ShowWindow<NewProfileWindow>());
            EditProfileCommand = new DelegateCommand(() => Unity.App.ShowWindow<EditProfileWindow>());
            SplitCueSheetCommand = new DelegateCommand(() => Unity.App.ShowWindow<SplitCueWindow>());
            EncodingInspectorCommand = new DelegateCommand(() => Unity.App.ShowWindow<EncodingInspectorWindow>());
            OptionsCommand = new DelegateCommand(() => Unity.App.ShowWindow<OptionsWindow>());
            CreateChecksumCommand = new DelegateCommand(() => Unity.App.ShowWindow<ChecksumWindow>());
            VerifyChecksumCommand = new DelegateCommand(() => Unity.App.ShowWindow<ChecksumWindow>());
            VerifyEACLogCommand = new DelegateCommand(() => Unity.App.ShowWindow<VerifyEACLogWindow>());

            EnhancedPropertyChanged += TagViewModel_EnhancedPropertyChanged;

            eventAggregator.GetEvent<GetDirectoryControllerEvent>().Subscribe(OnGetDirectoryController);
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
            get { return Get<IDirectoryController>(); }
            set { Set(value); }
        }

        public bool IsShowStatusBarChecked
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }

        public bool IsShowNavigationPaneChecked
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }
    }
}
