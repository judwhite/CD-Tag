using System;
using System.Windows;
using System.Windows.Threading;
using CDTag.Common;
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
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        public bool? ShowWindow<T>()
            where T : WindowViewBase
        {
            T window;
            MouseHelper.SetWaitCursor();
            try
            {
                window = Unity.Resolve<T>();
                window.Owner = MainWindow;
            }
            finally
            {
                MouseHelper.ResetCursor();
            }

            return window.ShowDialog();
        }
        
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}
