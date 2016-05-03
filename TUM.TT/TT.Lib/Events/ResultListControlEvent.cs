
using TT.Models;

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
