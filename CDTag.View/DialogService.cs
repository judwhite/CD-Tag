using System;
using System.Windows;
using System.Windows.Controls;
using CDTag.Common;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Dispatcher;
using CDTag.Common.Mvvm;
using CDTag.Common.Wpf;
using CDTag.Controls;
using CDTag.FileBrowser.Events;
using CDTag.View.Interfaces;
using CDTag.Views;
using Microsoft.Win32;

namespace CDTag.View
{
    public class DialogService : IDialogService
    {
        private static readonly IDispatcher _dispatcher;

        static DialogService()
        {
            _dispatcher = IoC.Resolve<IDispatcher>();
        }

        public bool? ShowWindow<T>()
            where T : IWindow
        {
            T window;
            MouseHelper.SetWaitCursor();
            try
            {
                window = IoC.Resolve<T>();
                window.Owner = Application.Current.MainWindow; // TODO ?
                var viewModel = window.DataContext as IViewModelBase;
                if (viewModel != null)
                    viewModel.CloseWindow = () => window.Close();
            }
            finally
            {
                MouseHelper.ResetCursor();
            }

            return window.ShowDialog();
        }

        public void ShowError(Exception exception)
        {
            ErrorContainer errorItems = ((MainWindow)Application.Current.MainWindow).ErrorItems;
            ShowError(exception, errorItems);
        }

        public void ShowError(Exception exception, IErrorContainer errorContainer)
        {
            if (exception == null)
                return;

            ErrorContainer errorGrid = (ErrorContainer)errorContainer;

            if (!_dispatcher.CheckAccess())
            {
                _dispatcher.BeginInvoke(() => ShowError(exception, errorGrid));
                return;
            }

            ErrorNotification errorNotification = new ErrorNotification();

            errorGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            errorNotification.SetValue(Grid.RowProperty, errorGrid.RowDefinitions.Count - 1);
            errorGrid.Children.Add(errorNotification);
            errorGrid.UpdateLayout(); // Force OnApplyTemplate for ErrorNotification

            errorNotification.Show(exception);
        }

        public void CloseAddressTextBox()
        {
            IoC.Resolve<IEventAggregator>().Publish<CloseAddressTextBoxEvent>(null);
        }

        public bool? ShowOpenFileDialog(string title, string filter, out string fileName)
        {
            OpenFileDialog openFileDialog;
            MouseHelper.SetWaitCursor();
            try
            {
                openFileDialog = new OpenFileDialog();
                openFileDialog.Title = title;
                openFileDialog.Filter = filter;
            }
            finally
            {
                MouseHelper.ResetCursor();
            }

            bool? result = openFileDialog.ShowDialog(Application.Current.MainWindow);

            if (result == true)
                fileName = openFileDialog.FileName;
            else
                fileName = null;

            return result;
        }
    }
}
