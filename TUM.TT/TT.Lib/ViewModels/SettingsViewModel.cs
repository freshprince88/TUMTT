using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TT.Lib.Managers;
using TT.Lib.Events;
using TT.Models;
using TT.Models.Api;
using TT.Models.Util;

namespace TT.Lib.ViewModels
{
    public class SettingsViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx
    {
        public IEventAggregator events { get; private set; }
        public IMatchManager MatchManager { get; set; }
        public ICloudSyncManager CloudSyncManager;
        private IMatchLibraryManager MatchLibrary;
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;

        #region Calculated Properties
        public string AccountEmail
        {
            get
            {
                return CloudSyncManager.GetAccountEmail();
            }
        }

        public string AccountStatus
        {
            get
            {
                return EnumExtensions.GetDescription<ConnectionStatus>(CloudSyncManager.GetConnectionStatus());
            }
        }

        public string AccountMessage
        {
            get
            {
                return CloudSyncManager.GetConnectionMessage();
            }
        }

        private string _matchStatus = "Loading...";
        public string MatchStatus
        {
            get
            {
                return _matchStatus;
            }
        }

        public string LibraryPath
        {
            get
            {
                return MatchLibrary.LibraryPath;
            }
        }
        #endregion


        public SettingsViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IMatchManager man, IDialogCoordinator coordinator, ICloudSyncManager cloudSyncManager, IMatchLibraryManager libraryManager)
        {
            this.DisplayName = "Settings";
            this.events = eventAggregator;
            MatchManager = man;
            MatchLibrary = libraryManager;
            CloudSyncManager = cloudSyncManager;
            _windowManager = windowmanager;
            DialogCoordinator = coordinator;

            RefreshMatchStatus();
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
        }

        protected override void OnDeactivate(bool close)
        {

            events.Unsubscribe(this);
        }

        #endregion

        #region View Methods
        public void ChangeAccount()
        {

        }

        public async void RefreshAccount()
        {
            await CloudSyncManager.Login();
            NotifyOfPropertyChange("AccountStatus");
            NotifyOfPropertyChange("AccountMessage");
        }

        public async void RefreshMatchStatus()
        {
            _matchStatus = EnumExtensions.GetDescription<SyncStatus>(SyncStatus.None);
            if (MatchManager.Match != null)
            {
                try
                {
                    MatchMeta meta = await CloudSyncManager.GetMatch(MatchManager.Match.ID);
                    _matchStatus = EnumExtensions.GetDescription<SyncStatus>(CloudSyncManager.GetSyncStatus(meta));
                }
                catch { }
            }
            NotifyOfPropertyChange("MatchStatus");
        }

        public void ChangeLibraryLocation()
        {

        }

        public async void ResetLibrary()
        {
            MessageDialogResult result = await DialogCoordinator.ShowMessageAsync(this,
                "Reset Match Library",
                "Are you sure to reset the match library and delete all associated files?",
                MessageDialogStyle.AffirmativeAndNegative);
            if(result == MessageDialogResult.Affirmative)
            {
                MatchLibrary.resetDb(true);
                events.PublishOnUIThread(new LibraryResetEvent());
            }
        }
        #endregion

        #region Events
        #endregion

        #region Helper Methods
        #endregion
    }
}
