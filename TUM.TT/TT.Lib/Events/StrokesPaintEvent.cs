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
        private int strokeNumber;
        private List<Stroke> strokes;
        
        public StrokesPaintEvent(List<Stroke> strokes, int strokeNumber)
        {
            Strokes = strokes;
            StrokeNumber = strokeNumber;
        }

        public int StrokeNumber { get; private set; }
        public List<Stroke> Strokes { get; private set;
        }
    }
}
