using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Models;

namespace TT.Lib.Events
{
    public class VideoPlayEvent
    {
        public Rally Current { get; set; }

        public VideoPlayEvent()
        {
            this.Current = null;
        }

        public VideoPlayEvent(Rally r)
        {
            this.Current = r;
        }
    }
}
