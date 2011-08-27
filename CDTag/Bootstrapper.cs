using System.Windows;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;
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
            Container.RegisterInstance<IApp>(Application.Current);
            Container.RegisterInstance<ITagViewModel>(Container.Resolve<TagViewModel>());

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
