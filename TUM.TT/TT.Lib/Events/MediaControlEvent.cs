using TT.Models.Util.Enums;

namespace TT.Models.Events
{
    public class MediaControlEvent
    {
        public Media.Control Ctrl { get; set; }
        public Media.Source Source { get; set; }

        public MediaControlEvent()
        {
            Ctrl = Media.Control.None;
            Source = Media.Source.None;
        }

        public MediaControlEvent( Media.Control ctrl)
        {
            Ctrl = ctrl;
            Source = Media.Source.None;
        }
        public MediaControlEvent(Media.Control ctrl, Media.Source source)
        {
            Ctrl = ctrl;
            Source = source;
        }
    }
}
