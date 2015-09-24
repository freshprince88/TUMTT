using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Events
{
    public class TableStdViewSelectionChangedEvent
    {

        public TableStdViewSelectionChangedEvent(HashSet<TableStandardViewModel.ETablePosition> positions, HashSet<TableStandardViewModel.EStrokeLength> strokeLengths)
        {
            Positions = positions;
            StrokeLengths = strokeLengths;
        }

        public HashSet<TableStandardViewModel.ETablePosition> Positions { get; set; }
        public HashSet<TableStandardViewModel.EStrokeLength> StrokeLengths { get; set; }
    }
}
