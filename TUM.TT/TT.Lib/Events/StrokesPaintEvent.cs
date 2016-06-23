using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;

namespace TT.Lib.Events
{
    public class StrokesPaintEvent
    {
        public StrokesPaintEvent(List<Stroke> strokes)
        {
            Strokes = strokes;
        }

        public List<Stroke> Strokes {
            get; private set;
        }
    }
}
