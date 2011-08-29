using System.Collections.ObjectModel;
using CDTag.Common;
using IdSharp.Tagging.ID3v1;

namespace CDTag.ViewModel.Tag.EditTag
{
    public interface IID3v1ViewModel : IViewModelBase
    {
        IID3v1Tag ID3v1 { get; }
        bool CanSave { get; }
        ObservableCollection<string> GenreCollection { get; }
        ObservableCollection<ID3v1TagVersion> ID3v1VersionCollection { get; }
    }
}
