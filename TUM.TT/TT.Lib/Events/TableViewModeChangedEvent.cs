using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Util.Enums;

namespace TT.Lib.Events
{
    public class TableViewModeChangedEvent
    {
        public TableViewModeChangedEvent(ViewMode.Position mode)
        {
            Mode = mode;
        }

        public ViewMode.Position Mode { get; set; }
    }
}
