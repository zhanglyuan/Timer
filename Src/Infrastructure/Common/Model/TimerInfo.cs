using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class TimerInfo
    {
        public string WorkingHour { get; set; }
        public string RushHour { get; set; }

        public bool IsShutDownComputer { get; set; }
    }
}