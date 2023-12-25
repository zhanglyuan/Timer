using Common.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace TImer.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
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

        public MainWindowViewModel()
        {
            DragMoveWindowCommand = new DelegateCommand(DragMoveWindowExecute);

            Mediator.EventAggregator.GetEvent<ShutDownComputerEvent>().Subscribe(OnShutDownComputerEvent, ThreadOption.UIThread);
        }

        private void OnShutDownComputerEvent()
        {
            IsShutDownComputerDialog = true;
        }

        private void DragMoveWindowExecute()
        {
            Mediator.EventAggregator.GetEvent<DragMoveWindowEvent>().Publish();
        }
    }
}