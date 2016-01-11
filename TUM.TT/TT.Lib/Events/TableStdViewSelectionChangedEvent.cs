using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Util.Enums;

namespace TT.Lib.Events
{
    public class TableStdViewSelectionChangedEvent
    {

        public TableStdViewSelectionChangedEvent(HashSet<Positions.Table> positions, HashSet<Positions.Length> strokeLengths)
        {
            Positions = positions;
            StrokeLengths = strokeLengths;
        }

        public HashSet<Positions.Table> Positions { get; set; }
        public HashSet<Positions.Length> StrokeLengths { get; set; }
    }
}
