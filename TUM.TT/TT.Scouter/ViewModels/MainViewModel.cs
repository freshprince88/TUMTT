using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib.Managers;
using System.Linq;
using TT.Lib.Models;
using System.Collections.ObjectModel;
using TT.Scouter.Interfaces;
using System.Windows.Controls;
using TT.Lib.Util;
using TT.Lib.Results;
using System.Collections.Generic;

namespace TT.Scouter.ViewModels
{
    public class MainViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private IEventAggregator Events;
        private IMatchManager Manager;

        public LiveViewModel LiveView { get; set; }
        public RemoteViewModel RemoteView { get; set; }

        public LiveViewModel.TimeMode LiveMode { get; set; }

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
                    if (_selectedTab == 0)
                    {
                        Rally lastRally = LiveView.Rallies.Last();
                        lastRally.UpdateServerAndScore();
                    }
                }
            }
        }

        public MainViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator dia)
        {
            Events = ev;
            Manager = man;
            LiveView = new LiveViewModel(Events, Manager);
            RemoteView = new RemoteViewModel(Events, Manager, dia);
        }


        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            LiveView.ViewMode = LiveMode;
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
