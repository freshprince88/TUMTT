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
        public TableViewSelectionChangedEvent(List<TableServiceViewModel.TablePosition> positions, List<TableServiceViewModel.StrokeLength> length, int playerPos)
        {
            Positions = positions;
            Length = length;
            PlayerPosition = playerPos;
        }

        public List<TableServiceViewModel.TablePosition> Positions { get; set; }
        public List<TableServiceViewModel.StrokeLength> Length { get; set; }
        public int PlayerPosition { get; set; }
    }
}
