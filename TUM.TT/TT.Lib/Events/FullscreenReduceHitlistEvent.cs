using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;

namespace TT.Lib.Events
{
    public class FullscreenReduceHitlistEvent
    {
        public bool ReduceHitlist { get; set; }

        public FullscreenReduceHitlistEvent(bool onOff)
        {
            ReduceHitlist = onOff;
        }

    }
}
