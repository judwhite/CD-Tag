using System;
using System.Windows;
using System.Windows.Threading;

namespace CDTag
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
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
            MessageBox.Show(e.Exception.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
