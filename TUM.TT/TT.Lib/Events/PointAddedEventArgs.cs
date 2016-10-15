using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{
    public class PointAddedEventArgs : EventArgs
    {
        public int numberOfPoints { get; set; }

        public PointAddedEventArgs(int nrOfPoints)
        {
            numberOfPoints = nrOfPoints;
        }
    }
}
