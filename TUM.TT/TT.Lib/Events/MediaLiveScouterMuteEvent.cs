using TT.Models.Util.Enums;

namespace TT.Lib.Events
{
    public class MediaLiveScouterMuteEvent
    {
        public Media.Mute Mute { get; set; }

        public MediaLiveScouterMuteEvent()
        {
            Mute = Media.Mute.Unmute;
        }

        public MediaLiveScouterMuteEvent(Media.Mute ctrl)
        {
            Mute = ctrl;
        }
    }
}
