using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Events;
using CDTag.FileBrowser.ViewModel;
using CDTag.ViewModel.Tag.EditTag;
using CDTag.Views.About;
using CDTag.Views.Tag.EditTag;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tag
{
    public partial class TagViewModel : ViewModelBase<ITagViewModel>, ITagViewModel
    {
        public TagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            //AboutCommand = new DelegateCommand(() => { throw new NotImplementedException(); });
            AboutCommand = new DelegateCommand(() => ((App)Application.Current).ShowWindow<AboutWindow>());
            ExitCommand = new DelegateCommand(() => Application.Current.MainWindow.Close());
            TagAlbumCommand = new DelegateCommand(() => { throw new NotImplementedException(); });
            EditTagsCommand = new DelegateCommand(ShowEditTags);

            EnhancedPropertyChanged += TagViewModel_EnhancedPropertyChanged;

            eventAggregator.GetEvent<GetDirectoryControllerEvent>().Subscribe(OnGetDirectoryController);
        }

        private static void ShowEditTags()
        {
            ((App)Application.Current).ShowWindow<EditTagWindow>();
        }

        private void OnGetDirectoryController(GetDirectoryControllerEventArgs e)
        {
            e.DirectoryController = DirectoryViewModel;
        }

        private void TagViewModel_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<ITagViewModel> e)
        {
            if (e.IsProperty(p => p.DirectoryViewModel))
            {
                RegisterCommandBindings();

                DirectoryViewModel.InitialDirectory = @"C:\";
            }
        }

        private void RegisterCommandBindings()
        {
            RegisterCommandBinding(ModifierKeys.Alt, Key.Left, DirectoryViewModel.GoBackCommand);
            RegisterCommandBinding(ModifierKeys.Alt, Key.Right, DirectoryViewModel.GoForwardCommand);
            RegisterCommandBinding(ModifierKeys.Alt, Key.Up, DirectoryViewModel.GoUpCommand);
            RegisterCommandBinding(ModifierKeys.Control, Key.A, DirectoryViewModel.SelectAllCommand);
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
