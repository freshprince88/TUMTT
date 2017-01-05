using TT.Models.Util.Enums;

namespace TT.Lib.Events
{
    public class MediaLiveScouterSpeedEvent
    {
        public Media.LiveScouterSpeed LiveScouterSpeed { get; set; }

        public MediaLiveScouterSpeedEvent()
        {
            LiveScouterSpeed = Media.LiveScouterSpeed.None;
        }

        public MediaLiveScouterSpeedEvent(Media.LiveScouterSpeed ctrl)
        {
            LiveScouterSpeed = ctrl;
        }
    }
}
