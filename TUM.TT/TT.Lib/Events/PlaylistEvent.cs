using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;

namespace TT.Lib.Events
{
    public class PlaylistEvent
    {
        public Playlist List { get; set; }

        public PlaylistEvent(Playlist list)
        {
            this.List = list;
        }
    }
}
