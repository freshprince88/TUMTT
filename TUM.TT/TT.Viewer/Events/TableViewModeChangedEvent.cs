using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Events
{
    public class TableViewModeChangedEvent
    {
        public TableViewModeChangedEvent(TableServiceViewModel.ViewMode mode)
        {
            Mode = mode;
        }

        public TableServiceViewModel.ViewMode Mode { get; set; }
    }
}
