﻿using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Models;
using TT.Lib.Events;
using TT.Models.Util.Enums;
using TT.Lib.Managers;
using TT.Lib;

namespace TT.Viewer.ViewModels
{
    public class BasicFilterStatisticsViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<PlaylistSelectionChangedEvent>
    {
        #region Properties

        private List<Rally> _selRallies;
        public List<Rally> SelectedRallies
        {
            get
            {
                return _selRallies;
            }
            set
            {
                _selRallies = value;
                NotifyOfPropertyChange("SelectedRallies");
            }
        }
        public Models.Util.Enums.Stroke.Player Player { get; private set; }
        public Models.Util.Enums.Stroke.Crunch Crunch { get; private set; }
        public Models.Util.Enums.Stroke.BeginningOfGame BeginningOfGame { get; private set; }
        public Models.Util.Enums.Stroke.GamePhase GamePhase { get; private set; }

        public HashSet<int> SelectedSets { get; private set; }
        public int MinRallyLength { get; set; }
        public bool LastStroke { get; set; }
        public int StrokeNumber { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }

        #endregion


        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public BasicFilterStatisticsViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            SelectedRallies = new List<Rally>();
            Player = Models.Util.Enums.Stroke.Player.None;
            Crunch = Models.Util.Enums.Stroke.Crunch.Not;
            BeginningOfGame = Models.Util.Enums.Stroke.BeginningOfGame.Not;
            GamePhase = Models.Util.Enums.Stroke.GamePhase.Not;
            SelectedSets = new HashSet<int>();

            Player1 = "Spieler 1";
            Player2 = "Spieler 2";
            MinRallyLength = 0;

            LastStroke = false;
            StrokeNumber = 0;
        }

        #region View Methods

