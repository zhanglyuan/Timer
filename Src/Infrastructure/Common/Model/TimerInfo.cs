using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class TimerInfo
    {
        [PrimaryKey]
        public string ID { get; set; } = Guid.NewGuid().ToString("N");

        public string WorkingHour { get; set; } = "09:00";
        public string RushHour { get; set; } = "18:00";

        public bool IsShutDownComputer { get; set; } = false;
    }
}