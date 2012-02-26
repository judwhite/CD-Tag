using System.Collections.ObjectModel;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Common.Mvvm;
using CDTag.Model.Profile;
using CDTag.Model.Profile.NewProfile;

namespace CDTag.ViewModels.Profile.NewProfile
{
    public interface INewProfileViewModel : IViewModelBase
    {
        bool CreateNFO { get; set; }
        bool CreateSampleNFO { get; set; }
        bool HasExistingNFO { get; set; }
        ICommand PreviousCommand { get; }
        ICommand NextCommand { get; }
        int PageIndex { get; }
        bool IsProfileNameFocused { get; set; }
        ObservableCollection<FormatItem> DirectoryFormats { get; }
        ObservableCollection<FormatItem> AudioFileFormats { get; }
        FormatItem DirectoryFormat { get; set; }
        FormatItem AudioFileFormat { get; set; }
        UserProfile Profile { get; }
        string NextButtonText { get; }
    }
}
