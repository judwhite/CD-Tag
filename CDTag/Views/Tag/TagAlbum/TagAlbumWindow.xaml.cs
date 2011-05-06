using CDTag.ViewModel.Tag.TagAlbum;

namespace CDTag.Views.Tag.TagAlbum
{
    /// <summary>
    /// Interaction logic for TagAlbumWindow.xaml
    /// </summary>
    public partial class TagAlbumWindow : WindowViewBase
    {
        public TagAlbumWindow(ITagAlbumViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
