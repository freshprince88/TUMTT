﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{

    public class FullscreenHideHitlistEvent {
        public bool Hide { get; set; }
        public FullscreenHideHitlistEvent(bool hideShow)
        {
            Hide = hideShow;
        }
    }
}
