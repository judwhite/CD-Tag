using System.Windows;
using CDTag.ViewModel.Tag;
using CDTag.Views;
using CDTag.Views.Tag;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace CDTag
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterInstance(typeof(ITagViewModel), Container.Resolve<TagViewModel>());

            Unity.Container = Container;
        }

        protected override DependencyObject CreateShell()
        {
            MainWindow shell = Container.Resolve<MainWindow>();
            shell.Show();

            return shell;
        }

        protected override void InitializeModules()
        {
            //MainModule module = Container.Resolve<MainModule>();
            //module.Initialize();
        }
    }

    public static class Unity
    {
        public static IUnityContainer Container { get; set; }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }

    // TODO: Take out if the decision is made to not use regions
    /*public class MainModule : IModule
    {
        public MainModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }

        public void Initialize()
        {
            var tagView = Container.Resolve<TagView>();
            RegionManager.Regions["TagRegion"].Add(tagView);

            var tagToolbar = Container.Resolve<TagToolbar>();
            RegionManager.Regions["ToolbarRegion"].Add(tagToolbar);

            var tagMenu = Container.Resolve<TagMenu>();
            RegionManager.Regions["Menu"].Add(tagMenu);
        }

        public IUnityContainer Container { get; private set; }
        public IRegionManager RegionManager { get; private set; }
    }*/
}
