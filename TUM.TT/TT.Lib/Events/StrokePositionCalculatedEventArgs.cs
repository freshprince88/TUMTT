using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TT.Lib.Events
{
    public class StrokePositionCalculatedEventArgs : EventArgs
    {
        public Point Position { get; set; }

        public StrokePositionCalculatedEventArgs(Point position)
        {
            Position = position;
        }
    }
}
