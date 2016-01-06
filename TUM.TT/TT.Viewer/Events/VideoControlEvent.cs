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
        public double Start { get; set; }
        public double End { get; set; }
        public TimeSpan Position { get; set; }
        public bool Init { get; set; }
        public bool Restart { get; set; }

        public VideoControlEvent()
        {
            PlayMode = Media.Mode.None;
            Start = -100;
            End = -100;
            PlaySpeed = Media.Speed.None;
            Init = false;
            Restart = false;
            Position = new TimeSpan(0, 0, 0, 0, 0 - 100);
        }
    }
}
