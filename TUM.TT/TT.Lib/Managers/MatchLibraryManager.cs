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
using TT.Lib.Util;
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

        private LiteDatabase db { get; set; }
        public string LibraryPath { get; private set; }
        public bool IsMovingFilesToLibrary { get; private set; }

        private const string matchesCollection = "matches";
        private const string libraryFileName = "Library.db";
        private const string analysisExtension = ".tto";
        private const string videoExtension = ".mp4";
        #endregion

        #region Calculated Properties
        #endregion

        public MatchLibraryManager(IEventAggregator eventAggregator, IMatchManager matchManager)
        {
            EventAggregator = eventAggregator;
            this.MatchManager = matchManager;
            EventAggregator.Subscribe(this);

            LibraryPath = Settings.Default.LocalLibraryPath;
            IsMovingFilesToLibrary = Settings.Default.IsMovingFilesToLibrary;
            InitDatabase();
        }

        #region Static Functions
        public static void GenerateThumbnail(string videoPath, string thumbPath)
        {
            TryDelteFile(thumbPath);

            var ffmpeg = new NReco.VideoConverter.FFMpegConverter();
            ffmpeg.GetVideoThumbnail(videoPath, thumbPath);
        }
        #endregion

        #region Path Generator
        public string GetMatchFilePath(MatchMeta match)
        {
            return Path.Combine(LibraryPath, MatchMetaExtensions.DefaultFilename(match) + analysisExtension);
        }

        public string GetVideoFilePath(MatchMeta match)
        {
            return Path.Combine(LibraryPath, MatchMetaExtensions.DefaultFilename(match) + videoExtension);
        }

        public string GetThumbnailPath(MatchMeta match)
        {
            return Path.Combine(LibraryPath, match.Guid.ToString());
        }
        #endregion

        #region Database scaffolding

        private void InitDatabase()
        {
            string libraryFile = Path.Combine(LibraryPath, libraryFileName);
            db = new LiteDatabase(libraryFile);
            var col = db.GetCollection<MatchMeta>(matchesCollection);
            col.EnsureIndex(x => x.Guid, true);
            col.EnsureIndex(x => x.LastOpenedAt, false);
        }

        public void ResetLibrary(bool deleteFiles = false)
        {
            if(deleteFiles) { 
                var matches = GetMatches(null, int.MaxValue);
                foreach(MatchMeta match in matches)
                {
                    TryDelteFile(match.FileName, LibraryPath);
                    TryDelteFile(match.VideoFileName, LibraryPath);
                    TryDelteFile(GetThumbnailPath(match), LibraryPath);
                }
            }
            db.DropCollection(matchesCollection);
        }
        #endregion

        #region Library Functions
        public IEnumerable<MatchMeta> GetMatches(String query = null, int limit=30)
        {
            var col = db.GetCollection<MatchMeta>(matchesCollection);
            IEnumerable<MatchMeta> results;
            if (query != null && query != String.Empty)
            {
                string[] squery = query.ToLower().Split(null);
                results = col
                    .Find(Query.All("LastOpenedAt", Query.Descending))
                    .Where(x => {
                        bool cond = true;
                        foreach (string q in squery)
                        {
                            cond = cond && (
                                x.Tournament.ToLower().Contains(q) ||
                                x.Category.ToLower().Contains(q) ||
                                x.FirstPlayer.Name.ToLower().Contains(q) ||
                                x.FirstPlayer.Nationality.ToLower().Contains(q) ||
                                x.SecondPlayer.Name.ToLower().Contains(q) ||
                                x.SecondPlayer.Nationality.ToLower().Contains(q)
                            );
                        }
                        return cond;
                    });
            }
            else
            {
                results = col.Find(Query.All("LastOpenedAt", Query.Descending), 0, limit);
            }

            return results;
        }

        public void DeleteMatch(Guid guid)
        {
            var col = db.GetCollection<MatchMeta>(matchesCollection);
            var match = col.FindById(guid.ToString());
            if (match != null) {
                col.Delete(match._id);
                TryDelteFile(match.FileName);
                TryDelteFile(match.VideoFileName);
                TryDelteFile(GetThumbnailPath(match));
            }
        }

        public MatchMeta FindMatch(Guid guid)
        {
            var col = db.GetCollection<MatchMeta>(matchesCollection);
            return col.FindById(guid.ToString());
        }
        #endregion

        #region Events
        public void Handle(MatchOpenedEvent message)
        {
            var meta = MatchMeta.fromMatch(MatchManager.Match);
            if (IsMovingFilesToLibrary)
            {
                MoveFilesToLibrary(meta);
            }

            meta.FileName = MatchManager.FileName;
            meta.VideoFileName = MatchManager.Match.VideoFile;
            
            var col = db.GetCollection<MatchMeta>(matchesCollection);

            var oldMeta = col.FindById(meta._id);

            if (oldMeta != null) {
                oldMeta.LastOpenedAt = DateTime.Now;
                return;
            }
            col.Insert(meta);

            // Try to generate thumbnail
            Task.Factory.StartNew(() =>
            {
                if (MatchManager.Match.VideoFile != null)
                {
                    try
                    {
                        GenerateThumbnail(MatchManager.Match.VideoFile, GetThumbnailPath(meta));
                    } catch { /* best effort */ }

                }
            });
        }
        #endregion

        #region Helper
        private void MoveFilesToLibrary(MatchMeta meta)
        {
            if(MatchManager.FileName.IsSubPathOf(LibraryPath) && MatchManager.Match.VideoFile.IsSubPathOf(LibraryPath))
            {
                return;
            }

            // Move video
            if(!MatchManager.Match.VideoFile.IsSubPathOf(LibraryPath))
            {
                try
                {
                    string newVideoFile = GetVideoFilePath(meta);
                    File.Copy(MatchManager.Match.VideoFile, newVideoFile);
                    MatchManager.Match.VideoFile = newVideoFile;
                }
                catch { }
            }

            // Move File
            if(!MatchManager.FileName.IsSubPathOf(LibraryPath))
            {
                MatchManager.FileName = GetMatchFilePath(meta);
            }

            Coroutine.ExecuteAsync(MatchManager.SaveMatch().GetEnumerator());
        }

        static private void TryDelteFile(string Path, string SubPath)
        {
            try
            {
                if (File.Exists(Path) && Path.IsSubPathOf(SubPath))
                {
                    File.Delete(Path);
                }
            }
            catch { /* Best effort */ }
        }
        #endregion
    }
}
