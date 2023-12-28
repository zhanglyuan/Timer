using Common;
using Common.Constants;
using Common.Events;
using Common.Interfaces;
using CommonUIBase.Controls;
using CommonUIBase.Controls.NotifyIcons.Runtimes;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace TImer.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IVersionUpdate versionUpdate;
        private readonly IRegionManager regionManager;
        private NotifyIcon notifyIcon;

        private bool _isShutDownComputerDialog;

        public bool IsShutDownComputerDialog
        {
            get { return _isShutDownComputerDialog; }
            set
            {
                SetProperty(ref _isShutDownComputerDialog, value);
            }
        }

        public DelegateCommand DragMoveWindowCommand { get; private set; }
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand UnLoadedCommand { get; private set; }

        public MainWindowViewModel(IContainerProvider containerProvider)
        {
            versionUpdate = containerProvider.Resolve<IVersionUpdate>();
            regionManager = containerProvider.Resolve<IRegionManager>();

            DragMoveWindowCommand = new DelegateCommand(DragMoveWindowExecute);
            LoadedCommand = new DelegateCommand(LoadedExecute);
            UnLoadedCommand = new DelegateCommand(UnLoadedExecute);

            Mediator.EventAggregator.GetEvent<ShutDownComputerEvent>().Subscribe(OnShutDownComputerEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<UpdateTimerEvent>().Subscribe(OnUpdateTimerEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<UpdateIsWorkingEvent>().Subscribe(OnUpdateIsWorkingEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<WindowTipEvent>().Subscribe(OnWindowTipEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<UpdateAppStartEvent>().Subscribe(OnUpdateAppStartEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<UpdateAppEndEvent>().Subscribe(OnUpdateAppEndEvent, ThreadOption.UIThread);
        }

        private void OnUpdateAppEndEvent(bool isSuccess)
        {
            var region = regionManager.Regions[RegionName.BottomRegion]; // 使用您的区域名称

            var viewToRemove = region.Views.FirstOrDefault(v => v.GetType().Name == ViewName.UpdateProgressBar);
            if (viewToRemove != null)
            {
                region.Remove(viewToRemove);
            }

            if (isSuccess)
            {
                Process.Start(new ProcessStartInfo()
                {
                    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalSettings.UpdateAppExeName),
                    Arguments = " /VERYSILENT " + $"\"{Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}\""
                });

                Mediator.EventAggregator.GetEvent<WindowCloseEvent>().Publish();
            }
        }

        private void OnUpdateAppStartEvent()
        {
            regionManager.RequestNavigate(RegionName.BottomRegion, ViewName.UpdateProgressBar);
        }

        private void OnWindowTipEvent()
        {
            Log.Info($"{nameof(OnWindowTipEvent)} Start");
            notifyIcon.ShowBalloonTips("下班倒计时", "下班啦！", NotifyIconInfoType.None);
            Log.Info($"{nameof(OnWindowTipEvent)} End");
        }

        private void OnUpdateIsWorkingEvent(bool obj)
        {
            if (!obj)
            {
                notifyIcon.Title = "下班倒计时";
            }
        }

        private void OnUpdateTimerEvent(Tuple<string, string, string> tuple)
        {
            notifyIcon.Title = $"{tuple.Item1}:{tuple.Item2}:{tuple.Item3}";
        }

        private void UnLoadedExecute()
        {
            notifyIcon?.Dispose();
        }

        private void LoadedExecute()
        {
            InitializeNotifyIcon();
        }

        private void OnShutDownComputerEvent()
        {
            IsShutDownComputerDialog = true;
        }

        private void DragMoveWindowExecute()
        {
            Mediator.EventAggregator.GetEvent<DragMoveWindowEvent>().Publish();
        }

        private void InitializeNotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Title = "下班倒计时";
            notifyIcon.Click += (s, e) =>
            {
                Mediator.EventAggregator.GetEvent<WindowShowEvent>().Publish();
            };

            var contextMenu = new System.Windows.Controls.ContextMenu();

            var version = new System.Windows.Controls.MenuItem()
            {
                Header = "版本:" + GlobalSettings.Version
            };
            contextMenu.Items.Add(version);

            var windowMin = new System.Windows.Controls.MenuItem()
            {
                Header = "最小化"
            };
            windowMin.Click += (s, e) =>
            {
                Mediator.EventAggregator.GetEvent<WindowHideEvent>().Publish();
            };
            contextMenu.Items.Add(windowMin);

            var shutDown = new System.Windows.Controls.MenuItem()
            {
                Header = "退出程序"
            };
            shutDown.Click += (s, e) =>
            {
                Mediator.EventAggregator.GetEvent<WindowCloseEvent>().Publish();
            };
            contextMenu.Items.Add(shutDown);

            notifyIcon.ContextMenu = contextMenu;
        }
    }
}