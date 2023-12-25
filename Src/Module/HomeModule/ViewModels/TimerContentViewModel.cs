using Common;
using Common.Events;
using Common.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Automation.Provider;

namespace HomeModule.ViewModels
{
    public class TimerContentViewModel : BindableBase
    {
        private string _hour = "00";

        public string hour
        {
            get { return _hour; }
            set
            {
                SetProperty(ref _hour, value);
            }
        }

        private string _minute = "00";

        public string minute
        {
            get { return _minute; }
            set
            {
                SetProperty(ref _minute, value);
            }
        }

        private string _second = "00";

        public string second
        {
            get { return _second; }
            set
            {
                SetProperty(ref _second, value);
            }
        }

        private bool _IsWorking = false;

        public bool IsWorking
        {
            get { return _IsWorking; }
            set
            {
                SetProperty(ref _IsWorking, value);
            }
        }

        private readonly ITimerRepository timerRepository;
        private Timer timer = null;

        public TimerContentViewModel(IContainerProvider containerProvider)
        {
            timerRepository = containerProvider.Resolve<ITimerRepository>();

            Mediator.EventAggregator.GetEvent<TimerInitEvent>().Subscribe(OnTimerInitEvent);
            Mediator.EventAggregator.GetEvent<UpdateTimerEvent>().Subscribe(OnUpdateTimerEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<UpdateIsWorking>().Subscribe(OnUpdateIsWorking, ThreadOption.UIThread);
        }

        private void OnTimerInitEvent()
        {
            InitTimer();
        }

        private void InitTimer()
        {
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            var timerInfo = timerRepository.GetTimer();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timerInfo = timerRepository.GetTimer();
            if (DateTime.Now >= DateTime.Parse(timerInfo.WorkingHour) && DateTime.Now < DateTime.Parse(timerInfo.RushHour))
            {
                TimeSpan dateTime = DateTime.Parse(timerInfo.RushHour) - DateTime.Now;
                int totalSeconds = (int)dateTime.TotalSeconds;

                if (totalSeconds > 0)
                {
                    Mediator.EventAggregator.GetEvent<UpdateIsWorking>().Publish(true);
                    Mediator.EventAggregator.GetEvent<UpdateTimerEvent>().Publish(Tuple.Create(
                             String.Format("{0:00}", dateTime.Hours),
                             String.Format("{0:00}", dateTime.Minutes),
                             String.Format("{0:00}", dateTime.Seconds)));
                }
                else if (totalSeconds == 0)
                {
                    Mediator.EventAggregator.GetEvent<UpdateIsWorking>().Publish(false);

                    CloseComputer();
                    Mediator.EventAggregator.GetEvent<UpdateTimerEvent>().Publish(Tuple.Create(
                           String.Format("{0:00}", dateTime.Hours),
                           String.Format("{0:00}", dateTime.Minutes),
                           String.Format("{0:00}", dateTime.Seconds)));
                }
            }
            else
            {
                Mediator.EventAggregator.GetEvent<UpdateIsWorking>().Publish(false);
            }
        }

        private void CloseComputer()
        {
            Log.Info($"{nameof(CloseComputer)} Strat");

            var timerInfo = timerRepository.GetTimer();
            if (timerInfo.IsShutDownComputer)
            {
#if !DEBUG
                Process.Start(new ProcessStartInfo("shutdown.exe", "/s /t 10")
                {
                    UseShellExecute = false,
                    Verb = "runas",
                    CreateNoWindow = true,
                });

#endif
                Mediator.EventAggregator.GetEvent<ShutDownComputerEvent>().Publish();
            }

            Mediator.EventAggregator.GetEvent<WindowShow>().Publish();
            Mediator.EventAggregator.GetEvent<WindowTip>().Publish();

            Log.Info($"{nameof(CloseComputer)} End");
        }

        private void OnUpdateIsWorking(bool obj)
        {
            IsWorking = obj;
        }

        private void OnUpdateTimerEvent(Tuple<string, string, string> tuple)
        {
            hour = tuple.Item1;
            minute = tuple.Item2;
            second = tuple.Item3;
        }
    }
}