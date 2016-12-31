﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{
    public class ReportSettingsChangedEvent
    {
        public bool MatchOpened { get; private set; }

        public ReportSettingsChangedEvent(bool matchNotOpenedYet)
        {
            this.MatchOpened = matchNotOpenedYet;
        }
    }
}
