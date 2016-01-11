using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Util.Enums;

namespace TT.Lib.Events
{
    public class ResultListControlEvent
    {
        public Rally SelectedRally { get; set; }

        public ResultListControlEvent()
        {
            this.SelectedRally = null;
        }

        public ResultListControlEvent(Rally selected)
        {
            this.SelectedRally = selected;
        }
    }
}
