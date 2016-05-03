using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;

namespace TT.Lib.Events
{
    public class VideoControlEvent
    {
        public Media.Control PlayMode { get; set; }
        public Media.Speed PlaySpeed { get; set; }
        public double Start { get; set; }
        public double End { get; set; }
        public TimeSpan Position { get; set; }
        public bool Init { get; set; }
        public bool Restart { get; set; }

        public VideoControlEvent()
        {
            PlayMode = Media.Control.None;
            Start = -100;
            End = -100;
            PlaySpeed = Media.Speed.None;
            Init = false;
            Restart = false;
            Position = new TimeSpan(0, 0, 0, 0, 0 - 100);
        }
    }
}
