using System.Collections.ObjectModel;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Model.Profile.NewProfile;

namespace CDTag.ViewModel.Profile.NewProfile
{
    public interface INewProfileViewModel : IViewModelBase
    {
        string ProfileName { get; set; }
        bool CreateNFO { get; set; }
        bool CreateSampleNFO { get; set; }
        bool HasExistingNFO { get; set; }
        ICommand PreviousCommand { get; }
        ICommand NextCommand { get; }
        int PageIndex { get; }
        bool UseUnderscores { get; set; }
        bool UseStandardCharactersOnly { get; set; }
        bool UseLatinCharactersOnly { get; set; }
        bool IsProfileNameFocused { get; set; }
        ObservableCollection<FormatItem> DirectoryFormats { get; }
        ObservableCollection<FormatItem> AudioFileFormats { get; }
    }
}
