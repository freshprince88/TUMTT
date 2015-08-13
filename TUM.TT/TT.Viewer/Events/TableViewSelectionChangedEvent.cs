using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Events
{
    public class TableViewSelectionChangedEvent
    {
        public TableViewSelectionChangedEvent(HashSet<TableServiceViewModel.ETablePosition> positions, HashSet<TableServiceViewModel.EServerPosition> playerPos)
        {
            Positions = positions;
            PlayerPositions = playerPos;
        }

        public HashSet<TableServiceViewModel.ETablePosition> Positions { get; set; }
        public HashSet<TableServiceViewModel.EServerPosition> PlayerPositions { get; set; }
    }
}
