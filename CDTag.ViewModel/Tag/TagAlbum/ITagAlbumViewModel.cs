using System.Windows.Input;
using CDTag.Common;
using CDTag.Model.Tag;

namespace CDTag.ViewModels.Tag.TagAlbum
{
    public interface ITagAlbumViewModel : IViewModelBase
    {
        Album Album { get; }
        int SelectedTrackIndex { get; set; }
        AlbumTrack SelectedTrack { get; set; }
        ICommand FinishCommand { get; }
        ICommand PreviewNFOCommand { get; }
        ICommand PreviousCommand { get; }
    }
}
