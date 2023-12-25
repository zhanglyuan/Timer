using Common;
using HomeModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows.Controls;

namespace HomeModule
{
    public class HomeModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionName.ContentRegion, ViewName.TimerContent);
            regionManager.RegisterViewWithRegion(RegionName.TopRegion, ViewName.TimerTool);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TimerTool>();
            containerRegistry.RegisterForNavigation<TimerContent>();
        }
    }
}