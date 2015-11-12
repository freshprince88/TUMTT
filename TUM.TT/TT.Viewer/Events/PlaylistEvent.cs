using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.Events
{
    public class PlaylistEvent
    {

        public Playlist Playlist { get; set; }

        public PlaylistEvent(Playlist list)
        {
            this.Playlist = list;
        }
    }
}
