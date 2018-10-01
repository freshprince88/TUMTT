using Caliburn.Micro;
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

namespace TT.Lib.Managers
{

    public class MatchLibraryManager : PropertyChangedBase, IMatchLibraryManager, IHandle<MatchOpenedEvent>
    {
        #region Properties
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator EventAggregator { get; set; }
        public IMatchManager MatchManager { get; set; }

        private LiteDatabase db { get; set; }

        private const string matchesCollection = "matches";
        private const string libraryDirectoryName = "TUM.TT Library";
        private const string libraryFileName = "Library.db";
        private const string analysisExtension = ".tto";
        private const string videoExtension = ".mp4";
        #endregion

        #region Calculated Properties
        public string LibraryPath {
            get
            {
                return Settings.Default.LocalLibraryPath;
            }
            set
            {
                if (Settings.Default.LocalLibraryPath != value)
                {
                    Settings.Default.LocalLibraryPath = value;
                    Settings.Default.Save();
                    InitDatabase(false);
                    
                    NotifyOfPropertyChange("LibraryPath");
                }
            }
        }

        public bool IsMovingFilesToLibrary
        {
            get
            {
                return Settings.Default.IsMovingFilesToLibrary;
            }
            set
            {
                if (Settings.Default.IsMovingFilesToLibrary != value)
                {
                    Settings.Default.IsMovingFilesToLibrary = value;
                    Settings.Default.Save();
              
                    NotifyOfPropertyChange("IsMovingFilesToLibrary");
                }
            }
        }

        public bool Uninitialized
        {
            get
            {
                return db == null;
            }
        }
        #endregion

        public MatchLibraryManager(IEventAggregator eventAggregator, IMatchManager matchManager)
        {
            EventAggregator = eventAggregator;
            this.MatchManager = matchManager;
            EventAggregator.Subscribe(this);
            
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
        public void InitDatabase(bool ValidateFileExistence = true)
        {
            if(!ValidateLibraryPath(ValidateFileExistence)) {
                return;
            }
            string libraryFile = Path.Combine(LibraryPath, libraryFileName);
            db = new LiteDatabase(libraryFile);
            var col = db.GetCollection<MatchMeta>(matchesCollection);
            col.EnsureIndex(x => x.Guid, true);
            col.EnsureIndex(x => x.LastOpenedAt, false);
        }

        private bool ValidateLibraryPath(bool CheckExistence = true)
        {
            if(String.IsNullOrEmpty(LibraryPath))
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var defaultibraryPath = Path.Combine(documentsPath, libraryDirectoryName);
                if (!Directory.Exists(defaultibraryPath))
                {
                    Directory.CreateDirectory(defaultibraryPath);
                }
                LibraryPath = defaultibraryPath;
                return true;
            }

            string libraryFile = Path.Combine(LibraryPath, libraryFileName);
            if (!Directory.Exists(LibraryPath) || (CheckExistence && !File.Exists(libraryFile)))
            {
                return false;
            }

            return true;
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
        public IEnumerable<MatchMeta> GetMatches(String query = null, int limit=100)
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
                TryDelteFile(match.FileName, LibraryPath);
                TryDelteFile(match.VideoFileName, LibraryPath);
                TryDelteFile(GetThumbnailPath(match), LibraryPath);
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
            var meta = MatchMeta.FromMatch(MatchManager.Match);
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

        static private void TryDelteFile(string Path, string SubPath = null)
        {
            try
            {
                bool SubPathCond = (SubPath != null) ? Path.IsSubPathOf(SubPath) : true;
                if (File.Exists(Path) && SubPathCond)
                {
                    File.Delete(Path);
                }
            }
            catch { /* Best effort */ }
        }
        #endregion
    }
}
