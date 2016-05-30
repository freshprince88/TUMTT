using System.Collections;
using System.Collections.Generic;
namespace TT.Lib.Events
{
    public class DeleteLinesEvent
    {
        public List<System.Windows.Shapes.Line> Lines;

        public DeleteLinesEvent(List<System.Windows.Shapes.Line> linesToDelete)
        {
            Lines = linesToDelete;
        }

    }
}