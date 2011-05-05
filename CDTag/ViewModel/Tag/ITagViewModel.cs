using System.Windows.Input;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;

namespace CDTag.ViewModel.Tag
{
    public interface ITagViewModel : IViewModelBase
    {
        IDirectoryController DirectoryViewModel { get; set; }

        ICommand AboutCommand { get; }
        ICommand TagAlbumCommand { get; }
        ICommand EditTagsCommand { get; }
        ICommand MassTagCommand { get; }
        ICommand ExitCommand { get; }
        ICommand NewProfileCommand { get; }
        ICommand EditProfileCommand { get; }

        ICommand CheckForUpdatesCommand { get; }
        ICommand CutCommand { get; }
        ICommand CopyCommand { get; }
        ICommand PasteCommand { get; }
        ICommand CopyToFolderCommand { get; }
        ICommand MoveToFolderCommand { get; }
        ICommand RefreshCommand { get; }
        ICommand CreateChecksumCommand { get; }
        ICommand VerifyChecksumCommand { get; }
        ICommand VerifyEacLogCommand { get; }
        ICommand SplitCueSheetCommand { get; }
        ICommand EncodingInspectorCommand { get; }
        ICommand ShowHelpCommand { get; }
        ICommand HomePageCommand { get; }
        ICommand SupportForumCommand { get; }

        bool IsShowStatusBarChecked { get; set; }
        bool IsShowNavigationPaneChecked { get; set; }
    }
}
