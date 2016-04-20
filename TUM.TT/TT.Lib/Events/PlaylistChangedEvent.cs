using TT.Lib.Models;

namespace TT.Lib.Events
{
    public class PlaylistChangedEvent : PlaylistEvent
    {
        public PlaylistChangedEvent(Playlist list) : base(list)
        {

        }
    }
}
