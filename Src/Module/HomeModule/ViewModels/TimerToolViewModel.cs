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
        private string basePath = System.IO.Path.Combine(Path.GetTempPath(), "Timer");

        private string defaultWorkingHour = "9:00";
        private string defaultRushHour = "18:00";

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
                timerRepository?.SaveTimer(string.Empty, string.Empty, value);
                SetProperty(ref _isShutDownComputer, value);
            }
        }

        public DelegateCommand SaveCommand { get; private set; }

        public TimerToolViewModel(IContainerProvider containerProvider)
        {
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            timerRepository = containerProvider.Resolve<ITimerRepository>();

            SaveCommand = new DelegateCommand(SaveExecute);
            _ = InitAsync();
        }

        private async void SaveExecute()
        {
            TimeSpan dateTime = DateTime.Parse(rushHour) - DateTime.Parse(workingHour);
            if (dateTime.TotalSeconds <= 0)
            {
                DialogHost.OpenDialogCommand.Execute(null, null);
                return;
            }

            await SaveTime(nameof(workingHour), workingHour);
            await SaveTime(nameof(rushHour), rushHour);

            timerRepository.SaveTimer(workingHour, rushHour, null);
        }

        private async Task InitAsync()
        {
            var tuple1 = await InitTime(nameof(workingHour), defaultWorkingHour);
            var tuple2 = await InitTime(nameof(rushHour), defaultRushHour);

            TimeSpan dateTime = DateTime.Parse(tuple2.Item2) - DateTime.Parse(tuple1.Item2);

            if (dateTime.TotalSeconds <= 0)
            {
                workingHour = defaultWorkingHour;
                rushHour = defaultRushHour;
                await SaveTime(nameof(workingHour), workingHour);
                await SaveTime(nameof(rushHour), rushHour);
            }
            else
            {
                workingHour = tuple1.Item2;
                rushHour = tuple2.Item2;
            }

            timerRepository.SaveTimer(workingHour, rushHour, false);
            Mediator.EventAggregator.GetEvent<TimerInitEvent>().Publish();
        }

        private async Task<Tuple<bool, string>> InitTime(string strPath, string defaultTime)
        {
            string path = Path.Combine(basePath, strPath + ".txt");
            try
            {
                if (File.Exists(path))
                {
                    string strWorkingHour = await File.ReadAllTextAsync(path);
                    if (DateTime.TryParse(strWorkingHour, out DateTime result))
                    {
                        return Tuple.Create(true, result.ToString("HH:mm"));
                    }
                }

                await File.WriteAllTextAsync(path, defaultTime);
            }
            catch (Exception)
            {
            }
            return Tuple.Create(false, defaultTime);
        }

        private async Task SaveTime(string strPath, string time)
        {
            string path = Path.Combine(basePath, strPath + ".txt");
            try
            {
                await File.WriteAllTextAsync(path, time);
            }
            catch (Exception)
            {
            }
        }
    }
}