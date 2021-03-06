﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;

namespace TT.Lib.Events
{
    public class ResultsChangedEvent : RalliesEvent
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsChangedEvent"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        public ResultsChangedEvent(IEnumerable<Rally> rallies)
            : base(rallies)
        {
        }
    }
}
