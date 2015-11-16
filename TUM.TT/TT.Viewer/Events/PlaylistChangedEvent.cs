using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.Events
{
    public class PlaylistChangedEvent
    {
        public string PlaylistName { get; set; }

        public PlaylistChangedEvent(string listName)            
        {
            PlaylistName = listName;
        }
    }
}
