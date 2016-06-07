using Caliburn.Micro;
using TT.Lib.Events;
using System.Collections.Generic;
using TT.Models;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib.Managers;
using TT.Lib.Interfaces;
using System.Linq;
using System;

namespace TT.Viewer.ViewModels
{

    public class MatchViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<VideoPlayEvent>              
    {
        public IMediaPosition MediaPlayer { get; private set; }
        public FilterStatisticsViewModel FilterStatisticsView { get; private set; }
        public ResultViewModel ResultView { get; private set; }
        public PlaylistViewModel PlaylistView { get; private set; } 
        public CommentViewModel CommentView { get; private set; }

        private Rally _rally;
        public Rally CurrentRally
        {
            get { return _rally; }
            set
            {
                if (_rally != value)
                {
                    _rally = value;               
                    NotifyOfPropertyChange();
                }
            }
        }

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
            CurrentRally = Manager.ActivePlaylist.Rallies.First();
            ResultView = new ResultViewModel(resultTabs, eventAggregator, manager);
            PlaylistView = new PlaylistViewModel(Events, Manager, dc);
            MediaPlayer = new MediaViewModel(Events, Manager, dc);
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
            this.ActivateItem(MediaPlayer);
            this.ActivateItem(FilterStatisticsView);
            this.ActivateItem(CommentView);                              
        }

        #endregion

        #region Methods



        #endregion

        #region Events

        public void Handle(VideoPlayEvent message)
        {
            var item = message.Current;

            if (item != null)
            {
                MediaPlayer.Minimum = item.Anfang;
                MediaPlayer.Maximum = item.Ende;

                if (MediaPlayer.toRallyStart == true)
                {
                    CurrentRally = item;
                    TimeSpan anfangRally = TimeSpan.FromMilliseconds(item.Anfang);
                    TimeSpan endeRally = TimeSpan.FromMilliseconds(item.Ende);               
                    MediaPlayer.MediaPosition = anfangRally;
                    MediaPlayer.EndPosition = endeRally;
                    MediaPlayer.Play();
                }
                else if (MediaPlayer.toRallyStart != true)
                {
                    CurrentRally = item;
                    TimeSpan anfangRally = TimeSpan.FromMilliseconds(item.Anfang);
                    TimeSpan endeRally = TimeSpan.FromMilliseconds(item.Ende - 1000);
                    MediaPlayer.MediaPosition = endeRally;
                    MediaPlayer.EndPosition = endeRally;
                    MediaPlayer.Play();
                }
            }
        }

        #endregion
    }
}


