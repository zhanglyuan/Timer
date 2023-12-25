using Common.Interfaces;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class TimerRepository : ITimerRepository
    {
        private TimerInfo _timerInfo = new TimerInfo();
        private object _locker = new object();

        public TimerInfo GetTimer()
        {
            lock (_locker)
            {
                return _timerInfo;
            }
        }

        public void SaveTimer(string workingHour, string rushHour, bool? isShutDownComputer)
        {
            lock (_locker)
            {
                if (!string.IsNullOrEmpty(workingHour))
                    _timerInfo.WorkingHour = workingHour;

                if (!string.IsNullOrEmpty(rushHour))
                    _timerInfo.RushHour = rushHour;

                if (isShutDownComputer != null)
                    _timerInfo.IsShutDownComputer = (bool)isShutDownComputer;
            }
        }
    }
}