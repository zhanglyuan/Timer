using Common;
using Common.Events;
using Common.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace HomeModule.ViewModels
{
    public class TimerToolViewModel : BindableBase
    {
        private readonly ITimerRepository timerRepository;

        private string _workingHour = string.Empty;

        public string workingHour
        {
            get { return _workingHour; }
            set
            {
                SetProperty(ref _workingHour, value);
            }
        }

        private string _rushHour = string.Empty;

        public string rushHour
        {
            get { return _rushHour; }
            set
            {
                SetProperty(ref _rushHour, value);
            }
        }

        private bool _isShutDownComputer = false;

        public bool IsShutDownComputer
        {
            get { return _isShutDownComputer; }
            set
            {
                var timer = timerRepository.GetTimer();
                if (timer != null && timer.IsShutDownComputer != value)
                {
                    timer.IsShutDownComputer = value;
                    timerRepository.SaveTimer(timer);
                }
                SetProperty(ref _isShutDownComputer, value);
            }
        }

        public DelegateCommand SaveCommand { get; private set; }

        public TimerToolViewModel(IContainerProvider containerProvider)
        {
            timerRepository = containerProvider.Resolve<ITimerRepository>();

            SaveCommand = new DelegateCommand(SaveExecute);
            Init();
        }

        private void SaveExecute()
        {
            TimeSpan dateTime = DateTime.Parse(rushHour) - DateTime.Parse(workingHour);
            if (dateTime.TotalSeconds <= 0)
            {
                DialogHost.OpenDialogCommand.Execute(null, null);
                return;
            }

            var timer = timerRepository.GetTimer();
            if (timer != null)
            {
                timer.RushHour = rushHour;
                timer.WorkingHour = workingHour;
                timerRepository.SaveTimer(timer);
            }
        }

        private void Init()
        {
            var timer = timerRepository.GetTimer();
            if (timer == null)
            {
                timer = new Common.Model.TimerInfo();
                timerRepository.SaveTimer(timer);
            }
            workingHour = timer.WorkingHour;
            rushHour = timer.RushHour;
            IsShutDownComputer = timer.IsShutDownComputer;

            Mediator.EventAggregator.GetEvent<TimerInitEvent>().Publish();
        }
    }
}