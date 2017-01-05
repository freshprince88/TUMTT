using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TT.Lib.Events
{
    public class PointAddedEventArgs : EventArgs
    {
        public int NumberOfPoints { get; set; }
        public Point LastPoint { get; set; }

        public PointAddedEventArgs(int nrOfPoints, Point lastPoint)
        {
            NumberOfPoints = nrOfPoints;
            LastPoint = lastPoint;
        }
    }
}
