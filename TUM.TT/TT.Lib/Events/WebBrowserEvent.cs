using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{
    public class WebBrowserEvent
    {
        public string CurrentUrl { get; set; }
        public WebBrowserEvent(string url)
        {
            CurrentUrl = url;
        }
    }
}
