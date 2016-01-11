using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Util.Enums;

namespace TT.Lib.Events
{
    public class TableViewSelectionChangedEvent
    {
        public TableViewSelectionChangedEvent(HashSet<Positions.Table> positions, HashSet<Positions.Server> playerPos)
        {
            Positions = positions;
            PlayerPositions = playerPos;
        }

        public HashSet<Positions.Table> Positions { get; set; }
        public HashSet<Positions.Server> PlayerPositions { get; set; }
    }
}
