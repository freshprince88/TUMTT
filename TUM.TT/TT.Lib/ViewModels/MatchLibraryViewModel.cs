using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.IO;
using System.Collections.ObjectModel;
using TT.Lib;
using TT.Lib.Managers;
using TT.Lib.Events;
using TT.Models;
using TT.Models.Api;
using TT.Lib.Properties;

namespace TT.Lib.ViewModels
{

    public class MatchLibraryViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx
    {
        public IEventAggregator events { get; private set; }
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;
        private IMatchManager MatchManager;
        private IMatchLibraryManager MatchLibrary;
        private ICloudSyncManager CloudSyncManager;

        public BindableCollection<MatchMeta> localResults { get; private set; }
        public NotifyTaskCompletion<MatchMetaResult> cloudResults { get; private set; }

        public string BaseUrl { get; private set; }
        public string LibraryPath { get; private set; }

        public MatchLibraryViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IDialogCoordinator coordinator, IMatchManager matchManager, ICloudSyncManager cloudSyncManager, IMatchLibraryManager matchLibrary)
        {
            this.DisplayName = "Match Library";
            this.events = eventAggregator;
            _windowManager = windowManager;
            DialogCoordinator = coordinator;
            MatchManager = matchManager;
            MatchLibrary = matchLibrary;
            CloudSyncManager = cloudSyncManager;

            BaseUrl = Settings.Default.CloudApiFrontend;
            LibraryPath = Settings.Default.LocalLibraryPath;
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

            localResults = new BindableCollection<MatchMeta>(MatchLibrary.GetMatches());
            cloudResults = new NotifyTaskCompletion<MatchMetaResult>(CloudSyncManager.GetMatches());
        }

        protected override void OnDeactivate(bool close)
        {
            events.Unsubscribe(this);
        }
        #endregion

        #region View Methods
        public void OpenMatch(ListView listView)
        {
            var item = listView.SelectedItems[0] as MatchMeta;
            Coroutine.BeginExecute(MatchManager.OpenMatch(item.FileName).GetEnumerator());
            this.TryClose();
        }

        public void DownloadMatch(ListView listView)
        {
            var item = listView.SelectedItems[0] as MatchMeta;
            var local = MatchLibrary.FindMatch(item.Guid);
            if (local != null)
            {
                Coroutine.BeginExecute(MatchManager.OpenMatch(local.FileName).GetEnumerator());
                this.TryClose();
                return;
            }




        }
        #endregion

        #region Events
        #endregion

        #region Helper Methods
        #endregion
    }
}
