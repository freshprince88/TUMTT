using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;

namespace TT.Lib.Events
{
    public class BasicFilterSelectionChangedEvent : RalliesEvent
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicFilterSelectionChangedEvent"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        public BasicFilterSelectionChangedEvent(List<Rally> rallies)
            : base(rallies.ToArray())
        {
        }
    }
}
