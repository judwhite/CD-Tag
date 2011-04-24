using System;
using System.Windows;
using CDTag.Common;

namespace CDTag.Views
{
    public class WindowViewBase : Window
    {
        protected WindowViewBase(IViewModelBase viewModel)
        {
            DataContext = viewModel;
        }

        public WindowViewBase()
        {
            // Note: only here to support XAML
            throw new NotSupportedException();
        }
    }
}
