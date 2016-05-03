using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{
    public class VideoLoadedEvent
    {
        public string VideoFile { get; private set; }

        public VideoLoadedEvent(string file)
        {
            VideoFile = file;
        }

    }
}
