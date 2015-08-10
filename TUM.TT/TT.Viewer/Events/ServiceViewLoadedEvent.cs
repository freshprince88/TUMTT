using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Models;

namespace TT.Viewer.Events
{
    public class ServiceViewLoadedEvent : RalliesEvent
    {
                /// <summary>
        /// Initializes a new instance of the <see cref="MatchOpenedEvent"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="fileName">The file name the match was opened from</param>
        public ServiceViewLoadedEvent(MatchRally[] rallies)
            : base(rallies)
        {
        }
    }
}
