using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using LiteDB;
using TT.Lib.Events;
using TT.Lib.Api;
using TT.Models.Api;
using TT.Lib.Properties;
using Caliburn.Micro;

namespace TT.Lib.Managers
{


    public class MatchLibraryManager : IMatchLibraryManager, IHandle<MatchOpenedEvent>
    {
        #region Properties
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator EventAggregator { get; set; }
        public IMatchManager MatchManager { get; set; }

        private LiteDatabase db { get; }
        private string libraryPath { get; set; }
        #endregion

        #region Calculated Properties
        #endregion

        public MatchLibraryManager(IEventAggregator eventAggregator, IMatchManager matchManager)
        {
            EventAggregator = eventAggregator;
            this.MatchManager = matchManager;

            EventAggregator.Subscribe(this);

            libraryPath = Settings.Default.LocalLibraryPath;
            string libraryFile = Path.Combine(libraryPath, "Library.db");
            db = new LiteDatabase(libraryFile);
            initDb();
        }

        private void initDb()
        {
            var col = db.GetCollection<MatchMeta>("matches");
            col.EnsureIndex(x => x.Guid, true);
            col.EnsureIndex(x => x.LastOpenedAt, false);
        }

        public IEnumerable<MatchMeta> GetMatches(String query = null)
        {
            var col = db.GetCollection<MatchMeta>("matches");
            var results = col.Find(Query.All("LastOpenedAt", Query.Descending), 0, 100);

            return results;
        }

        public MatchMeta FindMatch(Guid guid)
        {
            var col = db.GetCollection<MatchMeta>("matches");
            return col.FindById(guid.ToString());
        }

        public void Handle(MatchOpenedEvent message)
        {
            var meta = MatchMeta.fromMatch(MatchManager.Match);
            meta.FileName = MatchManager.FileName;
            
            var col = db.GetCollection<MatchMeta>("matches");

            var oldMeta = col.FindById(meta._id);

            if (oldMeta != null) {
                oldMeta.LastOpenedAt = DateTime.Now;
                return;
            }

            if(MatchManager.Match.VideoFile != null)
            {
                ConvertVideoFile(MatchManager.Match.VideoFile, MatchManager.Match.ID.ToString());

            }
            col.Insert(meta);
        }

        public string ConvertVideoFile(string videoPath, string imageName)
        {
            var thumb = Path.Combine(libraryPath, imageName);

            var ffmpeg = new NReco.VideoConverter.FFMpegConverter();
            ffmpeg.GetVideoThumbnail(videoPath, thumb);
            return thumb;
        }

    }
}
