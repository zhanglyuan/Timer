using Common;
using Common.Interfaces;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using TImer.Views;

namespace TImer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static Mutex mutex;

        protected override Window CreateShell()
        {
            mutex = new Mutex(true, Process.GetCurrentProcess().ProcessName, out bool createdNew);

            if (!createdNew)
            {
                ProcessFunctions.ActivateWindow(Assembly.GetExecutingAssembly().GetName().Name);
                Environment.Exit(0);
            }

#if DEBUG
            Log.Init(Process.GetCurrentProcess().ProcessName);
#endif

            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ITimerRepository, TimerRepository>();
            containerRegistry.RegisterSingleton<ILocalDB, LocalDB>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<HomeModule.HomeModule>();

            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}