using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Managers;

namespace TT.Scouter.ViewModels
{
    public class MainViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private IEventAggregator Events;
        private IMatchManager Manager;

        public LiveViewModel LiveView { get; set; }
        public RemoteViewModel RemoteView { get; set; }

        public enum Tabs
        {
            Live,
            Remote
        }

        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;
                    NotifyOfPropertyChange("SelectedTab");
                }
            }
        }

        public MainViewModel(IEventAggregator ev, IMatchManager man)
        {
            Events = ev;
            Manager = man;
            LiveView = new LiveViewModel(Events, Manager);
            RemoteView = new RemoteViewModel(Events, Manager);
        }


        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            this.ActivateItem(LiveView);
            this.ActivateItem(RemoteView);
        }

        protected override void OnDeactivate(bool close)
        {
            this.DeactivateItem(LiveView, close);
            this.DeactivateItem(RemoteView, close);
            base.OnDeactivate(close);
        }

        #endregion

        #region View Methods

        #endregion

    }
}
