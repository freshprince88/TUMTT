using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.Events
{
    public class FilterSelectionChangedEvent : RalliesEvent
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterSelectionChangedEvent"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        public FilterSelectionChangedEvent(List<Rally> rallies)
            : base(rallies.ToArray())
        {
        }
    }
}
