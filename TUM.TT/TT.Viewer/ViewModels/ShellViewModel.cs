using Caliburn.Micro;
using System.Windows;
using System.Reflection;
using TT.Lib.Results;
using TT.Lib.Util;
using TT.Viewer.Events;
using System.Collections.Generic;
using TT.Lib.Models;
using System.IO;
using TTA.Results;
using MahApps.Metro.Controls.Dialogs;

namespace TT.Viewer.ViewModels
{

    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<MatchEditedEvent>,
        IHandle<SaveMatchEvent>,
        IShell
    {
        public MediaViewModel MediaView { get; private set; }
        public FilterViewModel FilterView { get; private set; }
        public ResultViewModel ResultView { get; private set; }
        public PlaylistViewModel PlaylistView { get; private set; }
        public string SaveFileName { get; private set; }
        public Match Match { get; private set; }
        public bool IsModified { get; private set; }

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
            this.ActivateItem(PlaylistView);
        }

        protected override void OnDeactivate(bool close)
        {
            if (IsModified)
            {
                //var mySettings = new MetroDialogSettings()
                //{
                //    AffirmativeButtonText = "Save and Quit",
                //    NegativeButtonText = "Cancel",
                //    FirstAuxiliaryButtonText = "Quit Without Saving",
                //    AnimateShow = true,
                //    AnimateHide = false
                //};

                //var result = await DialogManager.ShowMessageAsync(this, "Quit application?",
                //    "Sure you want to quit application?",
                //    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                //bool _shutdown = result == MessageDialogResult.Affirmative;

                //if (_shutdown)
                //    Application.Current.Shutdown();
            }
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

            SaveFileName = dialog.Result;
            Match = deserialization.Result;

            this.Events.PublishOnUIThread(new MatchOpenedEvent(Match, deserialization.FileName));

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
            this.Events.PublishOnUIThread(new VideoLoadedEvent(Match.VideoFile));
        }

        public IEnumerable<IResult> SaveMatch()
        {
            var fileName = this.SaveFileName;
            if (fileName == null)
            {
                var dialog = new SaveFileDialogResult()
                {
                    Title = string.Format("Save match \"{0}\"...", this.Match.Title()),
                    Filter = Format.XML.DialogFilter,
                    DefaultFileName = this.Match.DefaultFilename(),
                };
                yield return dialog;
                fileName = dialog.Result;
            }

            var serialization = new SerializeMatchResult(this.Match, fileName, Format.XML.Serializer);
            yield return serialization
                .IsBusy("Saving")
                .Rescue()
                .WithMessage("Error saving the match", string.Format("Could not save the match to {0}.", fileName))
                .Propagate(); // Reraise the error to abort the coroutine

            this.SaveFileName = fileName;
            this.IsModified = false;

        }

        #endregion

        #region Event Handlers

        public void Handle(MatchEditedEvent message)
        {
            IsModified = true;
            this.Match = message.Match;
        }

        public void Handle(SaveMatchEvent message)
        {
            if (IsModified)
            {
                //this.Match = message.Match;
                Coroutine.BeginExecute(SaveMatch().GetEnumerator(), 
                    new CoroutineExecutionContext() { View = this.GetView(), Target = this });
            }
        }

        #endregion       
    }
}
