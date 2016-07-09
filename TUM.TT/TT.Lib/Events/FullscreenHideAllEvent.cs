using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{

    public class FullscreenHideAllEvent {
        public bool Hide { get; set; }
        public FullscreenHideAllEvent(bool hideShow)
        {
            Hide = hideShow;
        }
    }
}
