using System.Windows;
using CDTag.Common;
using CDTag.Events;
using CDTag.FileBrowser.ViewModel;
using CDTag.Views.About;
using CDTag.Views.Tag.EditTag;
using CDTag.Views.Tag.MassTag;
using CDTag.Views.Tag.TagAlbum;
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
