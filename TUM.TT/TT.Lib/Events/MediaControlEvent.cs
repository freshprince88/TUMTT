using TT.Models.Util.Enums;

namespace TT.Models.Events
{
    public class MediaControlEvent
    {
        public Media.Control Ctrl { get; set; }

        public MediaControlEvent()
        {
            Ctrl = Media.Control.None;
        }

        public MediaControlEvent( Media.Control ctrl)
        {
            Ctrl = ctrl;
        }
    }
}
