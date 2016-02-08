using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Events;
using TT.Lib.Managers;

namespace TT.Scouter.ViewModels
{
    public class WelcomeViewModel : Screen
    {
        private IEventAggregator Events;

        public WelcomeViewModel(IEventAggregator ev)
        {
            Events = ev;
        }

        #region View Methods

        public void OpenLiveView()
        {
            var liveViewModel = new LiveViewModel();
            IoC.BuildUp(liveViewModel);
            Events.PublishOnUIThread(new SetShellContentEvent(liveViewModel));
        }

        public void OpenDetailView()
        {
            //TODO: Open DetailView in Shell
        }

        #endregion
    }

}
