using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.Events
{
    public class SaveMatchEvent: MatchEvent
    {
        public SaveMatchEvent(Match match)
            : base(match)
        {

        }
    }
}
