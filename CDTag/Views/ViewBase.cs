using System;
using System.Windows.Controls;
using CDTag.Common;

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
            // Note: only here to support XAML
            throw new NotImplementedException();
        }
    }
}
