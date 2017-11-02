using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TT.Lib.Events;
using TT.Lib.Api;
using TT.Models.Api;
using Caliburn.Micro;

namespace TT.Lib.Managers
{

    public enum SyncStatus
    {
        None,
        NotExists,
        Outdated,
        InSync,
        ChangedByOther
    }

    public class CloudSyncManager : ICloudSyncManager, IHandle<MatchOpenedEvent>
    {
        #region Properties
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator EventAggregator { get; set; }

        public IMatchManager MatchManager { get; set; }
        #endregion
        private string AccessToken;
        private TTCloudApi CloudApi;

        #region Calculated Properties

        private User _loggedInUser;
        public User LoggedInUser
        {
            get
            {
                if (_loggedInUser == null)
                {
                    //_loggedInUser = CloudApi.GetCurrentUser();
                }
                return _loggedInUser;
            }
        }

        private MatchMeta _matchMeta;
        public MatchMeta MatchMeta
        {
            get
            {
                if (_matchMeta == null)
                {
                    //_matchMeta = CloudApi.GetMatch(MatchManager.Match.ID);
                }
                return _matchMeta;
            }
        }

        public SyncStatus SyncStatus
        {
            get
            {
                if (MatchMeta == null)
                {
                    return SyncStatus.NotExists;
                }
                else if (MatchManager.Match.LastCloudSync < MatchMeta.UpdatedAt)
                {
                    return SyncStatus.ChangedByOther;
                }
                else if (MatchManager.Match.LastCloudSync == MatchMeta.UpdatedAt)
                {
                    if (MatchManager.MatchModified)
                    {
                        return SyncStatus.Outdated;
                    }
                    return SyncStatus.InSync;
                }
                return SyncStatus.None;
            }
        }

        #endregion

        public CloudSyncManager(IEventAggregator eventAggregator, IMatchManager matchManager)
        {
            EventAggregator = eventAggregator;
            this.MatchManager = matchManager;

            EventAggregator.Subscribe(this);
        }

        public async void Login()
        {
            AccessToken = await TTCloudApi.Login("admin@example.com", "admin");
            CloudApi = new TTCloudApi(AccessToken);
        }

        public void Handle(MatchOpenedEvent message)
        {
            Console.WriteLine(SyncStatus);
            //UpsertMatchMeta();
            //ConvertVideoFile();
            //CloudApi.UploadMatchVideo(MatchMeta);
        }

        public void UpsertMatchMeta()
        {
            MatchMeta matchMeta = MatchMeta.fromMatch(MatchManager.Match);
            //CloudApi.PutMatch(matchMeta);
        }

        public Task<MatchMetaResult> GetMatches()
        {
            return CloudApi.GetMatches();
        }

        public void ConvertVideoFile()
        {
            MatchMeta.ConvertedVideoFile = Path.GetTempFileName();
            Console.Write(MatchMeta.ConvertedVideoFile);

            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            NReco.VideoConverter.ConvertSettings settings = new NReco.VideoConverter.ConvertSettings()
            {
                VideoFrameSize = NReco.VideoConverter.FrameSize.hd480
            };
            ffMpeg.ConvertMedia(MatchManager.Match.VideoFile, null, MatchMeta.ConvertedVideoFile, NReco.VideoConverter.Format.mp4, settings);
        }
    }
}
