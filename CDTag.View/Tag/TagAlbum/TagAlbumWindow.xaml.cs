using CDTag.View.Interfaces.Tag.TagAlbum;
using CDTag.ViewModels.Tag.TagAlbum;

namespace CDTag.Views.Tag.TagAlbum
{
    /// <summary>
    /// Interaction logic for TagAlbumWindow.xaml
    /// </summary>
    public partial class TagAlbumWindow : WindowViewBase, ITagAlbumWindow
    {
        public TagAlbumWindow(ITagAlbumViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
