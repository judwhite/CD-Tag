using System;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using CDTag.FileBrowser;
using CDTag.FileBrowser.ViewModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tag
{
    public partial class TagViewModel : ViewModelBase<ITagViewModel>, ITagViewModel
    {
        public TagViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            BackCommand = new DelegateCommand(() => { DirectoryController.GoBack(); RaiseNavigationCanExecuteChanged(); }, () => DirectoryController == null ? false : DirectoryController.IsGoBackEnabled);
            ForwardCommand = new DelegateCommand(() => { DirectoryController.GoForward(); RaiseNavigationCanExecuteChanged(); }, () => DirectoryController == null ? false : DirectoryController.IsGoForwardEnabled);
            UpCommand = new DelegateCommand(() => { DirectoryController.GoUp(); RaiseNavigationCanExecuteChanged(); }, () => DirectoryController == null ? false : true /* TODO: IsEnabled logic */ );
            ExitCommand = new DelegateCommand(() => Application.Current.MainWindow.Close());
            SelectAllCommand = new DelegateCommand(() => DirectoryController.SelectAll());
            InvertSelectionCommand = new DelegateCommand(() => DirectoryController.InvertSelection());

            EnhancedPropertyChanged += TagViewModel_EnhancedPropertyChanged;

            RegisterCommandBindings();
        }

        private void RegisterCommandBindings()
        {
            RegisterCommandBinding(ModifierKeys.Alt, Key.Left, BackCommand);
            RegisterCommandBinding(ModifierKeys.Alt, Key.Right, ForwardCommand);
            RegisterCommandBinding(ModifierKeys.Alt, Key.Up, UpCommand);
            RegisterCommandBinding(ModifierKeys.Control, Key.A, SelectAllCommand);
        }

        private void TagViewModel_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<ITagViewModel> e)
        {
            if (e.IsProperty(p => p.DirectoryController))
            {
                var oldValue = (DirectoryController)e.OldValue;
                if (oldValue != null)
                    oldValue.NavigationComplete -= DirectoryController_NavigationComplete;

                if (DirectoryController != null)
                {
                    if (oldValue == null)
                    {
                        DirectoryController.NavigateTo(@"C:\");
                    }
                    DirectoryController.NavigationComplete += DirectoryController_NavigationComplete;
                }

                RaiseNavigationCanExecuteChanged();
            }
        }

        private void DirectoryController_NavigationComplete(object sender, EventArgs e)
        {
            RaiseNavigationCanExecuteChanged();
        }

        public IDirectoryController DirectoryController
        {
            get { return Get<DirectoryController>(); }
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

        private void RaiseNavigationCanExecuteChanged()
        {
            ((DelegateCommand)BackCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)ForwardCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)UpCommand).RaiseCanExecuteChanged();
        }
    }
}
