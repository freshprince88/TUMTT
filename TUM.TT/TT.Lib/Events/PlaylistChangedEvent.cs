using TT.Models;

namespace TT.Models.Events
{
    public class PlaylistChangedEvent : PlaylistEvent
    {
        public PlaylistChangedEvent(Playlist list) : base(list)
        {

        }
    }
}
