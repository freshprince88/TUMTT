using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.Events
{
    public class VideoPlayEvent
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Previous { get; set; }
        public int Next { get; set; }

        public VideoPlayEvent()
        {
            Start = 0;
            End = 0;
            Previous = 0;
            Next = 0;
        }

        public VideoPlayEvent(int start, int end, int previous = 0, int next = 0)
        {
            Start = start;
            End = end;
            Previous = previous;
            Next = next;
        }
    }
}
