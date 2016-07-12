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

namespace TT.Scouter.ViewModels
{
    public class ChoiceOfEndsViewModel : Screen
    {

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager MatchManager;

        public LiveViewModel LiveView { get; private set; }

        private bool _player1TopPlayer2Bottom;
        /// <summary>
        /// Determines whether Player1 is FirstServer
        /// </summary>
        public bool Player1TopPlayer2Bottom
        {
            get { return _player1TopPlayer2Bottom; }
            set
            {
                if (_player1TopPlayer2Bottom != value)
                {
                    _player1TopPlayer2Bottom = value;
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange("Player1TopPlayer2Bottom");

                }

            }
        }
        private bool _player2TopPlayer1Bottom;
        /// <summary>
        /// Determines whether Player1 is FirstServer
        /// </summary>
        public bool Player2TopPlayer1Bottom
        {
            get { return _player2TopPlayer1Bottom; }
            set
            {
                if (_player2TopPlayer1Bottom != value)
                {
                    _player2TopPlayer1Bottom = value;
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange("Player2TopPlayer1Bottom");
                }

            }
        }

        public ChoiceOfEndsViewModel(IEventAggregator eventAggregator, IMatchManager man, LiveViewModel live)
        {
            this.events = eventAggregator;
            MatchManager = man;
            LiveView = live;
            if (MatchManager.Match.FirstPlayer.StartingTableEnd == StartingTableEnd.Top && MatchManager.Match.SecondPlayer.StartingTableEnd == StartingTableEnd.Bottom)
            {
                Player1TopPlayer2Bottom = true;
                Player2TopPlayer1Bottom = false;
            }
            if (MatchManager.Match.FirstPlayer.StartingTableEnd == StartingTableEnd.Bottom && MatchManager.Match.SecondPlayer.StartingTableEnd == StartingTableEnd.Top)
            {
                Player1TopPlayer2Bottom = false;
                Player2TopPlayer1Bottom = true;
            }
            else
            {
                Player1TopPlayer2Bottom = false;
                Player2TopPlayer1Bottom = false;
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
        public void SetChoiceOfEnds()
        {
            if (Player1TopPlayer2Bottom)
            {
                MatchManager.Match.FirstPlayer.StartingTableEnd = StartingTableEnd.Top;
                MatchManager.Match.SecondPlayer.StartingTableEnd = StartingTableEnd.Bottom;
                MatchManager.MatchModified = true;
            }
            else if (Player2TopPlayer1Bottom)
            {
                MatchManager.Match.FirstPlayer.StartingTableEnd = StartingTableEnd.Bottom;
                MatchManager.Match.SecondPlayer.StartingTableEnd = StartingTableEnd.Top;
                MatchManager.MatchModified = true;
            }
            else
            {
                MatchManager.Match.FirstPlayer.StartingTableEnd = StartingTableEnd.None;
                MatchManager.Match.SecondPlayer.StartingTableEnd = StartingTableEnd.None;
            }

        }

        public void Next()
        {
            LiveView.CurrentScreen = LiveView.ChoiceOfServiceReceive;
            LiveView.ChangeTransitioningContent();
        }
        #endregion
    }


}
