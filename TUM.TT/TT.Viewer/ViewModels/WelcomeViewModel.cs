using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class WelcomeViewModel : Screen
    {
        private IMatchManager Manager;

        public WelcomeViewModel(IMatchManager manager)
        {
            Manager = manager;
        }


        public IEnumerable<IResult> OpenMatch()
        {
            return Manager.OpenMatch();
        }
    }
}
