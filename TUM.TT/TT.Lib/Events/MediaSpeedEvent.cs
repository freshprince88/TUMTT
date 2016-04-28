using TT.Models.Util.Enums;

namespace TT.Models.Events
{
    public class MediaSpeedEvent
    {
        public Media.Speed Speed { get; set; }

        public MediaSpeedEvent()
        {
            Speed = Media.Speed.None;
        }

        public MediaSpeedEvent(Media.Speed ctrl)
        {
            Speed = ctrl;
        }
    }
}
