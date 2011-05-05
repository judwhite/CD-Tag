using CDTag.Common;
using CDTag.ViewModel.Tag;

namespace CDTag.Views.Tag
{
    /// <summary>
    /// Interaction logic for TagView.xaml
    /// </summary>
    public partial class TagView : ViewBase
    {
        private readonly TagToolbar _tagToolbar;

        public TagView()
            : this(Unity.Resolve<ITagViewModel>())
        {
        }

        public TagView(ITagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            _tagToolbar = Unity.Resolve<TagToolbar>();
            FileExplorer.Toolbar = _tagToolbar;

            // TODO: Move to ViewModel
            viewModel.DirectoryViewModel = FileExplorer.DirectoryController;
        }

        public TagToolbar TagToolbar
        {
            get { return _tagToolbar; }
        }
    }
}
