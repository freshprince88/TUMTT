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

    public class MatchLibraryViewModel : 
        Conductor<IScreen>.Collection.AllActive,
        IShell, INotifyPropertyChangedEx,
        IHandle<MatchOpenedEvent>,
        IHandle<LibraryResetEvent>,
        IHandle<CloudSyncConnectionStatusChangedEvent>
    {
        #region Properties
        public IEventAggregator events { get; private set; }
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;
        private IMatchManager MatchManager;
        private IMatchLibraryManager MatchLibrary;
        private ICloudSyncManager CloudSyncManager;

        public ObservableCollection<MatchMeta> LocalResults { get; private set; }
        public ObservableCollection<MatchMeta> CloudResults { get; private set; }

        public string LocalQueryValue { get; set; }
        public string CloudQueryValue { get; set; }
        public string SelectedLocalSort { get; set; } = "LastOpenedAt.Descending";
        public string SelectedCloudSort { get; set; } = "updatedAt.desc";
        #endregion

        #region Computed Properties
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

        public bool HasNoLogin
        {
            get
            {
                return CloudSyncManager.GetAccountEmail() == null;
            }
        }

        public bool IsOffline
        {
            get
            {
                return !HasNoLogin && (CloudSyncManager.GetConnectionStatus() != ConnectionStatus.Online);
            }
        }
        public string CloudErrorMessage
        {
            get
            {
                return CloudSyncManager.GetConnectionMessage();
            }
        }
        #endregion

        public MatchLibraryViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IDialogCoordinator coordinator, IMatchManager matchManager, ICloudSyncManager cloudSyncManager, IMatchLibraryManager matchLibrary)
        {
            this.DisplayName = "Match Library";
            this.events = eventAggregator;
            _windowManager = windowManager;
            DialogCoordinator = coordinator;
            MatchManager = matchManager;
            MatchLibrary = matchLibrary;
            CloudSyncManager = cloudSyncManager;

            LocalResults = new ObservableCollection<MatchMeta>();
            CloudResults = new ObservableCollection<MatchMeta>();
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

        public void OpenLogin()
        {
            _windowManager.ShowDialog(new LoginViewModel(_windowManager, events, MatchManager, DialogCoordinator, CloudSyncManager, MatchLibrary));
        }
        #endregion

        #region Search
        public void LoadLocalResults()
        {
            var sort = SelectedLocalSort.Split('.');
            var matches = MatchLibrary.GetMatches(LocalQueryValue, sort[0], sort[1]);
            LocalResults.Clear();
            matches.ToList().ForEach(LocalResults.Add);
        }

        public async void LocalQuery(TextBox textBox)
        {
            var query = textBox.Text;
            await Task.Delay(250);
            if(query == textBox.Text)
            {
                SearchIsActive = true;
                LoadLocalResults();
                SearchIsActive = false;
            }
        }

        public async void LoadCloudResults()
        {
            var sort = SelectedCloudSort.Split('.');
            try
            {
                var matches = await CloudSyncManager.GetMatches(CloudQueryValue, sort[0], sort[1]);
                CloudResults.Clear();
                matches.rows.ForEach(CloudResults.Add);
            } catch(CloudException) { }
        }

        public async void CloudQuery(TextBox textBox)
        {
            var query = textBox.Text;
            await Task.Delay(250);
            if (query == textBox.Text)
            {
                SearchIsActive = true;
                LoadCloudResults();
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

        public void DeleteMatch(MatchMeta match)
        {
            MatchLibrary.DeleteMatch(match.Guid);
            LocalResults.Remove(match);
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
                TryClose();
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

            await DownloadMatch(matchMeta, token, callback:(msg) =>
            {
                controller.SetMessage(msg);
            });    

            await controller.CloseAsync();
            if (!tokenSource.IsCancellationRequested)
            {
                TryClose();
            }
            tokenSource.Dispose();
        }

        private async Task<MatchMeta> DownloadMatch(MatchMeta matchMeta, CancellationToken token, bool openVideo = true, Action<string> callback = null)
        {
            string matchFilePath = MatchLibrary.GetMatchFilePath(matchMeta);
            string videoFilePath = MatchLibrary.GetVideoFilePath(matchMeta);
            MatchMeta meta;

            try
            {
                (meta, matchFilePath, videoFilePath) = await CloudSyncManager.DownloadMatch(
                    matchMeta.Guid, matchFilePath, videoFilePath, token, (status) =>
                    {
                        callback?.Invoke(status);
                    });
            }
            catch (TaskCanceledException)
            {
                // Delete downloaded artifacts
                try
                {
                    if (File.Exists(matchFilePath)) { File.Delete(matchFilePath); }
                    if (File.Exists(videoFilePath)) { File.Delete(videoFilePath); }
                }
                catch { /* Best effoty */ }
                return matchMeta;
            }
            catch (CloudException)
            {
                return matchMeta;
            }

            callback?.Invoke("Opening Match...");

            // Create phantom analysis for download w/o analysis
            if (string.IsNullOrEmpty(matchFilePath))
            {
                MatchManager.CreateNewMatch();
                MatchManager.FileName = matchFilePath = MatchLibrary.GetMatchFilePath(meta);
                MatchManager.Match.VideoFile = videoFilePath;
                MatchManager.Match.SyncToCloud = true;
                MatchMetaExtensions.UpdateMatchWithMetaInfo(MatchManager.Match, meta);

                await Coroutine.ExecuteAsync(MatchManager.SaveMatch().GetEnumerator());
            }
            if (openVideo)
            {
                Coroutine.BeginExecute(MatchManager.OpenMatch(matchFilePath, videoFilePath).GetEnumerator());
            } else
            {
                Coroutine.BeginExecute(MatchManager.OpenMatch(matchFilePath).GetEnumerator());
            }

            return meta;
        }

        public async void DownloadResults()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var count = CloudResults.Count;

            var mySettings = new MetroDialogSettings() { CancellationToken = token };
            ProgressDialogController controller = await DialogCoordinator.ShowProgressAsync(this,
                "Downloading " + count.ToString() + " Matches from Cloud",
                string.Empty, isCancelable: true, settings: mySettings);
            controller.SetProgress(0);
            controller.Canceled += (sender, e) =>
            {
                tokenSource.Cancel();
                controller.CloseAsync();
            };

            int progessCount = 0;
            foreach(MatchMeta matchMeta in CloudResults)
            {
                if(tokenSource.IsCancellationRequested)
                {
                    break;
                }

                if(MatchLibrary.FindMatch(matchMeta.Guid) == null)
                {
                    await DownloadMatch(matchMeta, token, openVideo: false, callback: (msg) =>
                    {
                        string s = string.Format("({0}/{1}): ", progessCount + 1, count);
                        controller.SetMessage(s + msg);
                    });
                }

                progessCount++;
                controller.SetProgress(progessCount / (double)count);
            }

            await controller.CloseAsync();
            tokenSource.Dispose();
            LoadLocalResults();
        }
        #endregion

        #region Events
        public void Handle(MatchOpenedEvent message)
        {
            this.TryClose();
        }

        public void Handle(LibraryResetEvent message)
        {
            LoadLocalResults();
        }

        public void Handle(CloudSyncConnectionStatusChangedEvent e)
        {
            NotifyOfPropertyChange(() => this.HasNoLogin);
            NotifyOfPropertyChange(() => this.IsOffline);
            NotifyOfPropertyChange(() => this.CloudErrorMessage);
            LoadCloudResults();
        }
        #endregion
    }
}
