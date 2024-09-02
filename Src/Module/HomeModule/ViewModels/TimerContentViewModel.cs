using Common;
using Common.Constants;
using Common.Events;
using Common.Interfaces;
using Common.Model;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
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

        private string _city = "当前天气：";

        public string City
        {
            get { return _city; }
            set
            {
                SetProperty(ref _city, value);
            }
        }

        private string _weatherTxt = string.Empty;

        public string WeatherTxt
        {
            get { return _weatherTxt; }
            set
            {
                SetProperty(ref _weatherTxt, value);
            }
        }

        private string _temperature = string.Empty;

        public string Temperature
        {
            get { return _temperature; }
            set
            {
                SetProperty(ref _temperature, value);
            }
        }

        private string _weatherToolTip = string.Empty;

        public string WeatherToolTip
        {
            get { return _weatherToolTip; }
            set
            {
                SetProperty(ref _weatherToolTip, value);
            }
        }

        private string _svgUrl = GlobalSettings.DefaultSvgUrl;

        public string SvgUrl
        {
            get { return _svgUrl; }
            set
            {
                SetProperty(ref _svgUrl, value);
            }
        }

        private readonly ITimerRepository timerRepository;
        private readonly IWeather weather;
        private Timer timer = null;

        public TimerContentViewModel(IContainerProvider containerProvider)
        {
            timerRepository = containerProvider.Resolve<ITimerRepository>();
            weather = containerProvider.Resolve<IWeather>();

            Mediator.EventAggregator.GetEvent<TimerInitEvent>().Subscribe(OnTimerInitEvent);
            Mediator.EventAggregator.GetEvent<UpdateTimerEvent>().Subscribe(OnUpdateTimerEvent, ThreadOption.UIThread);
            Mediator.EventAggregator.GetEvent<UpdateIsWorkingEvent>().Subscribe(OnUpdateIsWorkingEvent, ThreadOption.UIThread);

            Mediator.EventAggregator.GetEvent<UpdateWeatherEvent>().Subscribe(OnUpdateWeatherEvent, ThreadOption.UIThread);

            _ = weather.GetLocationWeather();
        }

        private void OnUpdateWeatherEvent(Tuple<IPLocation, WeatherInfo> weatherinfo)
        {
            if (weatherinfo != null && weatherinfo.Item1 != null && weatherinfo.Item2 != null)
            {
                City = weatherinfo?.Item1?.City + "当前天气：";
                WeatherTxt = weatherinfo?.Item2?.NowWeather.Text;
                Temperature = "温度" + weatherinfo?.Item2?.NowWeather.Temp + "℃";
                WeatherToolTip = WeatherTxt + " " + Temperature;

                SvgUrl = !string.IsNullOrEmpty(weatherinfo?.Item2?.NowWeather.Icon) ? string.Format(GlobalSettings.NowSvgUrl, weatherinfo?.Item2?.NowWeather.Icon) : GlobalSettings.DefaultSvgUrl;
            }
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
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timerInfo = timerRepository.GetTimer();
            if (!DateTime.TryParse(timerInfo?.WorkingHour, out DateTime workingHour) ||
                !DateTime.TryParse(timerInfo?.RushHour, out DateTime rushHour))
                return;

            if (DateTime.Now >= workingHour && DateTime.Now < rushHour)
            {
                TimeSpan dateTime = DateTime.Parse(timerInfo.RushHour) - DateTime.Now;
                int totalSeconds = (int)dateTime.TotalSeconds;

                if (totalSeconds > 0)
                {
                    Mediator.EventAggregator.GetEvent<UpdateIsWorkingEvent>().Publish(true);
                    Mediator.EventAggregator.GetEvent<UpdateTimerEvent>().Publish(Tuple.Create(
                             String.Format("{0:00}", dateTime.Hours),
                             String.Format("{0:00}", dateTime.Minutes),
                             String.Format("{0:00}", dateTime.Seconds)));
                }
                else if (totalSeconds == 0)
                {
                    Mediator.EventAggregator.GetEvent<UpdateIsWorkingEvent>().Publish(false);

                    CloseComputer();
                    Mediator.EventAggregator.GetEvent<UpdateTimerEvent>().Publish(Tuple.Create(
                           String.Format("{0:00}", dateTime.Hours),
                           String.Format("{0:00}", dateTime.Minutes),
                           String.Format("{0:00}", dateTime.Seconds)));
                }
            }
            else
            {
                Mediator.EventAggregator.GetEvent<UpdateIsWorkingEvent>().Publish(false);
            }
        }

        private void CloseComputer()
        {
            Log.Info($"{nameof(CloseComputer)} Strat");

            var timerInfo = timerRepository.GetTimer();
            if (timerInfo != null && timerInfo.IsShutDownComputer)
            {
//#if !DEBUG
//                Process.Start(new ProcessStartInfo("shutdown.exe", "/s /t 10")
//                {
//                    UseShellExecute = false,
//                    Verb = "runas",
//                    CreateNoWindow = true,
//                });

//#endif
                Mediator.EventAggregator.GetEvent<ShutDownComputerEvent>().Publish();
            }

            Mediator.EventAggregator.GetEvent<WindowShowEvent>().Publish();
            Mediator.EventAggregator.GetEvent<WindowTipEvent>().Publish();

            Log.Info($"{nameof(CloseComputer)} End");
        }

        private void OnUpdateIsWorkingEvent(bool obj)
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