using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;

namespace TT.Models.Events
{
    public class SpinControlSelectionChangedEvent
    {
        public SpinControlSelectionChangedEvent(List<Stroke.Spin> selected)
        {
            Selected = selected;
        }

        public List<Stroke.Spin> Selected { get; private set; }
    }
}
