using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Events
{
    public class SpinControlSelectionChangedEvent
    {
        public SpinControlSelectionChangedEvent(List<SpinControlViewModel.Spins> selected)
        {
            Selected = selected;
        }

        public List<SpinControlViewModel.Spins> Selected { get; private set; }
    }
}
