using System.Windows;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;
using CDTag.View;
using CDTag.View.Interfaces.About;
using CDTag.View.Interfaces.Checksum;
using CDTag.View.Interfaces.Options;
using CDTag.View.Interfaces.Profile.EditProfile;
using CDTag.View.Interfaces.Profile.NewProfile;
using CDTag.View.Interfaces.Tag.EditTag;
using CDTag.View.Interfaces.Tag.MassTag;
using CDTag.View.Interfaces.Tag.TagAlbum;
using CDTag.View.Interfaces.Tools;
using CDTag.ViewModel.About;
using CDTag.ViewModel.Checksum;
using CDTag.ViewModel.Events;
using CDTag.ViewModel.Options;
using CDTag.ViewModel.Profile.EditProfile;
using CDTag.ViewModel.Profile.NewProfile;
using CDTag.ViewModel.Tag;
using CDTag.ViewModel.Tag.EditTag;
using CDTag.ViewModel.Tag.MassTag;
using CDTag.ViewModel.Tag.TagAlbum;
using CDTag.ViewModel.Tools;
using CDTag.Views;
using CDTag.Views.About;
using CDTag.Views.Checksum;
using CDTag.Views.Options;
using CDTag.Views.Profile.EditProfile;
using CDTag.Views.Profile.NewProfile;
using CDTag.Views.Tag.EditTag;
using CDTag.Views.Tag.MassTag;
using CDTag.Views.Tag.TagAlbum;
using CDTag.Views.Tools;

namespace CDTag
{
    public class Bootstrapper
    {
        public void Run()
        {
            ConfigureContainer();
            CreateShell();
        }

        protected void ConfigureContainer()
        {
            // Instances
            IoC.RegisterInstance<IDialogService>(new DialogService());
            IoC.RegisterInstance<ITagViewModel>(IoC.Resolve<TagViewModel>());

            // Views
            IoC.RegisterType<IAboutWindow, AboutWindow>();
            IoC.RegisterType<IEditTagWindow, ID3v2Window>();
            IoC.RegisterType<ITagAlbumWindow, TagAlbumWindow>();
            IoC.RegisterType<IMassTagWindow, MassTagWindow>();
            IoC.RegisterType<IEditProfileWindow, EditProfileWindow>();
            IoC.RegisterType<INewProfileWindow, NewProfileWindow>();
            IoC.RegisterType<ISplitCueWindow, SplitCueWindow>();
            IoC.RegisterType<IEncodingInspectorWindow, EncodingInspectorWindow>();
            IoC.RegisterType<IOptionsWindow, OptionsWindow>();
            IoC.RegisterType<IChecksumWindow, ChecksumWindow>();
            IoC.RegisterType<IVerifyEACLogWindow, VerifyEACLogWindow>();

            // View models
            IoC.RegisterType<IDirectoryController, DirectoryController>();
            IoC.RegisterType<IAboutViewModel, AboutViewModel>();
            IoC.RegisterType<IID3v2ViewModel, ID3v2ViewModel>();
            IoC.RegisterType<ITagAlbumViewModel, TagAlbumViewModel>();
            IoC.RegisterType<IMassTagViewModel, MassTagViewModel>();
            IoC.RegisterType<IEditProfileViewModel, EditProfileViewModel>();
            IoC.RegisterType<INewProfileViewModel, NewProfileViewModel>();
            IoC.RegisterType<ISplitCueViewModel, SplitCueViewModel>();
            IoC.RegisterType<IEncodingInspectorViewModel, EncodingInspectorViewModel>();
            IoC.RegisterType<IOptionsViewModel, OptionsViewModel>();
            IoC.RegisterType<IChecksumViewModel, ChecksumViewModel>();
            IoC.RegisterType<IVerifyEACLogViewModel, VerifyEACLogViewModel>();

            // Events
            IoC.Resolve<IEventAggregator>().Subscribe<MessageBoxEvent>(ShowMessageBox);
        }

        private static void ShowMessageBox(MessageBoxEvent messageBox)
        {
            MessageBox.Show(
                owner: messageBox.Owner as Window,
                messageBoxText: messageBox.MessageBoxText,
                caption: messageBox.Caption,
                button: messageBox.MessageBoxButton,
                icon: messageBox.MessageBoxImage
            );
        }

        protected DependencyObject CreateShell()
        {
            MainWindow shell = IoC.Resolve<MainWindow>();
            shell.Show();

            return shell;
        }
    }
}
