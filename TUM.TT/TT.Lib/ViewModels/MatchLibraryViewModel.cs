using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.IO;
using System.Collections.ObjectModel;
using TT.Lib.Api;
using TT.Lib.Managers;
using TT.Lib.Events;
using TT.Lib.Util;
using TT.Models.Api;
using TT.Lib.Properties;
using System.Threading;
using Application = System.Windows.Application;

namespace TT.Lib.ViewModels
{

    public class MatchLibraryViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx, IHandle<MatchOpenedEvent>
    {
        public IEventAggregator events { get; private set; }
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;
        private IMatchManager MatchManager;
        private IMatchLibraryManager MatchLibrary;
        private ICloudSyncManager CloudSyncManager;

        public ObservableCollection<MatchMeta> localResults { get; private set; }
        public ObservableCollection<MatchMeta> cloudResults { get; private set; }

        public bool IsCloudError = false;
        public string CloudErrorMessage;

        private bool _searchIsActive;
        public bool SearchIsActive
        {
            get { return _searchIsActive; }
            set
            {
                if (_searchIsActive != value)
                {
                    _searchIsActive = value;
                    NotifyOfPropertyChange("SearchIsActive");
                }
            }
        }

        public string BaseUrl { get; private set; }
        public string LibraryPath
        {
            get
            {
                return MatchLibrary.LibraryPath;
            }
        }

        public MatchLibraryViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IDialogCoordinator coordinator, IMatchManager matchManager, ICloudSyncManager cloudSyncManager, IMatchLibraryManager matchLibrary)
        {
            this.DisplayName = "Match Library";
            this.events = eventAggregator;
            _windowManager = windowManager;
            DialogCoordinator = coordinator;
            MatchManager = matchManager;
            MatchLibrary = matchLibrary;
            CloudSyncManager = cloudSyncManager;

            localResults = new ObservableCollection<MatchMeta>();
            cloudResults = new ObservableCollection<MatchMeta>();
            BaseUrl = Settings.Default.CloudApiFrontend;
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

            LoadLocalResults();
            LoadCloudResults();
        }

        protected override void OnDeactivate(bool close)
        {
            events.Unsubscribe(this);
        }
        #endregion

        #region Menu
        public IEnumerable<IResult> OpenMatch()
        {
            return MatchManager.OpenMatch();
        }

        public void OpenSettings()
        {
            _windowManager.ShowDialog(new SettingsViewModel(_windowManager, events, MatchManager, DialogCoordinator, CloudSyncManager, MatchLibrary));
        }
        #endregion

        #region Search
        public void LoadLocalResults(string query = null)
        {
            var matches = MatchLibrary.GetMatches(query);
            localResults.Clear();
            matches.ToList().ForEach(localResults.Add);
        }

        public async void LocalQuery(TextBox textBox)
        {
            var query = textBox.Text;
            await Task.Delay(250);
            if(query == textBox.Text)
            {
                SearchIsActive = true;
                LoadLocalResults(query);
                SearchIsActive = false;
            }
        }

        public async void LoadCloudResults(string query = null)
        {
            try
            {
                var matches = await CloudSyncManager.GetMatches(query);
                cloudResults.Clear();
                matches.rows.ForEach(cloudResults.Add);
            } catch(TTCloudApiException e)
            {
                Console.WriteLine(e.Message);
                //TODO: Set Cloud error
            }
        }

        public async void CloudQuery(TextBox textBox)
        {
            var query = textBox.Text;
            await Task.Delay(250);
            if (query == textBox.Text)
            {
                SearchIsActive = true;
                LoadCloudResults(query);
                SearchIsActive = false;
            }
        }

        #endregion

        #region ListView Actions

        public void OpenMatch(ListView listView)
        {
            if(listView.SelectedItems.Count < 1)
            {
                return;
            }
            var item = listView.SelectedItems[0] as MatchMeta;
            Coroutine.BeginExecute(MatchManager.OpenMatch(item.FileName).GetEnumerator());
            this.TryClose();
        }

        public async void DownloadMatch(ListView listView)
        {
            if (listView.SelectedItems.Count < 1)
            {
                return;
            }
            var matchMeta = listView.SelectedItems[0] as MatchMeta;

            // Open local match if in library
            var local = MatchLibrary.FindMatch(matchMeta.Guid);
            if (local != null)
            {
                Coroutine.BeginExecute(MatchManager.OpenMatch(local.FileName).GetEnumerator());
                this.TryClose();
                return;
            }

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var mySettings = new MetroDialogSettings() { CancellationToken = token };
            ProgressDialogController controller = await DialogCoordinator.ShowProgressAsync(this, "Downloading Match from Cloud", String.Empty, isCancelable: true, settings: mySettings);
            controller.Canceled += (sender, e) =>
            {
                tokenSource.Cancel();
            };
            controller.SetIndeterminate();

            string matchFilePath = MatchLibrary.GetMatchFilePath(matchMeta);
            string videoFilePath = MatchLibrary.GetVideoFilePath(matchMeta);
            MatchMeta meta;

            try
            {
                (meta, matchFilePath, videoFilePath) = await CloudSyncManager.DownloadMatch(
                    matchMeta.Guid, matchFilePath, videoFilePath, token, (status) =>
                 {
                     controller.SetMessage(status);
                 });
            }
            catch(TaskCanceledException)
            {
                // Delete downloaded artifacts
                try
                {
                    if (File.Exists(matchFilePath)) { File.Delete(matchFilePath); }
                    if (File.Exists(videoFilePath)) { File.Delete(videoFilePath); }
                } catch { /* Best effoty */ }

                await controller.CloseAsync();
                tokenSource.Dispose();
                return;
            }
            catch(TTCloudApiException)
            {
                //TODO: Show downlaod failure
                return;
            }

            controller.SetMessage("Opening Match...");

            // Create phantom analysis for download w/o analysis
            if (String.IsNullOrEmpty(matchFilePath))
            {
                MatchManager.CreateNewMatch();
                MatchManager.FileName = matchFilePath = MatchLibrary.GetMatchFilePath(meta);
                MatchManager.Match.VideoFile = videoFilePath;
                MatchMetaExtensions.UpdateMatchWithMetaInfo(MatchManager.Match, meta);

                await Coroutine.ExecuteAsync(MatchManager.SaveMatch().GetEnumerator());
            }
            Coroutine.BeginExecute(MatchManager.OpenMatch(matchFilePath, videoFilePath).GetEnumerator());

            await controller.CloseAsync();
            tokenSource.Dispose();
            this.TryClose();
        }
        #endregion

        #region Events
        public void Handle(MatchOpenedEvent message)
        {
            this.TryClose();
        }

        #endregion

        #region Helper Methods

        #endregion
    }
}
