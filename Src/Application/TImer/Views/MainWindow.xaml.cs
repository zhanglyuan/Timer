using Common.Events;
using DryIoc;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TImer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard CloseStoryboard = new Storyboard();
        private Storyboard ShowStoryboard = new Storyboard();
        private bool IsClose;
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
            IsClose = false;
            CloseStoryboard?.Begin();
            e.Cancel = true;
        }

        private void OnWindowCloseEvent()
        {
            IsClose = true;
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
            CloseStoryboard?.Begin();
        }

        private void OnWindowHideEvent()
        {
            IsClose = false;
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
            CloseStoryboard?.Begin();
        }

        private void OnWindowShowEvent()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
            ShowStoryboard?.Begin();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = SystemParameters.WorkArea.Width - this.ActualWidth;
            this.Top = SystemParameters.WorkArea.Height - this.ActualHeight;

            var time = new Duration(TimeSpan.FromSeconds(0.8));

            var yAnimation = new DoubleAnimation(0, this.ActualWidth, time, FillBehavior.Stop);
            var xAnimation = new DoubleAnimation(this.ActualWidth, 0, time, FillBehavior.Stop);

            Storyboard.SetTarget(yAnimation, this);
            Storyboard.SetTarget(xAnimation, this);

            Storyboard.SetTargetProperty(yAnimation, new PropertyPath("RenderTransform.X"));
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath("RenderTransform.X"));

            CloseStoryboard.Children.Add(yAnimation);
            ShowStoryboard.Children.Add(xAnimation);

            CloseStoryboard.Completed += (s, e) =>
            {
                if (!IsClose)
                    this.Hide();
                else
                    Environment.Exit(0);
            };

            ShowStoryboard?.Begin();
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