using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Managers;
using System.Windows.Controls.Primitives;
using TT.Models;
using TT.Lib.Events;
using TT.Models.Util.Enums;
using System.Windows.Controls;

namespace TT.Scouter.ViewModels
{
    public class ChoiceOfServiceReceiveViewModel : Screen
    {

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager MatchManager;
        public LiveViewModel LiveView { get; private set; }
        private bool _firstServerPlayer1;
        /// <summary>
        /// Determines whether Player1 is FirstServer
        /// </summary>
        public bool FirstServerPlayer1
        {
            get { return _firstServerPlayer1; }
            set
            {
                if (_firstServerPlayer1 != value)
                {
                    _firstServerPlayer1 = value;
                    NotifyOfPropertyChange();
                }

            }
        }

        private bool _firstServerPlayer2;
        /// <summary>
        /// Determines whether Player1 is FirstServer
        /// </summary>
        public bool FirstServerPlayer2
        {
            get { return _firstServerPlayer2; }
            set
            {
                if (_firstServerPlayer2 != value)
                {
                    _firstServerPlayer2 = value;
                    NotifyOfPropertyChange();
                }

            }
        }



        public ChoiceOfServiceReceiveViewModel(IEventAggregator eventAggregator, IMatchManager man, LiveViewModel live)
        {
            this.events = eventAggregator;
            MatchManager = man;
            LiveView = live;

            if (!LiveView.FirstServerSet) { 
            FirstServerPlayer1 = false;
            FirstServerPlayer2 = false;
            }
            else
            {
                if (LiveView.Match.FirstServer == MatchPlayer.First)
                {
                    FirstServerPlayer1 = true;
                    FirstServerPlayer2 = false;
                }
                else
                {
                    FirstServerPlayer1 = false;
                    FirstServerPlayer2 = true;
                }
            }


        }


        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        /// 

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }
        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
        }

        /// <summary>
        /// Handles deactivation of this view model.
        /// </summary>
        /// <param name="close">Whether the view model is closed</param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }
        #endregion

        #region View Methods



        public void SetFirstServer()
        {
            if (FirstServerPlayer1)
            {   LiveView.firstServerBackup = MatchManager.ConvertPlayer(MatchManager.Match.FirstPlayer);
                LiveView.Server = MatchManager.ConvertPlayer(MatchManager.Match.FirstPlayer);
                LiveView.CurrentRally.Server = LiveView.Server;
                NotifyOfPropertyChange("LiveView.FirstServerBackup");
                NotifyOfPropertyChange("LiveView.Server");
                NotifyOfPropertyChange("LiveView.FirstServerSet");
                MatchManager.MatchModified = true;

            }
            else if (FirstServerPlayer2)
            {
                LiveView.firstServerBackup = MatchManager.ConvertPlayer(MatchManager.Match.SecondPlayer);
                LiveView.Server = MatchManager.ConvertPlayer(MatchManager.Match.SecondPlayer);
                LiveView.CurrentRally.Server = LiveView.Server;
                NotifyOfPropertyChange("LiveView.FirstServerBackup");
                NotifyOfPropertyChange("LiveView.Server");
                NotifyOfPropertyChange("LiveView.FirstServerSet");
                MatchManager.MatchModified = true;
            }
            else
            {
               
            }
        }
        public void Next()
        {
            LiveView.CurrentScreen = LiveView.LiveScouting;
            LiveView.ChangeTransitioningContent();
        }

        public void Previous()
        {
            LiveView.CurrentScreen = LiveView.ChoiceOfEnds;
            LiveView.ChangeTransitioningContent();
        }



        #endregion
    }
}
