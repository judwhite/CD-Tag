using CDTag.ViewModels.Tag.TagAlbum;
using CDTag.Views.Interfaces.Tag.TagAlbum;

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
