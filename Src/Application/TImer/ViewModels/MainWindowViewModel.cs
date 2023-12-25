using Common.Events;
using CommonUIBase.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace TImer.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
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

        public MainWindowViewModel()
        {
            DragMoveWindowCommand = new DelegateCommand(DragMoveWindowExecute);
            LoadedCommand = new DelegateCommand(LoadedExecute);
            UnLoadedCommand = new DelegateCommand(UnLoadedExecute);

            Mediator.EventAggregator.GetEvent<ShutDownComputerEvent>().Subscribe(OnShutDownComputerEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<UpdateTimerEvent>().Subscribe(OnUpdateTimerEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<UpdateIsWorking>().Subscribe(OnUpdateIsWorking, ThreadOption.UIThread);
        }

        private void OnUpdateIsWorking(bool obj)
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
                Mediator.EventAggregator.GetEvent<WindowShow>().Publish();
            };

            var contextMenu = new System.Windows.Controls.ContextMenu();

            var menuItem = new System.Windows.Controls.MenuItem()
            {
                Header = "最小化"
            };
            menuItem.Click += (s, e) =>
            {
                Mediator.EventAggregator.GetEvent<WindowHide>().Publish();
            };
            contextMenu.Items.Add(menuItem);

            var menuItem2 = new System.Windows.Controls.MenuItem()
            {
                Header = "退出程序"
            };
            menuItem2.Click += (s, e) =>
            {
                Mediator.EventAggregator.GetEvent<WindowClose>().Publish();
            };
            contextMenu.Items.Add(menuItem2);

            notifyIcon.ContextMenu = contextMenu;
        }
    }
}