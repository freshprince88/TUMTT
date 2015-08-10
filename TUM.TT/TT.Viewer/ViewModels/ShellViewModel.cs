using Caliburn.Micro;
using System.Windows;
using System.Reflection;
using TT.Lib.Results;
using TT.Lib.Util;
using TT.Viewer.Events;
using System.Collections.Generic;
using TT.Lib.Models;

namespace TT.Viewer.ViewModels
{

    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive,
        IShell
    {

        public FilterViewModel FilterView { get; private set; }
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            Events = eventAggregator;
            FilterView = new FilterViewModel(Events);
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

            this.Events.PublishOnUIThread(new MatchOpenedEvent(deserialization.Result, deserialization.FileName));
        }

        #endregion

        #region Event Handlers

        #endregion
    }
}