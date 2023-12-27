using Common.Interfaces;
using Common.Model;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class TimerRepository : ITimerRepository
    {
        private object _locker = new object();
        private readonly ILocalDB _db;

        public TimerRepository(IContainerProvider containerProvider)
        {
            _db = containerProvider.Resolve<ILocalDB>();
        }

        public TimerInfo GetTimer()
        {
            lock (_locker)
            {
                return _db.GetTimer();
            }
        }

        public bool SaveTimer(TimerInfo timer)
        {
            lock (_locker)
            {
                return _db.SaveTimer(timer);
            }
        }
    }
}