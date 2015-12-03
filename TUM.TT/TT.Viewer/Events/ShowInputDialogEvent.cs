using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.Events
{
    public class ShowInputDialogEvent
    {
        public string Content { get; set; }
        public string Header { get; set; }


        public ShowInputDialogEvent()
        {
            this.Content = "";
            this.Header = "";
        }

        public ShowInputDialogEvent(string Content, string Header)
        {
            this.Content = Content;
            this.Header = Header;
        }
    }
}
