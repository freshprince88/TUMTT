using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;

namespace TT.Lib.Events
{
    public class RalliesEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RalliesEvent"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        protected RalliesEvent(IEnumerable<Rally> rallies)
        {
            this.Rallies = rallies;
        }

        /// <summary>
        /// Gets the match affected by this event.
        /// </summary>
        public IEnumerable<Rally> Rallies { get; private set; }
    }
}
