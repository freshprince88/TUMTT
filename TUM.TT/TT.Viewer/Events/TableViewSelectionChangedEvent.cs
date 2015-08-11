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
        public TableViewSelectionChangedEvent(List<TableViewModel.TablePosition> positions, List<TableViewModel.StrokeLength> length, int playerPos)
        {
            Positions = positions;
            Length = length;
            PlayerPosition = playerPos;
        }

        public List<TableViewModel.TablePosition> Positions { get; set; }
        public List<TableViewModel.StrokeLength> Length { get; set; }
        public int PlayerPosition { get; set; }
    }
}
