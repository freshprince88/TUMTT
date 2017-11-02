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
using TT.Models.Util.Enums;
using System.Windows.Threading;

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
                    //MatchModified = _match == null ? false : true;
                    _match = value;                    
                    NotifyOfPropertyChange("MatchModified");
                    NotifyOfPropertyChange();

                }
            }
        }
        private bool _matchSaveAs;
        public bool MatchSaveAs
        {
            get { return _matchSaveAs; }
            set
            {
                if (_matchSaveAs != value)
                {
                    _matchSaveAs = value;
                    NotifyOfPropertyChange("MatchSaveAs");
                    NotifyOfPropertyChange();

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
                    NotifyOfPropertyChange();

                }
            }
        }
        private bool _matchExportExcel;
        public bool MatchExportExcel
        {
            get { return _matchExportExcel; }
            set
            {
                if (_matchExportExcel != value)
                {
                    _matchExportExcel = value;
                    NotifyOfPropertyChange("MatchExportExcel");
                    NotifyOfPropertyChange();

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

        private Rally _activeRally;
        public Rally ActiveRally
        {
            get { return _activeRally; }
            set
            {
                if (_activeRally != value)
                {
                    _activeRally = value;
                    NotifyOfPropertyChange();
                    Events.PublishOnUIThread(new ActiveRallyChangedEvent()
                    {
                        Current = _activeRally
                    });
                }
            }
        }

        private int _currentRallyLength;
        public int CurrentRallyLength
        {
            get { return _currentRallyLength; }
            set
            {
                if (_currentRallyLength != value)
                {
                    _currentRallyLength = value;
                    NotifyOfPropertyChange();
                    Events.PublishOnUIThread(new RallyLengthChangedEvent(value));
                }
            }
        }

        private IEnumerable<Rally> _selected;
        public IEnumerable<Rally> SelectedRallies
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    NotifyOfPropertyChange();
                    Events.PublishOnUIThread(new ResultsChangedEvent(_selected));
                }
            }
        }

        #endregion

        #region CalculatedProperties
        public CurrentTableEnd CurrentTableEndFirstPlayer
        {
            get
            {
                // Return none if StartingTableEnd = None
                if (Match.FirstPlayer.StartingTableEnd == StartingTableEnd.None) return CurrentTableEnd.None;

                // Works ONLY if StartingTableEnd.Top == 1 && StartingTableEnd.Bottom == 2!!!
                if ((int)StartingTableEnd.Top != 1 || (int)StartingTableEnd.Bottom != 2 || (int)CurrentTableEnd.Top != 1 || (int)CurrentTableEnd.Bottom != 2)
                    throw new System.Exception("That doesnt work anymore if you change the values of the enums for StartingTableEnd");

                int magicResult = ((((int)Match.FirstPlayer.StartingTableEnd + (ActiveRally.CurrentSetScore.Total % 2)) + 1) % 2) + 1;

                // If its the last set calculation is more complicated
                bool isLastSet = ((MatchModeExtensions.RequiredSets(Match.Mode) - 1) * 2) == ActiveRally.CurrentSetScore.Total;
                if (isLastSet)
                {
                    if (ActiveRally.CurrentRallyScore.Highest < 5)
                        magicResult = ((((int)Match.FirstPlayer.StartingTableEnd + (ActiveRally.CurrentSetScore.Total % 2)) + 1) % 2) + 1;
                    else
                        magicResult = (((((int)Match.FirstPlayer.StartingTableEnd + 1) % 2 + (ActiveRally.CurrentSetScore.Total % 2)) + 1) % 2) + 1;
                }
                return ((CurrentTableEnd)magicResult);
            }
        }

        public CurrentTableEnd CurrentTableEndSecondPlayer
        {
            get
            {
                switch(CurrentTableEndFirstPlayer)
                {
                    case CurrentTableEnd.None:
                        return CurrentTableEnd.None;
                    case CurrentTableEnd.Top:
                        return CurrentTableEnd.Bottom;
                    case CurrentTableEnd.Bottom:
                        return CurrentTableEnd.Top;
                }
                throw new Exception("This code should not be reachable");
            }
        }
        #endregion

        public MatchManager(IEventAggregator aggregator, Match m)
        {
            Events = aggregator;
            this.Match = m;
            CurrentRallyLength = 1;
            SelectedRallies = new List<Rally>();
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
                MatchSaveAs = true;
                NotifyOfPropertyChange("MatchSaveAs");
            }

            //Remove Dummy Rally from Scouter
            var lastRally = Match.Rallies.LastOrDefault();
            bool haveToAddAgain = false;
            if (Match.Rallies.Any())
            {
                if (lastRally.Winner == MatchPlayer.None)
                {
                    Match.Rallies.Remove(lastRally);
                    haveToAddAgain = true;
                }
            }
            var serialization = new SerializeMatchResult(Match, FileName, Format.XML.Serializer);
            yield return serialization
                .Rescue()
                .WithMessage("Error saving the match", string.Format("Could not save the match to {0}.", FileName))
                .Propagate(); // Reraise the error to abort the coroutine

            if (haveToAddAgain)
            {
                Execute.OnUIThread((System.Action)(() => Match.Rallies.Add(lastRally)));
            }
            MatchModified = false;
            NotifyOfPropertyChange("MatchModified");
        }

        public IEnumerable<IResult> SaveMatchAs()
        {
            var dialog = new SaveFileDialogResult()
            {
                Title = string.Format("Save match \"{0}\"...", Match.Title()),
                Filter = Format.XML.DialogFilter,
                DefaultFileName = FileName.Split('\\').Last(),
            };
            yield return dialog;
            FileName = dialog.Result;


            //Remove Dummy Rally from Scouter
            var lastRally = Match.Rallies.LastOrDefault();
            bool haveToAddAgain = false;
            if (Match.Rallies.Any())
            {
                if (lastRally.Winner == MatchPlayer.None)
                {
                    Match.Rallies.Remove(lastRally);
                    haveToAddAgain = true;
                }
            }

            var serialization = new SerializeMatchResult(Match, FileName, Format.XML.Serializer);
            yield return serialization
                .Rescue()
                .WithMessage("Error saving the match", string.Format("Could not save the match to {0}.", FileName))
                .Propagate(); // Reraise the error to abort the coroutine

            if (haveToAddAgain)
            {
                Execute.OnUIThread((System.Action)(() => Match.Rallies.Add(lastRally)));
            }

            MatchModified = false;
            NotifyOfPropertyChange("MatchModified");

        }

        public IEnumerable<IResult> ExportExcel()
        {
            // yield return new SequentialResult(this.SwapPlayersIfNecessary().GetEnumerator());

            var dialog = new SaveFileDialogResult()
            {
                Title = string.Format("Export match \"{0}\" to Excel...", Match.Title()),
                Filter = Format.Excel.DialogFilter,
                DefaultFileName = Match.DefaultFilename(),
            };
            yield return dialog;

            var fileName = dialog.Result;

            var serialization = new SerializeMatchResult(Match, fileName, Format.Excel.Serializer);
            yield return serialization
               .Rescue()
               .WithMessage("Error exporting the match", string.Format("Could not export the match to {0}.", FileName))
               .Propagate(); // Reraise the error to abort the coroutine


            //yield return new SerializeMatchResult(Match, FileName, Format.Excel.Serializer)
            //    //.IsBusy("Exporting")
            //    .Rescue()
            //    .WithMessage("Error exporting the match", string.Format("Could not export the match to {0}.", FileName))
            //    .Propagate();
        }

        public IEnumerable<IResult> DownloadMatch()
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
            NotifyOfPropertyChange("MatchModified");

        }

        public IEnumerable<IResult> OpenMatch(string fileName = null)
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Stop, Media.Source.Viewer));
            bool newVideoLoaded = false;

            var dialog = new OpenFileDialogResult()
            {
                Title = "Open match...",
                Filter = Format.XML.DialogFilter,
            };
            if (fileName == null || !File.Exists(fileName))
            {
                yield return dialog;
                FileName = dialog.Result;
            }
            else
                FileName = fileName;

            var deserialization = new DeserializeMatchResult(FileName, Format.XML.Serializer);
            yield return deserialization
                .Rescue()
                .WithMessage("Error loading the match", string.Format("Could not load a match from {0}.", dialog.Result))
                .Propagate(); // Reraise the error to abort the coroutine            

            var tempMatch = deserialization.Result;

            if (string.IsNullOrEmpty(tempMatch.VideoFile) || !File.Exists(tempMatch.VideoFile))
            {
                foreach (var result in LoadVideo(tempMatch))
                {
                    yield return result;
                }
                newVideoLoaded = true;
            }
            else
            {
                Events.PublishOnUIThread(new VideoLoadedEvent(tempMatch.VideoFile));
                newVideoLoaded = false;
            }

            this.Match = tempMatch;
            if (!newVideoLoaded)
            {
                MatchModified = false;
            }
            MatchSaveAs = true;
            NotifyOfPropertyChange("MatchSaveAs");
            ActivePlaylist = Match.Playlists.Where(p => p.Name == "Alle").FirstOrDefault();
            Events.PublishOnUIThread(new MatchOpenedEvent(Match));
            Events.PublishOnUIThread(new HideMenuEvent());
            //Events.PublishOnUIThread(new FullscreenEvent(false));
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
            Events.PublishOnUIThread(new HideMenuEvent());
            MatchModified = false;
            MatchSaveAs = true;
            NotifyOfPropertyChange("MatchSaveAs");
            NotifyOfPropertyChange("MatchModified");

        }




        public void DeleteRally(Rally r)
        {
            if (ActivePlaylist.Name != "Alle")
            {
                bool success = ActivePlaylist.RallyIDs.Remove(r.ID);
                Events.PublishOnUIThread(new PlaylistSelectionChangedEvent());
                Events.PublishOnUIThread(new PlaylistChangedEvent(ActivePlaylist));
                MatchModified = true;
            }
        }

        public void RenamePlaylist(string oldName, string newName)
        {
            if (oldName != "Alle" && oldName != "Markiert")
            {
                Playlist list = Match.Playlists.Where(p => p.Name == oldName).FirstOrDefault();

                if (list != null)
                {
                    Match.Playlists.Where(p => p.Name == oldName).FirstOrDefault().Name = newName;
                    ActivePlaylist = Match.Playlists.Where(p => p.Name == newName).FirstOrDefault();
                    Events.PublishOnUIThread(new PlaylistChangedEvent(ActivePlaylist));
                    MatchModified = true;

                }
            }
        }

        public void CreateNewMatch()
        {
            this.Match = new Match();
            Match.DateTime = DateTime.Now;
            Match.FirstPlayer = new Player(1);
            Match.SecondPlayer = new Player(2);
            Match.Playlists.Add(new Playlist(Match) { Name = "Alle" });
            Match.Playlists.Add(new Playlist(Match) { Name = "Markiert" });
            this.ActivePlaylist = this.Match.Playlists.Where(p => p.Name == "Alle").FirstOrDefault();
            this.FileName = String.Empty;
            this.MatchModified = false;
        }

        private IEnumerable<IResult> LoadVideo(Match temp)
        {
            var videoDialog = new OpenFileDialogResult()
            {
                Title = "Open video file...",
                Filter = string.Format("{0}|{1}", "Video Files", "*.mp4; *.wmv; *.avi; *.mov")
            };
            yield return videoDialog;
            MatchModified = videoDialog.Result != temp.VideoFile;
            temp.VideoFile = videoDialog.Result;
            Events.PublishOnUIThread(new VideoLoadedEvent(temp.VideoFile));
        }

        public IEnumerable<IResult> LoadVideo()
        {
            var videoDialog = new OpenFileDialogResult()
            {
                Title = "Open video file...",
                Filter = string.Format("{0}|{1}", "Video Files", "*.mp4; *.wmv; *.avi; *.mov")
            };
            yield return videoDialog;
            MatchModified = videoDialog.Result != Match.VideoFile;
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
        public IEnumerable<IResult> GenerateReport(string type)
        {
            var dialog = new SaveFileDialogResult()
            {
                Title = "Choose a target for PDF report",
                Filter = "PDF reports|*.pdf",
                DefaultFileName = this.Match.DefaultFilename(),
            };
            yield return dialog;

            var fileName = dialog.Result;

            yield return new GenerateReportResult(this.Match, fileName, type)
                .Rescue()
                .WithMessage("Error generating report", string.Format("Could not save the report to {0}.", fileName))
                .Propagate();
        }

        #endregion

    }
}
