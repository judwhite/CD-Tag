using System.Windows.Input;
using CDTag.Common;

namespace CDTag.ViewModels.Tag
{
    public partial class TagViewModel : ViewModelBase<ITagViewModel>, ITagViewModel
    {
        public ICommand NewProfileCommand
        {
            get { return Get<ICommand>("NewProfileCommand"); }
            private set { Set("NewProfileCommand", value); }
        }

        public ICommand EditProfileCommand
        {
            get { return Get<ICommand>("EditProfileCommand"); }
            private set { Set("EditProfileCommand", value); }
        }

        public ICommand AboutCommand
        {
            get { return Get<ICommand>("AboutCommand"); }
            private set { Set("AboutCommand", value); }
        }

        public ICommand TagAlbumCommand
        {
            get { return Get<ICommand>("TagAlbumCommand"); }
            private set { Set("TagAlbumCommand", value); }
        }

        public ICommand EditTagsCommand
        {
            get { return Get<ICommand>("EditTagsCommand"); }
            private set { Set("EditTagsCommand", value); }
        }

        public ICommand CheckForUpdatesCommand
        {
            get { return Get<ICommand>("CheckForUpdatesCommand"); }
            private set { Set("CheckForUpdatesCommand", value); }
        }

        public ICommand ExitCommand
        {
            get { return Get<ICommand>("ExitCommand"); }
            private set { Set("ExitCommand", value); }
        }

        public ICommand CutCommand
        {
            get { return Get<ICommand>("CutCommand"); }
            private set { Set("CutCommand", value); }
        }

        public ICommand CopyCommand
        {
            get { return Get<ICommand>("CopyCommand"); }
            private set { Set("CopyCommand", value); }
        }

        public ICommand PasteCommand
        {
            get { return Get<ICommand>("PasteCommand"); }
            private set { Set("PasteCommand", value); }
        }

        public ICommand CopyToFolderCommand
        {
            get { return Get<ICommand>("CopyToFolderCommand"); }
            private set { Set("CopyToFolderCommand", value); }
        }

        public ICommand MoveToFolderCommand
        {
            get { return Get<ICommand>("MoveToFolderCommand"); }
            private set { Set("MoveToFolderCommand", value); }
        }

        public ICommand RefreshCommand
        {
            get { return Get<ICommand>("RefreshCommand"); }
            private set { Set("RefreshCommand", value); }
        }

        public ICommand CreateChecksumCommand
        {
            get { return Get<ICommand>("CreateChecksumCommand"); }
            private set { Set("CreateChecksumCommand", value); }
        }

        public ICommand VerifyChecksumCommand
        {
            get { return Get<ICommand>("VerifyChecksumCommand"); }
            private set { Set("VerifyChecksumCommand", value); }
        }

        public ICommand VerifyEacLogCommand
        {
            get { return Get<ICommand>("VerifyEacLogCommand"); }
            private set { Set("VerifyEacLogCommand", value); }
        }

        public ICommand SplitCueSheetCommand
        {
            get { return Get<ICommand>("SplitCueSheetCommand"); }
            private set { Set("SplitCueSheetCommand", value); }
        }

        public ICommand EncodingInspectorCommand
        {
            get { return Get<ICommand>("EncodingInspectorCommand"); }
            private set { Set("EncodingInspectorCommand", value); }
        }

        public ICommand MassTagCommand
        {
            get { return Get<ICommand>("MassTagCommand"); }
            private set { Set("MassTagCommand", value); }
        }

        public ICommand ShowHelpCommand
        {
            get { return Get<ICommand>("ShowHelpCommand"); }
            private set { Set("ShowHelpCommand", value); }
        }

        public ICommand HomePageCommand
        {
            get { return Get<ICommand>("HomePageCommand"); }
            private set { Set("HomePageCommand", value); }
        }

        public ICommand SupportForumCommand
        {
            get { return Get<ICommand>("SupportForumCommand"); }
            private set { Set("SupportForumCommand", value); }
        }

        public ICommand OptionsCommand
        {
            get { return Get<ICommand>("OptionsCommand"); }
            private set { Set("OptionsCommand", value); }
        }

        public ICommand VerifyEACLogCommand
        {
            get { return Get<ICommand>("VerifyEACLogCommand"); }
            private set { Set("VerifyEACLogCommand", value); }
        }
    }
}
