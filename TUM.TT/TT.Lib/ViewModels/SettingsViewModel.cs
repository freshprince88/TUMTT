using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TT.Lib.Managers;
using TT.Lib.Results;
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

        protected override async void OnDeactivate(bool close)
        {

            if (MatchManager.MatchModified)
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Save and Close",
                    NegativeButtonText = "Cancel",
                    FirstAuxiliaryButtonText = "Close Without Saving",
                    AnimateShow = true,
                    AnimateHide = false
                };

                var result = await DialogCoordinator.ShowMessageAsync(this, "Close Window?",
                    "You didn't save your changes?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                bool _shutdown = result == MessageDialogResult.Affirmative;

                if (_shutdown)
                {
                    Coroutine.BeginExecute(MatchManager.SaveMatch().GetEnumerator(), new CoroutineExecutionContext() { View = this.GetView() });
                    Application.Current.Shutdown();
                }
            }
            events.Unsubscribe(this);
        }

        /// <summary>
        /// Determines whether the view model can be closed.
        /// </summary>
        /// <param name="callback">Called to perform the closing</param>
        public override void CanClose(System.Action<bool> callback)
        {
            var context = new CoroutineExecutionContext()
            {
                Target = this,
                View = this.GetView() as DependencyObject,
            };

            Coroutine.BeginExecute(
                this.PrepareClose().GetEnumerator(),
                context,
                (sender, args) =>
                {
                    callback(args.WasCancelled != true);
                });
        }

        /// <summary>
        /// Prepare the closing of this view model.
        /// </summary>
        /// <returns>The actions to execute before closing</returns>
        private IEnumerable<IResult> PrepareClose()
        {
            if (MatchManager.MatchModified)
            {



                var question = new YesNoCloseQuestionResult()
                {
                    Title = "Save the Changes?",
                    Question = "The Player Informations are modified. Save changes?",
                    AllowCancel = true,

                };
                yield return question;

                var match = MatchManager.Match;
                var lastRally = match.Rallies.LastOrDefault();
                //TODO
                if (match.Rallies.Any())
                {
                    if (lastRally.Winner == MatchPlayer.None)
                        match.Rallies.Remove(lastRally);
                }

                if (question.Result)
                {
                    foreach (var action in MatchManager.SaveMatch())
                    {
                        yield return action;
                    }
                }
            }
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

        public void ResetLibrary()
        {
            MatchLibrary.resetDb();
        }
        #endregion

        #region Events
        #endregion

        #region Helper Methods
        #endregion
    }
}
