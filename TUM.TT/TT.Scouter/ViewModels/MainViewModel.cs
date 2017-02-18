using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Linq;
using TT.Models;
using TT.Lib.Managers;
using System;

namespace TT.Scouter.ViewModels
{
    public class MainViewModel : Conductor<IScreen>.Collection.OneActive 
    {
        private IEventAggregator Events;
        private IMatchManager Manager;
        private IDialogCoordinator Dialogs;


        public LiveViewModel LiveView { get; set; }
        public RemoteViewModel RemoteView { get; set; }

        public LiveViewModel.TimeMode LiveMode { get; set; }
        private TimeSpan _lastLiveMediaPos;
        public TimeSpan LastLiveMediaPosition
        {
            get
            {
                return _lastLiveMediaPos;
            }
            set
            {
                if (_lastLiveMediaPos != value)
                    _lastLiveMediaPos = value;
                NotifyOfPropertyChange();
            }
        }
        private TimeSpan _lastRemoteMediaPos;

        public TimeSpan LastRemoteMediaPosition
        {
            get
            {
                return _lastRemoteMediaPos;
            }
            set
            {
                if (_lastRemoteMediaPos != value)
                    _lastRemoteMediaPos = value;
                NotifyOfPropertyChange();
            }
        }

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
                        this.ActivateItem(LiveView);

                        if (LiveView.Rallies.Any())
                        {
                            if (LiveView.Rallies.Last().Winner != MatchPlayer.None)
                            {
                                LiveView.CurrentRally = new Rally();
                                LiveView.Rallies.Add(LiveView.CurrentRally);
                                LiveView.CurrentRally.UpdateServerAndScore();
                                NotifyOfPropertyChange("LiveView.CurrentRally");
                            }
                        }
                        else
                        {
                            LiveView.CurrentRally = new Rally();
                            Manager.ActivePlaylist.Rallies.Add(LiveView.CurrentRally);
                            LiveView.Server = LiveView.firstServerBackup;
                            LiveView.CurrentRally.Server= LiveView.firstServerBackup;
                            LiveView.CurrentRally.UpdateServerAndScore();
                            NotifyOfPropertyChange("LiveView.CurrentRally");
                        }

                    }
                    if (_selectedTab == 1)
                    {                        
                        if (LiveView.Rallies.Any())
                        {
                            if (LiveView.Rallies.Last().Winner == MatchPlayer.None)
                            {
                                Manager.ActivePlaylist.Rallies.Remove(Manager.ActivePlaylist.Rallies.Last());
                            }
                        }

                        this.ActivateItem(RemoteView);
                    }
                }
            }
        }

        public MainViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator cor)
        {
            Events = ev;
            Manager = man;
            Dialogs = cor;
            LiveView = new LiveViewModel(Events, Manager, Dialogs);
            RemoteView = new RemoteViewModel(Events, Manager, Dialogs);                        
        }


        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            switch (SelectedTab)
            {
                case 0:
                    this.ActivateItem(LiveView);
                    break;
                case 1:
                    this.ActivateItem(RemoteView);
                    break;
                default:
                    break;
            }                        
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
