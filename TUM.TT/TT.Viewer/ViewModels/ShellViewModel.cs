using Caliburn.Micro;
using System.Windows;
using System.Reflection;
using TT.Lib.Results;
using TT.Lib.Util;
using TT.Lib.Events;
using System.Collections.Generic;
using TT.Lib.Models;
using System.IO;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{

    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive,
        IShell
    {
        public MediaViewModel MediaView { get; private set; }
        public FilterStatisticsViewModel FilterStatisticsView { get; private set; }
        public ResultViewModel ResultView { get; private set; }
        public PlaylistViewModel PlaylistView { get; private set; }
        public string SaveFileName { get; private set; }
        public bool IsModified { get; private set; }
        private IMatchManager Manager;

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }

        public ShellViewModel(IEventAggregator eventAggregator, IEnumerable<IResultViewTabItem> resultTabs, IMatchManager manager)
        {
            this.DisplayName = "TUM.TT";
            Events = eventAggregator;
            Manager = manager;
            FilterStatisticsView = new FilterStatisticsViewModel(Events, Manager);
            MediaView = new MediaViewModel(Events);
            ResultView = new ResultViewModel(resultTabs);
            PlaylistView = new PlaylistViewModel(Events, Manager);
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
            this.ActivateItem(FilterStatisticsView);
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
            return Manager.OpenMatchAction();
        }


        public IEnumerable<IResult> SaveMatch()
        {
            return Manager.SaveMatchAction();
        }
        
        #endregion
    }
}
