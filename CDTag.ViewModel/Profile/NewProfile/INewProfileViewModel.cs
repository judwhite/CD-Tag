using System.Windows.Input;
using CDTag.Common;

namespace CDTag.ViewModel.Profile.NewProfile
{
    public interface INewProfileViewModel : IViewModelBase
    {
        bool CreateNFO { get; set; }
        bool CreateSampleNFO { get; set; }
        bool HasExistingNFO { get; set; }
        ICommand PreviousCommand { get; }
        ICommand NextCommand { get; }
        int PageIndex { get; }
        bool UseUnderscores { get; set; }
        bool UseStandardCharactersOnly { get; set; }
        bool UseLatinCharactersOnly { get; set; }
    }
}
