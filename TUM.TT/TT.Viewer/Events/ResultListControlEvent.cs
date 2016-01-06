using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Util.Enums;

namespace TT.Viewer.Events
{
    public class ResultListControlEvent
    {
        public Media.Control Control { get; set; }

        public ResultListControlEvent()
        {
            this.Control = Media.Control.None;
        }

        public ResultListControlEvent(Media.Control ctrl)
        {
            this.Control = ctrl;
        }
    }
}
