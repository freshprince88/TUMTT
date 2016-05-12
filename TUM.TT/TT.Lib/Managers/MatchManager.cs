using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TT.Lib.Events;
using TT.Models;
using TT.Lib.Results;
using TT.Lib.Util;
using MahApps.Metro.Controls.Dialogs;

using System.Windows;
using TT.Lib;


namespace TT.Lib.Managers
{
    public class MatchManager : Caliburn.Micro.PropertyChangedBase, IMatchManager 
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
                    NotifyOfPropertyChange("MatchModified");

                }
            }
        }
        private bool _matchMod;
        public bool MatchModified
        {
            get { return _matchMod; }
            set
            {
                if (_matchMod != value)
                {
                    _matchMod = value;
                    NotifyOfPropertyChange("MatchModified");

                }
            }
        }
        private string _activeList;
        public Playlist ActivePlaylist
        {
            get { return Match.Playlists.Where(p => p.Name == _activeList).FirstOrDefault(); }
            set
            {
                string pName = value.Name;
                if (_activeList != pName)
                {
                    _activeList = pName;
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
            if (FileName == null || FileName == string.Empty)
            {
                var dialog = new SaveFileDialogResult()
                {
                    Title = string.Format("Save match \"{0}\"...", Match.Title()),
                    Filter = Format.XML.DialogFilter,
                    DefaultFileName = Match.DefaultFilename(),
                };
                yield return dialog;
                FileName = dialog.Result;
            }

            var serialization = new SerializeMatchResult(Match, FileName, Format.XML.Serializer);
            yield return serialization
                .Rescue()
                .WithMessage("Error saving the match", string.Format("Could not save the match to {0}.", FileName))
                .Propagate(); // Reraise the error to abort the coroutine

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
            FileName = dialog.Result;

            var deserialization = new DeserializeMatchResult(FileName, Format.XML.Serializer);
            yield return deserialization
                .Rescue()
                .WithMessage("Error loading the match", string.Format("Could not load a match from {0}.", dialog.Result))
                .Propagate(); // Reraise the error to abort the coroutine

            Match = deserialization.Result;
            ActivePlaylist = Match.Playlists.Where(p => p.Name == "Alle").FirstOrDefault();
            Events.PublishOnUIThread(new MatchOpenedEvent(Match));

            if (string.IsNullOrEmpty(Match.VideoFile) || !File.Exists(Match.VideoFile))
            {
                foreach (var result in LoadVideo())
                {
                    yield return result;
                }
            }
            else
            {
                Events.PublishOnUIThread(new VideoLoadedEvent(Match.VideoFile));
                MatchModified = false;
            }
        }
        public IEnumerable<IResult> OpenLiveMatch()
        {
            var dialog = new OpenFileDialogResult()
            {
                Title = "Open match...",
                Filter = Format.XML.DialogFilter,
            };
            yield return dialog;
            FileName = dialog.Result;

            var deserialization = new DeserializeMatchResult(FileName, Format.XML.Serializer);
            yield return deserialization
                .Rescue()
                .WithMessage("Error loading the match", string.Format("Could not load a match from {0}.", dialog.Result))
                .Propagate(); // Reraise the error to abort the coroutine

            Match = deserialization.Result;
            ActivePlaylist = Match.Playlists.Where(p => p.Name == "Alle").FirstOrDefault();

            Events.PublishOnUIThread(new MatchOpenedEvent(Match));


        }

        public void DeleteRally(Rally r) //Immer noch die schon gelöschte Rally als Parameter!!!!
        {
            if (ActivePlaylist.Name != "Alle")
            {
                Boolean test;
                test = ActivePlaylist.Rallies.Remove(r);
                Playlist test2 = ActivePlaylist;
                Events.PublishOnUIThread(new PlaylistSelectionChangedEvent());
                Events.PublishOnUIThread(new PlaylistChangedEvent(ActivePlaylist));
                MatchModified = true;
                

            }
        }

        public void RenamePlaylist(string oldName, string newName)
        {
            if (oldName != "Alle")
            {
                Playlist list = Match.Playlists.Where(p => p.Name == oldName).FirstOrDefault();

                if (list != null)
                {
                    list.Name = newName;
                    Events.PublishOnUIThread(new PlaylistChangedEvent(ActivePlaylist));
                    MatchModified = true;
                    
                }
            }
        }

        public void CreateNewMatch()
        {
            this.Match = new Match();
            Match.DateTime = DateTime.Now;
            Match.FirstPlayer = new Player();
            Match.SecondPlayer = new Player();
            Match.Playlists.Add(new Playlist() { Name = "Alle", Match = Match });
            Match.Playlists.Add(new Playlist() { Name = "Markiert", Match = Match });
            this.ActivePlaylist = this.Match.Playlists.Where(p => p.Name == "Alle").FirstOrDefault();
            this.FileName = String.Empty;
            this.MatchModified = false;
        }

        public IEnumerable<IResult> LoadVideo()
        {
            var videoDialog = new OpenFileDialogResult()
            {
                Title = "Open video file...",
                Filter = string.Format("{0}|{1}", "Video Files", "*.mp4; *.wmv; *.avi; *.mov")
            };
            yield return videoDialog;
            Match.VideoFile = videoDialog.Result;
            Events.PublishOnUIThread(new VideoLoadedEvent(Match.VideoFile));
        }

        public MatchPlayer ConvertPlayer(Player p)
        {
            return p.Name == this.Match.FirstPlayer.Name ? MatchPlayer.First : MatchPlayer.Second;
        }

        /// <summary>
        /// Generates a PDF report.
        /// </summary>
        /// <returns>The actions to generate the report.</returns>
        public IEnumerable<IResult> GenerateReport()
        {
            var dialog = new SaveFileDialogResult()
            {
                Title = "Choose a target for PDF report",
                Filter = "PDF reports|*.pdf",
                DefaultFileName = this.Match.DefaultFilename(),
            };
            yield return dialog;

            var fileName = dialog.Result;

            yield return new GenerateReportResult(this.Match, fileName)
                .Rescue()
                .WithMessage("Error generating report", string.Format("Could not save the report to {0}.", fileName))
                .Propagate();
        }

        #endregion
    }
}
