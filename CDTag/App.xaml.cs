using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CDTag.Common;
using CDTag.Controls;
using CDTag.FileBrowser.Events;
using CDTag.View;
using CDTag.View.Interfaces;
using CDTag.Views;

namespace CDTag
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Note: Known bug with App.xaml having no StartupUri and the theme speicified in XAML's Application.Resources
            Resources = (ResourceDictionary)LoadComponent(new Uri("/Themes/Default/Theme.xaml", UriKind.RelativeOrAbsolute));

            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            IoC.Resolve<IDialogService>().ShowError(e.Exception);
            e.Handled = true;
        }
    }
}
