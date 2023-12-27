using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ILocalDB
    {
        bool SaveTimer(TimerInfo timer);

        TimerInfo GetTimer();
    }
}