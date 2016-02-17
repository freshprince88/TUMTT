using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Util.Enums;

namespace TT.Lib.Events
{
    public class FullscreenEvent
    {
        public bool Fullscreen { get; set; }

        public FullscreenEvent(bool onOff)
        {
            Fullscreen = onOff;
        }

    }
}
