using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Models;

namespace TT.Viewer.Events
{
    public class RalliesEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RalliesEvent"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        protected RalliesEvent(MatchRally[] rallies)
        {
            this.Rallies = rallies;
        }

        /// <summary>
        /// Gets the match affected by this event.
        /// </summary>
        public MatchRally[] Rallies { get; private set; }
    }
}
