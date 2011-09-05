﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using CDTag.Common;
using CDTag.Controls;
using CDTag.FileBrowser.Events;
using CDTag.View.Interfaces;
using CDTag.Views;

namespace CDTag.View
{
    public class DialogService : IDialogService
    {
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

            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(new Action(() => ShowError(exception, errorGrid)));
                return;
            }

            ErrorNotification errorNotification = new ErrorNotification();

            errorGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            errorNotification.SetValue(Grid.RowProperty, errorGrid.RowDefinitions.Count - 1);
            errorGrid.Children.Add(errorNotification);
            errorGrid.UpdateLayout(); // Force OnApplyTemplate for ErrorNotification

            errorNotification.Show(exception);
        }

        public string LocalApplicationDirectory
        {
            get
            {
                string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"CD-Tag");
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                return directory;
            }
        }

        public void CloseAddressTextBox()
        {
            IoC.Resolve<IEventAggregator>().Publish<CloseAddressTextBoxEvent>(null);
        }
    }
}