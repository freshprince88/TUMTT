using Caliburn.Micro;
using System.Windows;
using System.Reflection;
using TT.Models.Results;
using TT.Models.Util;
using TT.Models.Events;
using System.Collections.Generic;
using TT.Models;
using System.IO;
using MahApps.Metro.Controls.Dialogs;
using TT.Models.Managers;
using TT.Models.Util.Enums;

namespace TT.Viewer.ViewModels
{

    public class MatchViewModel : Conductor<IScreen>.Collection.AllActive
        
        
    {
        public MediaViewModel MediaView { get; private set; }
        public FilterStatisticsViewModel FilterStatisticsView { get; private set; }
        public ResultViewModel ResultView { get; private set; }
        public PlaylistViewModel PlaylistView { get; private set; } 
        public CommentViewModel CommentView { get; private set; }      

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }
        private IMatchManager Manager;

        public MatchViewModel(IEventAggregator eventAggregator, IEnumerable<IResultViewTabItem> resultTabs, IMatchManager manager, IDialogCoordinator dc)
        {
            this.DisplayName = "TUM.TT";
            Events = eventAggregator;
            Manager = manager;
            ResultView = new ResultViewModel(resultTabs);
            PlaylistView = new PlaylistViewModel(Events, Manager, dc);
            MediaView = new MediaViewModel(Events, Manager);
            FilterStatisticsView = new FilterStatisticsViewModel(Events, Manager);
            CommentView = new CommentViewModel(Events, Manager);                  
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
            this.ActivateItem(ResultView);
            this.ActivateItem(PlaylistView);
            this.ActivateItem(MediaView);
            this.ActivateItem(FilterStatisticsView);
            this.ActivateItem(CommentView);                              
        }

        #endregion

        #region Methods
       


        #endregion
    }
}


