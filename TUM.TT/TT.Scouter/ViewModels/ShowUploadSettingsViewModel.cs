using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using TT.Lib;
using TT.Lib.Managers;
using TT.Lib.Api;
using TT.Lib.Events;
using TT.Models;
using TT.Models.Api;
using TT.Models.Util;

namespace TT.Scouter.ViewModels
{
    public class ShowUploadSettingsViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx, IHandle<CloudSyncActivityStatusChangedEvent>
    {
        public IEventAggregator events { get; private set; }
        public IMatchManager MatchManager { get; set; }
        public ICloudSyncManager CloudSyncManager { get; set; }
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;
        public Match Match { get { return MatchManager.Match; } }

        #region Calculated Properties
        private string _matchStatus = "Loading...";
        public string MatchStatus
        {
            get
            {
                return _matchStatus;
            }
        }
        public bool IsIndeterminate
        {
            get
            {
                return
                    (CloudSyncManager.ActivityStauts != ActivityStauts.None &&
                     CloudSyncManager.ActivityStauts != ActivityStauts.TranscodingVideo);
            }
        }
        public bool IsActivity
        {
            get
            {
                return CloudSyncManager.ActivityStauts != ActivityStauts.None;
            }
        }
        public bool IsProgressValue
        {
            get
            {
                return CloudSyncManager.ActivityStauts == ActivityStauts.TranscodingVideo;
            }
        }
        #endregion

        public ShowUploadSettingsViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IMatchManager man, ICloudSyncManager cloudSyncManager, IDialogCoordinator coordinator)
        {
            this.DisplayName = "Upload Settings";
            this.events = eventAggregator;
            MatchManager = man;
            _windowManager = windowmanager;
            DialogCoordinator = coordinator;

            CloudSyncManager = cloudSyncManager;
        }

        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            RefreshMatchStatus();
        
        }

        protected override void OnDeactivate(bool close)
        { 
            events.Unsubscribe(this);
        }
        #endregion

        #region View Methods

        public async void RefreshMatchStatus()
        {
            _matchStatus = EnumExtensions.GetDescription(SyncStatus.None);
            if (MatchManager.Match != null)
            {
                try
                {
                    MatchMeta meta = await CloudSyncManager.GetMatch(MatchManager.Match.ID);
                    _matchStatus = EnumExtensions.GetDescription(CloudSyncManager.GetSyncStatus(meta));
                }
                catch (CloudException) {}
            }
            NotifyOfPropertyChange(() => MatchStatus);
        }

        public async void SyncMatch()
        {
            await CloudSyncManager.UploadMatch();
        }

        public void CacnelSync()
        {
            CloudSyncManager.CancelSync();
        }

        #endregion

        #region Events
        public void Handle(CloudSyncActivityStatusChangedEvent e)
        {
            NotifyOfPropertyChange(() => IsIndeterminate);
            NotifyOfPropertyChange(() => IsActivity);
            NotifyOfPropertyChange(() => IsProgressValue);
        }

        #endregion
    }
}
