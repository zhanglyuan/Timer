using Common.Events;
using CommonUIBase.Controls;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Input;

namespace TImer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Unloaded += MainWindow_Unloaded;

            Mediator.EventAggregator.GetEvent<DragMoveWindowEvent>().Subscribe(OnDragMoveWindowEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<WindowAcActivateEvent>().Subscribe(OnWindowAcActivateEvent, ThreadOption.UIThread);
        }

        protected override void OnActivated(EventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
            }

            base.OnActivated(e);
        }

        private void OnWindowAcActivateEvent()
        {
            this.Show();
            this.Activate();
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            notifyIcon?.Dispose();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = SystemParameters.PrimaryScreenWidth - this.ActualWidth;
            this.Top = SystemParameters.PrimaryScreenHeight - this.ActualHeight;

            InitializeNotifyIcon();
        }

        private void InitializeNotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Title = "下班倒计时";
            notifyIcon.Click += (s, e) =>
            {
                this.Show();
                this.Activate();
            };

            var contextMenu = new System.Windows.Controls.ContextMenu();

            var menuItem = new System.Windows.Controls.MenuItem()
            {
                Header = "最小化"
            };
            menuItem.Click += (s, e) =>
            {
                this.Hide();
            };
            contextMenu.Items.Add(menuItem);

            var menuItem2 = new System.Windows.Controls.MenuItem()
            {
                Header = "退出程序"
            };
            menuItem2.Click += (s, e) =>
            {
                this.Close();
            };
            contextMenu.Items.Add(menuItem2);

            notifyIcon.ContextMenu = contextMenu;
        }

        private void OnDragMoveWindowEvent()
        {
            try
            {
                this.Focus();
                this.DragMove();
            }
            catch (Exception)
            {
            }
        }
    }
}