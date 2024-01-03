using Common.Events;
using Prism.Events;
using System;
using System.Windows;

namespace TImer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;

            Mediator.EventAggregator.GetEvent<DragMoveWindowEvent>().Subscribe(OnDragMoveWindowEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<WindowShowEvent>().Subscribe(OnWindowShowEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<WindowHideEvent>().Subscribe(OnWindowHideEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<WindowCloseEvent>().Subscribe(OnWindowCloseEvent, ThreadOption.UIThread);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void OnWindowCloseEvent()
        {
            Environment.Exit(0);
        }

        private void OnWindowHideEvent()
        {
            this.Hide();
        }

        protected override void OnActivated(EventArgs e)
        {
            try
            {
                this.Visibility = Visibility.Visible;
                this.Activate();
                this.Show();
            }
            catch (Exception)
            {
            }

            base.OnActivated(e);
        }

        private void OnWindowShowEvent()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = SystemParameters.PrimaryScreenWidth - this.ActualWidth;
            this.Top = SystemParameters.PrimaryScreenHeight - this.ActualHeight;
        }

        private void OnDragMoveWindowEvent()
        {
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {
            }
        }
    }
}