using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using TT.Lib.Events;
using TT.Lib.Api;
using TT.Models.Api;
using Caliburn.Micro;
using NReco.VideoConverter;
using NReco.VideoInfo;
using vaultsharp;
using vaultsharp.native;

namespace TT.Lib.Managers
{

    public enum ConnectionStatus
    {
        [Description("Offline")]
        Offline,
        [Description("Connecting")]
        Connecting,
        [Description("Online")]
        Online
    }

    public enum SyncStatus
    {
        [Description("No status")]
        None,
        [Description("Match does not exist in cloud")]
        NotExists,
        [Description("Local match is outdated")]
        Outdated,
        [Description("Match is in sync")]
        InSync,
        [Description("Match in cloud was changed by other user")]
        ChangedByOther
    }

    public enum ActivityStauts
    {
        [Description("No Activity")]
        None,
        [Description("Checking Upload Status")]
        Check,
        [Description("Updating Match")]
        Updating,
        [Description("Uploading Video")]
        UploadVideo,
        [Description("Transcoding Video")]
        TranscodingVideo,
    }

    public class CloudException : Exception
    {

        public CloudException()
        {
        }

        public CloudException(string message)
            : base(message)
        {
        }

        public CloudException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class CloudSyncManager : PropertyChangedBase, ICloudSyncManager,
        IHandle<MatchOpenedEvent>, IHandle<MatchSavedEvent>
    {
        #region Properties
        private IEventAggregator EventAggregator { get; set; }
        public IMatchManager MatchManager { get; set; }
        public ConnectionStatus ConnectionStatus = ConnectionStatus.Offline;
        public string ConnectionMessage;
        private TTCloudApi CloudApi;
        private CancellationTokenSource TokenSource;
        private FFMpegConverter ffmpeg;

        public bool AutoUpload { get; set; } = false;
        private const string applicationName = "TUM.TT Cloud";
        public bool IsUploadRequired { get; private set; } = false;
        #endregion

        #region Calculated Prperties
        private User currentUser = null;
        public User CurrentUser
        {
            get
            {
                return currentUser;
            }
        }

        private double uploadProgress;
        public double UploadProgress
        {
            get
            {
                return uploadProgress;
            }
            private set
            {
                uploadProgress = value;
                NotifyOfPropertyChange(() => UploadProgress);
            }
        }

        private ActivityStauts activityStauts = ActivityStauts.None;
        public ActivityStauts ActivityStauts
        {
            get
            {
                return activityStauts;
            }
            private set
            {
                var notify = activityStauts != value;
                activityStauts = value;
                NotifyOfPropertyChange(() => ActivityStauts);
                if(notify) EventAggregator.PublishOnUIThread(new CloudSyncActivityStatusChangedEvent(activityStauts));
            }
        }
        #endregion

        public CloudSyncManager(IEventAggregator eventAggregator, IMatchManager matchManager)
        {
            EventAggregator = eventAggregator;
            this.MatchManager = matchManager;

            EventAggregator.Subscribe(this);

            CloudApi = new TTCloudApi(string.Empty);
            TokenSource = new CancellationTokenSource();
            ffmpeg = new FFMpegConverter();
        }

        #region Login
        public void SetCredentials(string email, string password)
        {
            WindowsCredentialManager.WriteCredentials(applicationName, email, password, (int)CredentialPersistence.LocalMachine);
        }

        private Credential GetCredentials()
        {
            var Credential = new Credential();
            try
            {
                Credential = WindowsCredentialManager.ReadCredential(applicationName);
            } catch {};
            return Credential;
        }

        public string GetAccountEmail()
        {
            return GetCredentials().UserName;
        }

        public async Task<string> Login()
        {
            Credential Credentials = GetCredentials();
            string AccessToken = "";
            var oldStatus = ConnectionStatus;
            try
            {
                ConnectionStatus = ConnectionStatus.Connecting;
                string password = Credentials.GetSecret(System.Text.Encoding.Unicode.GetString);
                AccessToken = await TTCloudApi.Login(Credentials.UserName, password);
                ConnectionStatus = ConnectionStatus.Online;
                ConnectionMessage = String.Empty;
            } catch(CloudException e)
            {
                ConnectionMessage = e.Message;
                ConnectionStatus = ConnectionStatus.Offline;
            } catch
            {
                ConnectionStatus = ConnectionStatus.Offline;
            }
            

            CloudApi = new TTCloudApi(AccessToken);
            if (ConnectionStatus == ConnectionStatus.Online)
            {
                currentUser = await CloudApi.GetCurrentUser();
            }

            if (oldStatus != ConnectionStatus)
            {
                EventAggregator.PublishOnUIThread(new CloudSyncConnectionStatusChangedEvent(ConnectionStatus));
            }

            return AccessToken;
        }
        #endregion

        #region Status Flags
        public ConnectionStatus GetConnectionStatus()
        {
            return ConnectionStatus;
        }

        public string GetConnectionMessage()
        {
            return ConnectionMessage;
        }

        public SyncStatus GetSyncStatus(MatchMeta meta)
        {
            if (meta == null)
            {
                return SyncStatus.NotExists;
            }
            else if (MatchManager.Match.LastCloudSync == new DateTime())
            {
                return SyncStatus.Outdated;
            }
            else if (MatchManager.Match.LastCloudSync < meta.AnalysisFileUpdatedAt)
            {
                return SyncStatus.ChangedByOther;
            }
            else if (MatchManager.Match.LastCloudSync >= meta.AnalysisFileUpdatedAt)
            {
                return SyncStatus.InSync;
            }
            return SyncStatus.None;
        }
        #endregion

        #region Events
        public async void Handle(MatchOpenedEvent message)
        {
            if(AutoUpload)
            {
                // Wait for match to be fully loaded
                await Task.Delay(5000);
                if (MatchManager.Match.SyncToCloud)
                {
                    await UploadMatch();
                }
            }
        }

        public async void Handle(MatchSavedEvent message)
        {
            if(message.Match.SyncToCloud && ActivityStauts == ActivityStauts.None)
            {
                try {
                    await UploadAnalysisFile();
                } catch(CloudException)
                {
                    IsUploadRequired = true;
                }
            }
        }
        #endregion

        #region Basic Api Functions
        private Task<MatchMeta> UpsertMatchMeta()
        {
            MatchMeta matchMeta = MatchMeta.FromMatch(MatchManager.Match);
            return CloudApi.PutMatch(matchMeta, TokenSource.Token);
        }

        public Task<MatchMetaResult> GetMatches(string query=null, string sortFild="updatedAt", string sortOrder="desc", int limit = 100)
        {
            return CloudApi.GetMatches(query, sortFild, sortOrder, limit);
        }

        public Task<MatchMeta> GetMatch(Guid id)
        {
            return CloudApi.GetMatch(id);
        }
        #endregion

        #region Upload
        public async Task<MatchMeta> UploadMatch()
        {
            if (TokenSource.IsCancellationRequested)
            {
                TokenSource.Dispose();
                TokenSource = new CancellationTokenSource();
            }

            MatchMeta meta = MatchMeta.FromMatch(MatchManager.Match);
            ActivityStauts = ActivityStauts.Check;
            try
            {
                meta = await CloudApi.GetMatch(MatchManager.Match.ID, TokenSource.Token);
                var status = GetSyncStatus(meta);

                if (status == SyncStatus.NotExists || status == SyncStatus.Outdated)
                {
                    await UpsertMatchMeta();
                    meta = await UploadAnalysisFile();
                    status = GetSyncStatus(meta);
                }

                if (status == SyncStatus.InSync && meta.VideoStatus == VideoStatus.None)
                {
                    meta = await UploadVideo(MatchManager.Match.VideoFile, MatchManager.Match.ID);
                }
            }
            catch (CloudException)
            {
                TokenSource.Cancel();
            }
            catch (TaskCanceledException) {}

            ActivityStauts = ActivityStauts.None;
            return meta;
        }

        public async Task<MatchMeta> UploadMetaVideo(MatchMeta meta, string videoFilename)
        {
            if (TokenSource.IsCancellationRequested)
            {
                TokenSource.Dispose();
                TokenSource = new CancellationTokenSource();
            }

            ActivityStauts = ActivityStauts.Updating;
            try
            {
                await CloudApi.PutMatch(meta, TokenSource.Token);
                meta = await UploadVideo(videoFilename, meta.Guid);
            }
            catch (CloudException)
            {
                TokenSource.Cancel();
            }
            catch (TaskCanceledException) { }

            ActivityStauts = ActivityStauts.None;
            return meta;
        }

        private async Task<MatchMeta> UploadAnalysisFile()
        {
            await Coroutine.ExecuteAsync(MatchManager.SaveMatch().GetEnumerator());
            var meta = await CloudApi.UploadAnalysisFile(MatchManager.Match.ID, MatchManager.FileName, TokenSource.Token);
            MatchManager.Match.LastCloudSync = meta.AnalysisFileUpdatedAt;
            await Coroutine.ExecuteAsync(MatchManager.SaveMatch().GetEnumerator());
            IsUploadRequired = false;
            return meta;
        }

        public async Task<MatchMeta> UpdateAnalysis()
        {
            var cloudMeta = await CloudApi.GetMatch(MatchManager.Match.ID);
            var status = GetSyncStatus(cloudMeta);

            await UpsertMatchMeta();
            return await UploadAnalysisFile();
        }

        private async Task<MatchMeta> UploadVideo(string videoFile, Guid guid)
        {
            var isVideoValid = await IsVideoValid(videoFile);
            if (!isVideoValid)
            {
                ActivityStauts = ActivityStauts.TranscodingVideo;
                videoFile = await ConvertVideoFile(videoFile);
            }
            ActivityStauts = ActivityStauts.UploadVideo;
            return await CloudApi.UploadMatchVideo(guid, videoFile, TokenSource.Token);
        }

        public void CancelSync()
        {
            ffmpeg?.Stop();
            ffmpeg?.Abort();
            TokenSource.Cancel();
        }
        #endregion

        #region Download
        public async Task<Tuple<MatchMeta, string, string>> DownloadMatch(Guid matchId, string matchFilePath, string videoFilePath, CancellationToken token, Action<string> callback = null)
        {
            callback?.Invoke("Finding match...");
            var match = await CloudApi.GetMatch(matchId);

            if (match.AnalysisFileStatus)
            {
                callback?.Invoke("Downloading analysis file...");
                await CloudApi.DownloadFile(match.Guid, matchFilePath, token);
            } else { matchFilePath = null; }

            if (match.VideoStatus == VideoStatus.Ready)
            {
                callback?.Invoke("Downloading video... (this may take some time)");
                await CloudApi.DownloadVideo(match.Guid, videoFilePath, token);
            } else { videoFilePath = null; }

            token.ThrowIfCancellationRequested();

            return new Tuple<MatchMeta, string, string>(match, matchFilePath, videoFilePath);
        }
        #endregion

        #region Video Helper Functions
        public Task<bool> IsVideoValid(string fileName)
        {
            return Task.Run(() =>
            {
                var ffProbe = new FFProbe();
                var videoInfo = ffProbe.GetMediaInfo(fileName);
                if(videoInfo.Streams.Length >= 2)
                {
                    foreach(var stream in videoInfo.Streams)
                    {
                        if(stream.CodecType == "video" && stream.CodecName != "h264")
                        {
                            return false;
                        }
                        if(stream.CodecType == "audio" && stream.CodecName != "aac")
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }, cancellationToken: TokenSource.Token);
        }

        public Task<string> ConvertVideoFile(string fileName)
        {
            return Task.Run(() =>
            {
                string convertedFileName = Path.GetTempFileName();

                ffmpeg = new FFMpegConverter();
                ffmpeg.ConvertProgress += (sender, arg) =>
                {
                    UploadProgress = 100 * arg.Processed.Ticks / (double)arg.TotalDuration.Ticks;
                };
                ffmpeg.ConvertMedia(fileName, convertedFileName, Format.mp4);
                return convertedFileName;
            }, cancellationToken: TokenSource.Token);

        }
        #endregion
    }
}