        public void SetFilter(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("setallbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(1);
                    SelectedSets.Add(2);
                    SelectedSets.Add(3);
                    SelectedSets.Add(4);
                    SelectedSets.Add(5);
                    SelectedSets.Add(6);
                    SelectedSets.Add(7);
                }
                else
                {
                    SelectedSets.Remove(1);
                    SelectedSets.Remove(2);
                    SelectedSets.Remove(3);
                    SelectedSets.Remove(4);
                    SelectedSets.Remove(5);
                    SelectedSets.Remove(6);
                    SelectedSets.Remove(7);
                }
            }
            else if (source.Name.ToLower().Contains("set1button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(1);
                }
                else
                {
                    SelectedSets.Remove(1);
                }
            }
            else if (source.Name.ToLower().Contains("set2button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(2);
                }
                else
                {
                    SelectedSets.Remove(2);
                }
            }
            else if (source.Name.ToLower().Contains("set3button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(3);
                }
                else
                {
                    SelectedSets.Remove(3);
                }
            }
            else if (source.Name.ToLower().Contains("set4button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(4);
                }
                else
                {
                    SelectedSets.Remove(4);
                }
            }
            else if (source.Name.ToLower().Contains("set5button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(5);
                }
                else
                {
                    SelectedSets.Remove(5);
                }
            }
            else if (source.Name.ToLower().Contains("set6button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(6);
                }
                else
                {
                    SelectedSets.Remove(6);
                }
            }
            else if (source.Name.ToLower().Contains("set7button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(7);
                }
                else
                {
                    SelectedSets.Remove(7);
                }
            }
            UpdateSelection(Manager.ActivePlaylist);
        }

        public void CrunchOrNot(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("crunchtime"))
            {
                if (source.IsChecked.Value)
                {
                    Crunch = Models.Util.Enums.Stroke.Crunch.CrunchTime;

                }
                else
                {
                    Crunch = Models.Util.Enums.Stroke.Crunch.Not;
                    //GamePhase = Models.Util.Enums.Stroke.GamePhase.Not;

                }
            }
            UpdateSelection(Manager.ActivePlaylist);
        }
        public void BeginningOfGameOrNot(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("beginningofgame"))
            {
                if (source.IsChecked.Value)
                {
                    BeginningOfGame = Models.Util.Enums.Stroke.BeginningOfGame.BeginningOfGame;
                    

                }
                else
                {
                    BeginningOfGame = Models.Util.Enums.Stroke.BeginningOfGame.Not;

                }
            }
            UpdateSelection(Manager.ActivePlaylist);
        }
        public void CrunchOrBeginningPhase(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("beginningofgame"))
            {
                if (source.IsChecked.Value)
                {
                    if (GamePhase == Models.Util.Enums.Stroke.GamePhase.Not)
                        GamePhase = Models.Util.Enums.Stroke.GamePhase.BeginningOfGame;
                    else if (GamePhase == Models.Util.Enums.Stroke.GamePhase.CrunchTime)
                        GamePhase = Models.Util.Enums.Stroke.GamePhase.AllPhases;
                }
                else
                {
                    if (GamePhase == Models.Util.Enums.Stroke.GamePhase.BeginningOfGame)
                        GamePhase = Models.Util.Enums.Stroke.GamePhase.Not;
                    else if (GamePhase == Models.Util.Enums.Stroke.GamePhase.AllPhases)
                        GamePhase = Models.Util.Enums.Stroke.GamePhase.CrunchTime;
                }
            }
            else if (source.Name.ToLower().Contains("crunchtime"))
            {
                if (source.IsChecked.Value)
                {
                    if (GamePhase == Models.Util.Enums.Stroke.GamePhase.Not)
                        GamePhase = Models.Util.Enums.Stroke.GamePhase.CrunchTime;
                    else if (GamePhase == Models.Util.Enums.Stroke.GamePhase.BeginningOfGame)
                        GamePhase = Models.Util.Enums.Stroke.GamePhase.AllPhases;
                }
                else
                {
                    if (GamePhase == Models.Util.Enums.Stroke.GamePhase.CrunchTime)
                        GamePhase = Models.Util.Enums.Stroke.GamePhase.Not;
                    else if (GamePhase == Models.Util.Enums.Stroke.GamePhase.AllPhases)
                        GamePhase = Models.Util.Enums.Stroke.GamePhase.BeginningOfGame;
                }
            }

            UpdateSelection(Manager.ActivePlaylist);
        }

        public void P1P2(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("player1"))
            {
                if (source.IsChecked.Value)
                {
                    if (Player == Models.Util.Enums.Stroke.Player.None)
                        Player = Models.Util.Enums.Stroke.Player.Player1;
                    else if (Player == Models.Util.Enums.Stroke.Player.Player2)
                        Player = Models.Util.Enums.Stroke.Player.Both;
                }
                else
                {
                    if (Player == Models.Util.Enums.Stroke.Player.Player1)
                        Player = Models.Util.Enums.Stroke.Player.None;
                    else if (Player == Models.Util.Enums.Stroke.Player.Both)
                        Player = Models.Util.Enums.Stroke.Player.Player2;
                }
            }
            else if (source.Name.ToLower().Contains("player2"))
            {
                if (source.IsChecked.Value)
                {
                    if (Player == Models.Util.Enums.Stroke.Player.None)
                        Player = Models.Util.Enums.Stroke.Player.Player2;
                    else if (Player == Models.Util.Enums.Stroke.Player.Player1)
                        Player = Models.Util.Enums.Stroke.Player.Both;
                }
                else
                {
                    if (Player == Models.Util.Enums.Stroke.Player.Player2)
                        Player = Models.Util.Enums.Stroke.Player.None;
                    else if (Player == Models.Util.Enums.Stroke.Player.Both)
                        Player = Models.Util.Enums.Stroke.Player.Player1;
                }
            }
            UpdateSelection(Manager.ActivePlaylist);
        }

        public void DetailsChecked(bool radio_percent_checked)
        {
            events.PublishOnUIThread(new StatisticDetailChangedEvent(true, radio_percent_checked));
        }

        public void DetailsUnchecked(bool radio_percent_checked)
        {
            events.PublishOnUIThread(new StatisticDetailChangedEvent(false, radio_percent_checked));
        }

        #endregion

        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            Player1 = Manager.Match.FirstPlayer.Name.Split(' ')[0];
            Player2 = Manager.Match.SecondPlayer.Name.Split(' ')[0];
            NotifyOfPropertyChange();
            NotifyOfPropertyChange("SelectedRallies");
            NotifyOfPropertyChange("Manager.ActivePlaylist");
            UpdateSelection(Manager.ActivePlaylist);
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);

            UpdateSelection(Manager.ActivePlaylist);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }

        #endregion

        #region Event Handlers

        public void Handle(PlaylistSelectionChangedEvent message)
        {
            UpdateSelection(Manager.ActivePlaylist);
        }


        #endregion

        #region Helper Methods

        public void UpdateSelection(Playlist list)
        {
            if (list.Rallies != null)
            {
                SelectedRallies = list.Rallies.Where(r => Convert.ToInt32(r.Length) > MinRallyLength && HasSet(r) && HasGamePhase(r) && HasPlayer(r)).ToList();
                this.events.PublishOnUIThread(new BasicFilterSelectionChangedEvent(SelectedRallies));
            }
        }

        private bool HasPlayer(Rally r)
        {
            if (LastStroke == false) { 
            switch (this.Player)
            {
                case Models.Util.Enums.Stroke.Player.Player1:
                    return r.Strokes[StrokeNumber].Player == MatchPlayer.First;
                case Models.Util.Enums.Stroke.Player.Player2:
                    return r.Strokes[StrokeNumber].Player == MatchPlayer.Second;
                case Models.Util.Enums.Stroke.Player.None:
                    return true;
                case Models.Util.Enums.Stroke.Player.Both:
                    return true;
                default:
                    return false;
            }
            }
            else
            {
                switch (this.Player)
                {
                    case Models.Util.Enums.Stroke.Player.Player1:
                        return r.LastWinnerStroke().Player == MatchPlayer.First;
                    case Models.Util.Enums.Stroke.Player.Player2:
                        return r.LastWinnerStroke().Player == MatchPlayer.Second;
                    case Models.Util.Enums.Stroke.Player.None:
                        return true;
                    case Models.Util.Enums.Stroke.Player.Both:
                        return true;
                    default:
                        return false;
                }
            }
        }

        private bool HasSet(Rally r)
        {
            List<bool> ORresults = new List<bool>();

            foreach (var set in SelectedSets)
            {
                int setTotal = Convert.ToInt32(r.CurrentSetScore.First) + Convert.ToInt32(r.CurrentSetScore.Second) + 1;
                ORresults.Add(setTotal == set);
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasCrunchTime(Rally r)
        {
            switch (this.Crunch)
            {
                case Models.Util.Enums.Stroke.Crunch.CrunchTime:
                    return (Convert.ToInt32(r.CurrentRallyScore.First) + Convert.ToInt32(r.CurrentRallyScore.Second)) >= 16;
                case Models.Util.Enums.Stroke.Crunch.Not:
                    return true;
                default:
                    return false;
            }
        }
        private bool HasBeginningOfGame(Rally r)
        {
            switch (this.BeginningOfGame)
            {
                case Models.Util.Enums.Stroke.BeginningOfGame.BeginningOfGame:
                    return (((Convert.ToInt32(r.CurrentRallyScore.First) + Convert.ToInt32(r.CurrentRallyScore.Second)) <= 8) && (Convert.ToInt32(r.CurrentRallyScore.First) <= 4) && (Convert.ToInt32(r.CurrentRallyScore.Second) <= 4));
                case Models.Util.Enums.Stroke.BeginningOfGame.Not:
                    return true;
                default:
                    return false;
            }
        }

        private bool HasGamePhase(Rally r)
        {
            switch (this.GamePhase)
            {
                case Models.Util.Enums.Stroke.GamePhase.BeginningOfGame:
                    return (((Convert.ToInt32(r.CurrentRallyScore.First) + Convert.ToInt32(r.CurrentRallyScore.Second)) <= 8) && (Convert.ToInt32(r.CurrentRallyScore.First) <= 4) && (Convert.ToInt32(r.CurrentRallyScore.Second) <= 4));
                case Models.Util.Enums.Stroke.GamePhase.CrunchTime:
                    return (Convert.ToInt32(r.CurrentRallyScore.First) + Convert.ToInt32(r.CurrentRallyScore.Second)) >= 16;
                case Models.Util.Enums.Stroke.GamePhase.AllPhases:
                    return ((((Convert.ToInt32(r.CurrentRallyScore.First) + Convert.ToInt32(r.CurrentRallyScore.Second)) <= 8) && (Convert.ToInt32(r.CurrentRallyScore.First) <= 4) && (Convert.ToInt32(r.CurrentRallyScore.Second) <= 4)) || (Convert.ToInt32(r.CurrentRallyScore.First) + Convert.ToInt32(r.CurrentRallyScore.Second)) >= 16);
                case Models.Util.Enums.Stroke.GamePhase.Not:
                    return true;
                default:
                    return false;
            }
        }

        #endregion
    }
}
