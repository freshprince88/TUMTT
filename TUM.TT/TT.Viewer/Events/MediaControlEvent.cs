using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Util.Enums;

namespace TT.Viewer.Events
{
    public class MediaControlEvent
    {
        public Media.Control PrevNext { get; set; }

        public MediaControlEvent()
        {
            PrevNext = Media.Control.None;
        }

        public MediaControlEvent( Media.Control ctrl)
        {
            PrevNext = ctrl;
        }
    }
}
