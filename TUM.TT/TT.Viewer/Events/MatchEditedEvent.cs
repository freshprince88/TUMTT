using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.Events
{
    public class MatchEditedEvent : MatchEvent
    {
        public MatchEditedEvent(Match m)
            : base(m)
        {

        }
    }
}
