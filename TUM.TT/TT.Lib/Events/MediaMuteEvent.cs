﻿using TT.Models.Util.Enums;

namespace TT.Lib.Events
{
    public class MediaMuteEvent
    {
        public Media.Mute Mute { get; set; }

        public MediaMuteEvent()
        {
            Mute = Media.Mute.Unmute;
        }

        public MediaMuteEvent(Media.Mute ctrl)
        {
            Mute = ctrl;
        }
    }
}
