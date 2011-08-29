using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Model.About;

namespace CDTag.ViewModel.About
{
    public class AboutViewModel : ViewModelBase, IAboutViewModel
    {
        public AboutViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            CloseCommand = new DelegateCommand<Window>(window => window.Close());
            NavigateCommand = new DelegateCommand<Uri>(Navigate);
            CopyComponentCommand = new DelegateCommand(CopyComponentInformation);

            var assembly = Assembly.GetEntryAssembly();
            object[] attributes = assembly.GetCustomAttributes(true);
            CopyrightText = attributes.OfType<AssemblyCopyrightAttribute>().Single().Copyright;

            var version = assembly.GetName().Version;

            VersionText = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

            ReleaseDate = new DateTime(2000, 1, 1).AddDays(version.Build);

            Thread thread = new Thread(GetAssemblies);
            thread.Start();
        }

        private void GetAssemblies()
        {
            try
            {
                string appDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string[] files = Directory.GetFiles(appDir, "*.dll", SearchOption.TopDirectoryOnly);
                ObservableCollection<ComponentInformation> items = new ObservableCollection<ComponentInformation>();
                foreach (string file in files)
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(file);
                        AssemblyName assemblyName = assembly.GetName();
                        Version version = assemblyName.Version;

                        ComponentInformation info = new ComponentInformation();
                        info.Name = assemblyName.Name;
                        info.Version = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
                        info.SortVersion = string.Format("{0:0000000}.{1:0000000}.{2:0000000}.{3:0000000}", version.Major, version.Minor, version.Build, version.Revision);
                        
                        items.Add(info);
                    }
                    catch (Exception ex)
                    {
                        ShowException(ex);
                    }
                }

                Application.Current.Dispatcher.Invoke(new Action(() => ComponentsCollection = items));
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
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

        private void CopyComponentInformation()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in ComponentsCollection)
            {
                stringBuilder.AppendLine(string.Format("{0}\t{1}", item.Name, item.Version));
            }

            Clipboard.SetText(stringBuilder.ToString());

            // TODO: Localize
            MessageBox.Show("Component information copied to clipboard.", "Copy Component Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public ICommand NavigateCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public string ReleaseNotes
        {
            get { return Get<string>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ObservableCollection<ComponentInformation> ComponentsCollection
        {
            get { return Get<ObservableCollection<ComponentInformation>>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand CopyComponentCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand CloseCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public string CopyrightText
        {
            get { return Get<string>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public string VersionText
        {
            get { return Get<string>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public DateTime ReleaseDate
        {
            get { return Get<DateTime>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }
    }
}
