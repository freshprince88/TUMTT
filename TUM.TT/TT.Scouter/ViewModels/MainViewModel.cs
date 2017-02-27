using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Linq;
using TT.Models;
using TT.Lib.Managers;
using System;
using System.Collections.Generic;

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

        private Tabs _selectedTab;
        public Tabs SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;                    
                    if (_selectedTab == Tabs.Live)
                    {                       
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
                            LiveView.CurrentRally.Server = LiveView.firstServerBackup;
                            LiveView.CurrentRally.UpdateServerAndScore();
                            NotifyOfPropertyChange("LiveView.CurrentRally");
                        }

                        this.ActivateItem(LiveView);
                    }
                    if (_selectedTab == Tabs.Remote)
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
                    NotifyOfPropertyChange("SelectedTab");
                }
            }
        }

        public MainViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator cor)
        {
            Events = ev;
            Manager = man;
            Dialogs = cor;
            Manager.ActiveRally = new Rally();
            LiveView = new LiveViewModel(Events, Manager, Dialogs);
            RemoteView = new RemoteViewModel(Events, Manager, Dialogs);
        }


        #region Caliburn Hooks
        protected override void OnActivate()
        {

            base.OnActivate();
            switch (SelectedTab)
            {
                case Tabs.Live:
                    LiveView.ViewMode = LiveMode;
                    this.ActivateItem(LiveView);
                    break;
                case Tabs.Remote:
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

        public IEnumerable<IResult> AddVideoFile()
        {
            foreach (var result in Manager.LoadVideo())
                yield return result;
            LiveView.ViewMode = LiveViewModel.TimeMode.Video;
            NotifyOfPropertyChange("MediaPlayer");
        }
        #endregion

    }
}
