using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{
    public class SetShellContentEvent
    {
        public IScreen ViewModel { get; set; }

        public SetShellContentEvent()
        {
            ViewModel = null;
        }

        public SetShellContentEvent(IScreen viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
