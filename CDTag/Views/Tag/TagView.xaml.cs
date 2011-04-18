using System.Windows.Controls;
using CDTag.ViewModel.Tag;

namespace CDTag.Views.Tag
{
    /// <summary>
    /// Interaction logic for TagView.xaml
    /// </summary>
    public partial class TagView : UserControl
    {
        public TagView(ITagViewModel viewModel)
        {
            InitializeComponent();

            FileExplorer.DirectoryController.NavigateTo(@"C:\");

            DataContext = viewModel; // TODO: Put this in ViewBase
            viewModel.DirectoryController = FileExplorer.DirectoryController;
        }
    }
}
