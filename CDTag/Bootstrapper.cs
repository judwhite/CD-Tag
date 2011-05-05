using System.Windows;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;
using CDTag.ViewModel.About;
using CDTag.ViewModel.Tag;
using CDTag.ViewModel.Tag.EditTag;
using CDTag.ViewModel.Tag.MassTag;
using CDTag.ViewModel.Tag.TagAlbum;
using CDTag.Views;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace CDTag
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterInstance(typeof(IApp), Application.Current);
            Container.RegisterInstance(typeof(ITagViewModel), Container.Resolve<TagViewModel>());
            Container.RegisterType(typeof(IDirectoryController), typeof(DirectoryController));
            Container.RegisterType(typeof(IAboutViewModel), typeof(AboutViewModel));
            Container.RegisterType(typeof(IEditTagViewModel), typeof(EditTagViewModel));
            Container.RegisterType(typeof(IID3v1ViewModel), typeof(ID3v1ViewModel));
            Container.RegisterType(typeof(ITagAlbumViewModel), typeof(TagAlbumViewModel));
            Container.RegisterType(typeof(IMassTagViewModel), typeof(MassTagViewModel));

            Unity.Container = Container;
        }

        protected override DependencyObject CreateShell()
        {
            MainWindow shell = Container.Resolve<MainWindow>();
            shell.Show();

            return shell;
        }
    }
}
