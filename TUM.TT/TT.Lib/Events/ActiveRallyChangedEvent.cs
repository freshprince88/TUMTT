using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;

namespace TT.Lib.Events
{
    public class ActiveRallyChangedEvent
    {
        public Rally Current { get; set; }

        public ActiveRallyChangedEvent()
        {
            this.Current = null;
        }

        public ActiveRallyChangedEvent(Rally r)
        {
            this.Current = r;
        }
    }
}
