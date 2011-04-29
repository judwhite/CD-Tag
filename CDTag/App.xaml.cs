using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CDTag.Common;
using CDTag.Controls;
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
            ErrorNotification errorNotification = new ErrorNotification();
                
            Grid errorItems = ((MainWindow)MainWindow).ErrorItems;
            errorItems.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            errorNotification.SetValue(Grid.RowProperty, errorItems.RowDefinitions.Count - 1);
            errorItems.Children.Add(errorNotification);
            errorItems.UpdateLayout(); // Force OnApplyTemplate for ErrorNotification

            errorNotification.Show(e.Exception);

            e.Handled = true;
        }
    }
}
