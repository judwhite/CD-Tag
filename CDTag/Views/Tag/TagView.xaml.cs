using System.ComponentModel;
using CDTag.Common;
using CDTag.ViewModel.Tag;

namespace CDTag.Views.Tag
{
    /// <summary>
    /// Interaction logic for TagView.xaml
    /// </summary>
    public partial class TagView : ViewBase
    {
        public TagView()
            : this(Unity.Resolve<ITagViewModel>())
        {
        }

        public TagView(ITagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            FileExplorer.Toolbar = Unity.Resolve<TagToolbar>();

            // TODO: Move to ViewModel
            viewModel.DirectoryController = FileExplorer.DirectoryController;
        }
    }
}
