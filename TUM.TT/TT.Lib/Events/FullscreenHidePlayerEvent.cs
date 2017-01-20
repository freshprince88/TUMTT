using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{

    public class FullscreenHidePlayerEvent
    {
        public bool Hide { get; set; }
        public FullscreenHidePlayerEvent(bool hideShow)
        {
            Hide = hideShow;
        }
    }
}
