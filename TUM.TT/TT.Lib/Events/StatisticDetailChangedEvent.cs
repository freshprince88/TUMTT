using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{
    public class StatisticDetailChangedEvent
    {
        public bool DetailChecked { get; set; }
        public bool Percent { get; set; }

        public StatisticDetailChangedEvent()
        {
            DetailChecked = false;
            Percent = false;
        }

        public StatisticDetailChangedEvent(bool detail, bool percent)
        {
            DetailChecked = detail;
            Percent = percent;
        }
    }
}
