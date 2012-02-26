using System;
using System.Windows.Controls;
using CDTag.Common;
using CDTag.Common.Mvvm;

namespace CDTag.Views
{
    public class ViewBase : UserControl
    {
        protected ViewBase(IViewModelBase viewModel)
        {
            DataContext = viewModel;
        }

        public ViewBase()
        {
            // Note: only here to support XAML. Do not throw NotImplementedException() here, XAML complains
        }
    }
}
