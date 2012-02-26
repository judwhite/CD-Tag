using System;
using System.Windows;
using CDTag.Common.ApplicationServices;
using CDTag.Common.Wpf;
using CDTag.Views.Interfaces;

namespace CDTag.ViewModel.Tests
{
    public class UnitTestDialogService : IDialogService
    {
        public void ShowError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void ShowError(Exception exception, IErrorContainer errorContainer)
        {
            throw new NotImplementedException();
        }

        public bool? ShowWindow<T>()
            where T : IWindow
        {
            throw new NotImplementedException();
        }

        public void CloseAddressTextBox()
        {
            throw new NotImplementedException();
        }

        public bool? ShowOpenFileDialogResult { get; set; }
        public string ShowOpenFileDialogFileName { get; set; }

        public bool? ShowWindow(IWindow window, Window owner)
        {
            throw new NotImplementedException();
        }

        public bool? ShowWindow<T>(Window owner) 
            where T : IWindow
        {
            throw new NotImplementedException();
        }

        public bool? ShowOpenFileDialog(string title, string filter, Window owner, out string fileName)
        {
            fileName = ShowOpenFileDialogFileName;
            return ShowOpenFileDialogResult;
        }
    }
}
