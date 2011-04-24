using System;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;
using CDTag.Views.About;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tag
{
    public partial class TagViewModel : ViewModelBase<ITagViewModel>, ITagViewModel
    {
        public TagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            AboutCommand = new DelegateCommand(() => ((App)Application.Current).ShowWindow<AboutWindow>());
            ExitCommand = new DelegateCommand(() => Application.Current.MainWindow.Close());

            EnhancedPropertyChanged += TagViewModel_EnhancedPropertyChanged;
        }

        private void TagViewModel_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<ITagViewModel> e)
        {
            if (e.IsProperty(p => p.DirectoryViewModel))
            {
                RegisterCommandBindings();

                DirectoryViewModel.CurrentDirectory = @"C:\";
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
