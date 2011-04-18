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
        protected override DependencyObject CreateShell()
        {
            MainWindow shell = Container.Resolve<MainWindow>();
            shell.Show();

            return shell;
        }

        protected override void InitializeModules()
        {
            Container.RegisterType<ITagToolbarViewModel, TagToolbarViewModel>();
            Container.RegisterType<ITagViewModel, TagViewModel>();
            
            IModule x = Container.Resolve<ModuleA>();
            x.Initialize();
        }
    }

    public class ModuleA : IModule
    {
        public ModuleA(IUnityContainer container, IRegionManager regionManager)
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
        }

        public IUnityContainer Container { get; private set; }
        public IRegionManager RegionManager { get; private set; }
    }

}
