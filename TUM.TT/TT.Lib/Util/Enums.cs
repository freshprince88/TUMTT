using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Util.Enums
{
    public class Media
    {
        public enum Mute
        {
            Mute,
            Unmute
        }

        public enum Speed
        {
            Quarter = 25,
            Half = 50,
            Third = 33,
            Full = 100,
            None
        }

        public enum Control
        {
            Previous,
            Next,
            Stop,
            Pause,
            Play,
            None
        }
    }
}
