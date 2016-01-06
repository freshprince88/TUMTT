using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Viewer.ViewModels;
using TT.Lib.Util.Enums;

namespace TT.Viewer.Events
{
    public class VideoControlEvent
    {
        public Media.Mode PlayMode { get; set; }
        public Media.Speed PlaySpeed { get; set; }
        public TimeSpan Position { get; set; }
        public double Duration { get; set; }
        public bool Init { get; set; }
        public bool Restart { get; set; }

        public VideoControlEvent()
        {
            PlayMode = Media.Mode.None;
            Position = new TimeSpan(0, 0, 0, 0, -100);
            PlaySpeed = Media.Speed.None;
            Duration = -100;
            Init = false;
            Restart = false;
        }

        public VideoControlEvent(Media.Mode mode, Media.Speed speed, TimeSpan position, double duration, bool init, bool restart)
        {
            PlayMode = mode;
            Position = position;
            PlaySpeed = speed;
            Duration = duration;
            Init = init;
            Restart = restart;
        }
    }
}
