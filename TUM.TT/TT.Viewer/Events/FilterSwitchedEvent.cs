using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.Events
{
    public class FilterSwitchedEvent : MatchEvent
    {
        public FilterSwitchedEvent(Match match)
            : base(match)
        {

        }
    }
}
