using Caliburn.Micro;
using System.Windows;
using System.Reflection;
using TT.Lib.Results;
using TT.Lib.Util;
using TT.Viewer.Events;
using System.Collections.Generic;
using TT.Lib.Models;
using System.IO;

namespace TT.Viewer.ViewModels
{

    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive,
        IShell
    {
        public MediaViewModel MediaView { get; private set; }
        public FilterViewModel FilterView { get; private set; }
        public ResultViewModel ResultView { get; private set; }
        public PlaylistViewModel PlaylistView { get; private set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }

        public ShellViewModel(IEventAggregator eventAggregator, IEnumerable<IResultViewTabItem> resultTabs)
        {
            this.DisplayName = "TUM.TT";
            Events = eventAggregator;
            FilterView = new FilterViewModel(Events);
            MediaView = new MediaViewModel(Events);
            ResultView = new ResultViewModel(resultTabs);
            PlaylistView = new PlaylistViewModel(Events);
        }

        #region Caliburn hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Subscribe ourself to the event bus
            //this.Events.Subscribe(this);        
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Events.Subscribe(this);
            this.ActivateItem(FilterView);
            this.ActivateItem(MediaView);
            this.ActivateItem(ResultView);
        }

        #endregion

        #region Methods

        public IEnumerable<IResult> OpenNewMatch()
        {
            var dialog = new OpenFileDialogResult()
            {
                Title = "Open match...",
                Filter = Format.XML.DialogFilter,
            };
            yield return dialog;

            var deserialization = new DeserializeMatchResult(dialog.Result, Format.XML.Serializer);
            yield return deserialization
                .IsBusy("Loading")
                .Rescue()
                .WithMessage("Error loading the match", string.Format("Could not load a match from {0}.", dialog.Result))
                .Propagate(); // Reraise the error to abort the coroutine

            Match match = deserialization.Result;

            this.Events.PublishOnUIThread(new MatchOpenedEvent(match, deserialization.FileName));

            if(string.IsNullOrEmpty(match.VideoFile) || !File.Exists(match.VideoFile)){
                var videoDialog = new OpenFileDialogResult()
                {
                    Title = "Open video file...",
                    Filter = string.Format("{0}|{1}", "Video Files", "*.mp4; *.wmv; *.avi; *.mov")
                };
                yield return videoDialog;

                match.VideoFile = videoDialog.Result;
                this.Events.PublishOnUIThread(new VideoLoadedEvent(match.VideoFile));
            }            
            
        }

        #endregion

        #region Event Handlers

        #endregion
    }
}
