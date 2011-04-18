using System.Windows.Controls;
using CDTag.ViewModel.Tag;

namespace CDTag.Views.Tag
{
    /// <summary>
    /// Interaction logic for TagToolbar.xaml
    /// </summary>
    public partial class TagToolbar : UserControl
    {
        public TagToolbar(ITagToolbarViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
