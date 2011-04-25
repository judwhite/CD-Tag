using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

            var assembly = Assembly.GetEntryAssembly();
            object[] attributes = assembly.GetCustomAttributes(true);
            CopyrightText = attributes.OfType<AssemblyCopyrightAttribute>().Single().Copyright;

            var version = assembly.GetName().Version;

            VersionText = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

            ReleaseDate = new DateTime(2000, 1, 1).AddDays(version.Build);
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

        public string CopyrightText
        {
            get { return Get<string>(); }
            private set { Set(value); }
        }

        public string VersionText
        {
            get { return Get<string>(); }
            private set { Set(value); }
        }

        public DateTime ReleaseDate
        {
            get { return Get<DateTime>(); }
            private set { Set(value); }
        }
    }
}
