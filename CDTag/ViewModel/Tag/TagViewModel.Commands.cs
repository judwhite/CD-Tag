using System.Windows.Input;
using CDTag.Common;

namespace CDTag.ViewModel.Tag
{
    public partial class TagViewModel : ViewModelBase<ITagViewModel>, ITagViewModel
    {
        public ICommand NewProfileCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand EditProfileCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand AboutCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand TagAlbumCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand EditTagsCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand CheckForUpdatesCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand ExitCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand CutCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand CopyCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand PasteCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand CopyToFolderCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand MoveToFolderCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand RefreshCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand CreateChecksumCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand VerifyChecksumCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand VerifyEacLogCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand SplitCueSheetCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand EncodingInspectorCommand
        {
            get { return Get<ICommand>(""); }
            private set { Set("", value); }
        }

        public ICommand MassTagCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand ShowHelpCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand HomePageCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        public ICommand SupportForumCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }
    }
}
