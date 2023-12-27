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

            Mediator.EventAggregator.GetEvent<DragMoveWindowEvent>().Subscribe(OnDragMoveWindowEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<WindowShowEvent>().Subscribe(OnWindowShowEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<WindowHideEvent>().Subscribe(OnWindowHideEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<WindowCloseEvent>().Subscribe(OnWindowCloseEvent, ThreadOption.UIThread);
        }

        private void OnWindowCloseEvent()
        {
            this.Close();
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
            }
            catch (Exception)
            {
            }

            base.OnActivated(e);
        }

        private void OnWindowShowEvent()
        {
            this.Show();
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