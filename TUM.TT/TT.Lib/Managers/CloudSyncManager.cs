using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using TT.Lib.Events;
using TT.Lib.Api;
using TT.Models;
using TT.Models.Api;
using Caliburn.Micro;
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
        [Description("No Match")]
        None,
        [Description("Match does not exist in Cloud")]
        NotExists,
        [Description("Local match is outdated")]
        Outdated,
        [Description("Match is in sync")]
        InSync,
        [Description("Match in cloud was changed by other user")]
        ChangedByOther
    }

    public class CloudSyncManager : ICloudSyncManager, IHandle<MatchOpenedEvent>
    {
        #region Properties
        private IEventAggregator EventAggregator { get; set; }
        public IMatchManager MatchManager { get; set; }
        public ConnectionStatus ConnectionStatus = ConnectionStatus.Offline;
        public string ConnectionMessage;
        private TTCloudApi CloudApi;

        private const string applicationName = "TUM.TT Cloud";
        #endregion

        public CloudSyncManager(IEventAggregator eventAggregator, IMatchManager matchManager)
        {
            EventAggregator = eventAggregator;
            this.MatchManager = matchManager;

            EventAggregator.Subscribe(this);
            SetCredentials("admin@example.com", "admin123");
        }

        public void SetCredentials(string email, string password)
        {
            WindowsCredentialManager.WriteCredentials(applicationName, email, password, (int) CredentialPersistence.LocalMachine);
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
            try
            {
                ConnectionStatus = ConnectionStatus.Connecting;
                string password = Credentials.GetSecret(System.Text.Encoding.Unicode.GetString);
                AccessToken = await TTCloudApi.Login(Credentials.UserName, password);
                ConnectionStatus = ConnectionStatus.Online;
                ConnectionMessage = String.Empty;

            } catch(TTCloudApiException e)
            {
                ConnectionMessage = e.Message;
                ConnectionStatus = ConnectionStatus.Offline;
            }

            CloudApi = new TTCloudApi(AccessToken);
            return AccessToken;
        }

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
            else if (MatchManager.Match.LastCloudSync < meta.UpdatedAt)
            {
                return SyncStatus.ChangedByOther;
            }
            else if (MatchManager.Match.LastCloudSync == meta.UpdatedAt)
            {
                if (MatchManager.MatchModified)
                {
                    return SyncStatus.Outdated;
                }
                return SyncStatus.InSync;
            }
            return SyncStatus.None;

        }

        public async void Handle(MatchOpenedEvent message)
        {
            MatchMeta meta;
            SyncStatus syncStatus;
            try
            {
                meta = await CloudApi.GetMatch(MatchManager.Match.ID);
                syncStatus = GetSyncStatus(meta);
            } catch  {
                syncStatus = SyncStatus.None;
            }
            Console.WriteLine(syncStatus);
            //UpsertMatchMeta();
            //ConvertVideoFile();
            //CloudApi.UploadMatchVideo(MatchMeta);
        }

        public void UpsertMatchMeta()
        {
            MatchMeta matchMeta = MatchMeta.fromMatch(MatchManager.Match);
            //CloudApi.PutMatch(matchMeta);
        }

        public Task<MatchMetaResult> GetMatches(string query=null)
        {
            return CloudApi.GetMatches(query);
        }

        public Task<MatchMeta> GetMatch(Guid id)
        {
            return CloudApi.GetMatch(id);
        }

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

        public void ConvertVideoFile(MatchMeta meta)
        {
            meta.ConvertedVideoFile = Path.GetTempFileName();
            Console.Write(meta.ConvertedVideoFile);

            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            NReco.VideoConverter.ConvertSettings settings = new NReco.VideoConverter.ConvertSettings()
            {
                VideoFrameSize = NReco.VideoConverter.FrameSize.hd480
            };
            ffMpeg.ConvertMedia(MatchManager.Match.VideoFile, null, meta.ConvertedVideoFile, NReco.VideoConverter.Format.mp4, settings);
        }
    }
}
