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
            Mediator.EventAggregator.GetEvent<WindowShow>().Subscribe(OnWindowShow, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<WindowHide>().Subscribe(OnWindowHide, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<WindowClose>().Subscribe(OnWindowClose, ThreadOption.UIThread);
        }

        private void OnWindowClose()
        {
            this.Close();
        }

        private void OnWindowHide()
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

        private void OnWindowShow()
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