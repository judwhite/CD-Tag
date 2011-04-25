using System;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;

namespace CDTag.Views
{
    public class WindowViewBase : Window
    {
        protected WindowViewBase(IViewModelBase viewModel)
        {
            DataContext = viewModel;

            PreviewKeyDown += WindowViewBase_PreviewKeyDown;
        }

        private void WindowViewBase_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        public WindowViewBase()
        {
            // Note: only here to support XAML
            throw new NotSupportedException();
        }
    }
}
