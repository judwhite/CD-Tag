using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.About
{
    public class AboutViewModel : ViewModelBase, IAboutViewModel
    {
        public AboutViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            CloseCommand = new DelegateCommand<Window>((window) => window.Close());
            NavigateCommand = new DelegateCommand<Uri>(Navigate);
        }

        private static void Navigate(Uri uri)
        {
            try
            {
                Process.Start(uri.AbsoluteUri);
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public ICommand NavigateCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public string ReleaseNotes
        {
            get { return Get<string>(); }
            private set { Set(value); }
        }

        public ObservableCollection<string> ComponentsCollection
        {
            get { return Get<ObservableCollection<string>>(); }
            private set { Set(value); }
        }

        public ICommand CopyComponentCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand CloseCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }
    }
}
