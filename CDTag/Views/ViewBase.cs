using System.Windows.Controls;
using CDTag.Common;

namespace CDTag.Views
{
    public abstract class ViewBase : UserControl
    {
        protected ViewBase(IViewModelBase viewModel)
        {
            DataContext = viewModel;
        }
    }
}
