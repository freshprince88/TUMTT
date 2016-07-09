
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;

namespace TT.Lib.Events
{
    public class PlayModeEvent
    {
        public bool? PlayMode { get; set; }

        public PlayModeEvent(bool? pm)
        {
            PlayMode = pm;
        }

    }
}