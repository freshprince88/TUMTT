using TT.Lib.Util.Enums;

namespace TT.Lib.Events
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
