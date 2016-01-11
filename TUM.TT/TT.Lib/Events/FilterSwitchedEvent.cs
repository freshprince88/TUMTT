using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{
    public class FilterSwitchedEvent : PlaylistEvent
    {
        public FilterSwitchedEvent(Playlist list)
            : base(list)
        {

        }
    }
}
