using System.Windows;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;
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
        public Container Container { get; set; }

        public Bootstrapper()
        {
            Container = new Container();
        }

        public void Run()
        {
            ConfigureContainer();
            CreateShell();
        }

        protected void ConfigureContainer()
        {
            // Instances
            Container.RegisterInstance<IApp>(Application.Current);
            Container.RegisterInstance<ITagViewModel>(Container.Resolve<TagViewModel>());

            // Views
            Container.RegisterType<IAboutWindow, AboutWindow>();
            Container.RegisterType<IEditTagWindow, ID3v2Window>();
            Container.RegisterType<ITagAlbumWindow, TagAlbumWindow>();
            Container.RegisterType<IMassTagWindow, MassTagWindow>();
            Container.RegisterType<IEditProfileWindow, EditProfileWindow>();
            Container.RegisterType<INewProfileWindow, NewProfileWindow>();
            Container.RegisterType<ISplitCueWindow, SplitCueWindow>();
            Container.RegisterType<IEncodingInspectorWindow, EncodingInspectorWindow>();
            Container.RegisterType<IOptionsWindow, OptionsWindow>();
            Container.RegisterType<IChecksumWindow, ChecksumWindow>();
            Container.RegisterType<IVerifyEACLogWindow, VerifyEACLogWindow>();

            // View models
            Container.RegisterType<IDirectoryController, DirectoryController>();
            Container.RegisterType<IAboutViewModel, AboutViewModel>();
            Container.RegisterType<IID3v2ViewModel, ID3v2ViewModel>();
            Container.RegisterType<ITagAlbumViewModel, TagAlbumViewModel>();
            Container.RegisterType<IMassTagViewModel, MassTagViewModel>();
            Container.RegisterType<IEditProfileViewModel, EditProfileViewModel>();
            Container.RegisterType<INewProfileViewModel, NewProfileViewModel>();
            Container.RegisterType<ISplitCueViewModel, SplitCueViewModel>();
            Container.RegisterType<IEncodingInspectorViewModel, EncodingInspectorViewModel>();
            Container.RegisterType<IOptionsViewModel, OptionsViewModel>();
            Container.RegisterType<IChecksumViewModel, ChecksumViewModel>();
            Container.RegisterType<IVerifyEACLogViewModel, VerifyEACLogViewModel>();

            Unity.Container = Container;
        }

        protected DependencyObject CreateShell()
        {
            MainWindow shell = Container.Resolve<MainWindow>();
            shell.Show();

            return shell;
        }
    }
}
