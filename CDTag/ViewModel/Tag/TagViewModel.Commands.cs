using System.Reflection;
using System.Windows.Input;
using CDTag.Common;

namespace CDTag.ViewModel.Tag
{
    public partial class TagViewModel : ViewModelBase<ITagViewModel>, ITagViewModel
    {
        public ICommand NewProfileCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand EditProfileCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand AboutCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand TagAlbumCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand EditTagsCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand CheckForUpdatesCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand ExitCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand CutCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand CopyCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand PasteCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand CopyToFolderCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand MoveToFolderCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand RefreshCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand CreateChecksumCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand VerifyChecksumCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand VerifyEacLogCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand SplitCueSheetCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand EncodingInspectorCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand MassTagCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand ShowHelpCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand HomePageCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand SupportForumCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand OptionsCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ICommand VerifyEACLogCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }
    }
}
