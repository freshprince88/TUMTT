using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.Events
{
    public class PlaylistNamedEvent
    {
        public string Name { get; set; }

        public PlaylistNamedEvent(string name)
        {
            this.Name = name;
        }

    }
}
