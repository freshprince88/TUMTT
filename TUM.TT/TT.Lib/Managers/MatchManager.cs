using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TT.Lib.Events;
using TT.Lib.Models;
using TT.Lib.Results;
using TT.Lib.Util;

namespace TT.Lib.Managers
{
    public class MatchManager : IMatchManager
    {
        #region Properties
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator Events { get; set; }

        public string FileName { get; set; }

        private Match _match;
        public Match Match
        {
            get { return _match; }
            set
            {
                if (_match != value)
                {
                    _match = value;
                    MatchModified = true;
                }
            }
        }

        public bool MatchModified { get; set; }

        private Playlist _activeList;
        public Playlist ActivePlaylist
        {
            get { return _activeList; }
            set
            {
                if (_activeList != value)
                {
                    _activeList = value;
                    Events.PublishOnUIThread(new PlaylistSelectionChangedEvent());
                }
            }
        }

        #endregion

        public MatchManager(IEventAggregator aggregator)
        {
            Events = aggregator;
        }

        #region Business Logic

        public IEnumerable<IResult> SaveMatch()
        {
            var fileName = this.FileName;
            if (fileName == null)
            {
                var dialog = new SaveFileDialogResult()
                {
                    Title = string.Format("Save match \"{0}\"...", Match.Title()),
                    Filter = Format.XML.DialogFilter,
                    DefaultFileName = Match.DefaultFilename(),
                };
                yield return dialog;
                fileName = dialog.Result;
            }

            var serialization = new SerializeMatchResult(Match, fileName, Format.XML.Serializer);
            yield return serialization
                .Rescue()
                .WithMessage("Error saving the match", string.Format("Could not save the match to {0}.", fileName))
                .Propagate(); // Reraise the error to abort the coroutine

            FileName = fileName;
            MatchModified = false;

        }

        public IEnumerable<IResult> OpenMatch()
        {
            var dialog = new OpenFileDialogResult()
            {
                Title = "Open match...",
                Filter = Format.XML.DialogFilter,
            };
            yield return dialog;

            var deserialization = new DeserializeMatchResult(dialog.Result, Format.XML.Serializer);
            yield return deserialization
                .Rescue()
                .WithMessage("Error loading the match", string.Format("Could not load a match from {0}.", dialog.Result))
                .Propagate(); // Reraise the error to abort the coroutine

            FileName = dialog.Result;
            Match = deserialization.Result;
            ActivePlaylist = Match.Playlists.Where(p => p.Name == "Alle").FirstOrDefault();

            if (string.IsNullOrEmpty(Match.VideoFile) || !File.Exists(Match.VideoFile))
            {
                var videoDialog = new OpenFileDialogResult()
                {
                    Title = "Open video file...",
                    Filter = string.Format("{0}|{1}", "Video Files", "*.mp4; *.wmv; *.avi; *.mov")
                };
                yield return videoDialog;

                Match.VideoFile = videoDialog.Result;
            }

            Events.PublishOnUIThread(new MatchOpenedEvent(Match));
            Events.PublishOnUIThread(new VideoLoadedEvent(Match.VideoFile));
        }

        public void DeleteRally(Rally r)
        {
            if (ActivePlaylist.Name != "Alle")
            {
                ActivePlaylist.Rallies.Remove(r);
                Events.PublishOnUIThread(new PlaylistSelectionChangedEvent());
                Events.PublishOnUIThread(new PlaylistChangedEvent(ActivePlaylist));
            }
        }

        public void RenamePlaylist(string oldName, string newName)
        {
            if(oldName != "Alle")
            {
                Playlist list = Match.Playlists.Where(p => p.Name == oldName).FirstOrDefault();

                if(list != null)
                {
                    list.Name = newName;
                    Events.PublishOnUIThread(new PlaylistChangedEvent(ActivePlaylist));
                }
            }
        }

        public void CreateNewMatch()
        {
            this.Match = new Match();
            this.ActivePlaylist = null;
            this.FileName = String.Empty;
            this.MatchModified = false;
        }

        public OpenFileDialogResult LoadVideo()
        {
            var videoDialog = new OpenFileDialogResult()
            {
                Title = "Open video file...",
                Filter = string.Format("{0}|{1}", "Video Files", "*.mp4; *.wmv; *.avi; *.mov")
            };
            return videoDialog;            
        }

        #endregion
    }
}
