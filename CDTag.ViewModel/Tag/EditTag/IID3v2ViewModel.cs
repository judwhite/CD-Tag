using System.Collections.ObjectModel;
using CDTag.Common;
using CDTag.Model.Tag;
using IdSharp.AudioInfo;
using IdSharp.Tagging.ID3v2;

namespace CDTag.ViewModels.Tag.EditTag
{
    public interface IID3v2ViewModel : IViewModelBase
    {
        ObservableCollection<string> GenreCollection { get; }
        ObservableCollection<ID3v2TagVersion> ID3v2VersionCollection { get; }
        ObservableCollection<PictureType> PictureTypeCollection { get; }
        
        string FullFileName { get; }
        string ShortFileName { get; }

        IID3v2Tag ID3v2 { get; }
        IAudioFile AudioFile { get; }
        ObservableCollection<Picture> PictureCollection { get; }
        Picture CurrentPicture { get; }
        
        string EncoderPreset { get; }
        bool CanSave { get; }
    }
}
