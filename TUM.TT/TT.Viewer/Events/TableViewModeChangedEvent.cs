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
        public TableViewModeChangedEvent(TableViewModel.ViewMode mode)
        {
            Mode = mode;
        }

        public TableViewModel.ViewMode Mode { get; set; }
    }
}
