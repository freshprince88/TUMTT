﻿using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Lib.Models;
using TT.Lib.Events;
using TT.Lib.Util.Enums;

namespace TT.Viewer.ViewModels
{
    public class BasicFilterViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<FilterSwitchedEvent>,
        IHandle<MatchOpenedEvent>
    {
        #region Properties

        public string FilterPointPlayer1Button { get; set; }
        public string FilterPointPlayer2Button { get; set; }

        public SpinControlViewModel SpinControl { get; private set; }
        public TableServiceViewModel TableView { get; private set; }
        public List<Rally> SelectedRallies { get; private set; }

        public Playlist RallyList { get; private set; }
        public List<Stroke.Spin> SelectedSpins { get; private set; }
        
        public Stroke.Point Point { get; private set; }
        public Stroke.Server Server { get; private set; }
       
        public Stroke.Crunch Crunch { get; private set; }

        public HashSet<int> SelectedSets { get; private set; }
        public HashSet<int> SelectedRallyLengths { get; private set; }
        public int MinRallyLength { get; set; }
        public bool LastStroke { get; set; }
        public int StrokeNumber { get; set; }

        public String PlayerLabel { get; set; }

        #endregion


        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public BasicFilterViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedRallies = new List<Rally>();            
            RallyList = new Playlist();            
            Point = Stroke.Point.None;
            Server = Stroke.Server.None;           
            Crunch = Stroke.Crunch.Not;
            SelectedSets = new HashSet<int>();
            SelectedRallyLengths = new HashSet<int>();
            FilterPointPlayer1Button = "Spieler 1";
            FilterPointPlayer2Button = "Spieler 2";
            MinRallyLength = 0;
            PlayerLabel = "";
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
            UpdateSelection();
        }

        public void RallyLengthFilter(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("rallylengthallbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedRallyLengths.Add(1);
                    SelectedRallyLengths.Add(2);
                    SelectedRallyLengths.Add(3);
                    SelectedRallyLengths.Add(4);
                    SelectedRallyLengths.Add(5);
                    SelectedRallyLengths.Add(6);
                }
                else
                {
                    SelectedRallyLengths.Remove(1);
                    SelectedRallyLengths.Remove(2);
                    SelectedRallyLengths.Remove(3);
                    SelectedRallyLengths.Remove(4);
                    SelectedRallyLengths.Remove(5);
                    SelectedRallyLengths.Remove(6);
                }
            }
            else if (source.Name.ToLower().Contains("rallylength1button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedRallyLengths.Add(1);
                }
                else
                {
                    SelectedRallyLengths.Remove(1);
                }
            }
            else if (source.Name.ToLower().Contains("rallylength2button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedRallyLengths.Add(2);
                }
                else
                {
                    SelectedRallyLengths.Remove(2);
                }
            }
            else if (source.Name.ToLower().Contains("rallylength3button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedRallyLengths.Add(3);
                }
                else
                {
                    SelectedRallyLengths.Remove(3);
                }
            }
            else if (source.Name.ToLower().Contains("rallylength4button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedRallyLengths.Add(4);
                }
                else
                {
                    SelectedRallyLengths.Remove(4);
                }
            }
            else if (source.Name.ToLower().Contains("rallylength5button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedRallyLengths.Add(5);
                }
                else
                {
                    SelectedRallyLengths.Remove(5);
                }
            }
            else if (source.Name.ToLower().Contains("rallylength5upbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedRallyLengths.Add(6);
                }
                else
                {
                    SelectedRallyLengths.Remove(6);
                }
            }
            UpdateSelection();
        }

        public void CrunchOrNot(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("crunchtime"))
            {
                if (source.IsChecked.Value)
                {
                    Crunch = Stroke.Crunch.CrunchTime;
                }
                else
                {
                    Crunch = Stroke.Crunch.Not;
                }
            }
            UpdateSelection();
        }


        public void P1P2Point(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("player1"))
            {
                if (source.IsChecked.Value)
                {
                    if (Point == Stroke.Point.None)
                        Point = Stroke.Point.Player1;
                    else if (Point == Stroke.Point.Player2)
                        Point = Stroke.Point.Both;
                }
                else
                {
                    if (Point == Stroke.Point.Player1)
                        Point = Stroke.Point.None;
                    else if (Point == Stroke.Point.Both)
                        Point = Stroke.Point.Player2;
                }
            }
            else if (source.Name.ToLower().Contains("player2"))
            {
                if (source.IsChecked.Value)
                {
                    if (Point == Stroke.Point.None)
                        Point = Stroke.Point.Player2;
                    else if (Point == Stroke.Point.Player1)
                        Point = Stroke.Point.Both;
                }
                else
                {
                    if (Point == Stroke.Point.Player2)
                        Point = Stroke.Point.None;
                    else if (Point == Stroke.Point.Both)
                        Point = Stroke.Point.Player1;
                }
            }
            UpdateSelection();
        }

        public void P1P2Server(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("player1"))
            {
                if (source.IsChecked.Value)
                {
                    if (Server == Stroke.Server.None)
                        Server = Stroke.Server.Player1;
                    else if (Server == Stroke.Server.Player2)
                        Server = Stroke.Server.Both;
                }
                else
                {
                    if (Server == Stroke.Server.Player1)
                        Server = Stroke.Server.None;
                    else if (Server == Stroke.Server.Both)
                        Server = Stroke.Server.Player2;
                }
            }
            else if (source.Name.ToLower().Contains("player2"))
            {
                if (source.IsChecked.Value)
                {
                    if (Server == Stroke.Server.None)
                        Server = Stroke.Server.Player2;
                    else if (Server == Stroke.Server.Player1)
                        Server = Stroke.Server.Both;
                }
                else
                {
                    if (Server == Stroke.Server.Player2)
                        Server = Stroke.Server.None;
                    else if (Server == Stroke.Server.Both)
                        Server = Stroke.Server.Player1;
                }
            }
            UpdateSelection();
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

        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }

        #endregion

        #region Event Handlers

        public void Handle(FilterSwitchedEvent message)
        {
            this.RallyList = message.Playlist;

            UpdateSelection();
        }

        public void Handle(MatchOpenedEvent message)
        {
            if (message.Match.FirstPlayer != null && message.Match.SecondPlayer != null)
            {
                FilterPointPlayer1Button = message.Match.FirstPlayer.Name.Split(' ')[0];
                FilterPointPlayer2Button = message.Match.SecondPlayer.Name.Split(' ')[0];
            }
        }

        #endregion

        #region Helper Methods

        public void UpdateSelection()
        {
            if (this.RallyList.Rallies != null)
            {
                SelectedRallies = this.RallyList.Rallies.Where(r => Convert.ToInt32(r.Length) > MinRallyLength && HasSet(r) && HasRallyLength(r) && HasCrunchTime(r) && HasPoint(r) && HasServer(r)).ToList();              
                this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
            }
        }


        private bool HasPoint(Rally r)
        {
            switch (this.Point)
            {
                case Stroke.Point.Player1:
                    return r.Winner == "First";  //TODO Name der Spieler dynamisch????
                case Stroke.Point.Player2:
                    return r.Winner == "Second"; //TODO Name der Spieler dynamisch????
                case Stroke.Point.None:
                    return true;
                case Stroke.Point.Both:
                    return true;
                default:
                    return false;
            }
        }

        private bool HasServer(Rally r)
        {
            switch (this.Server)
            {
                case Stroke.Server.Player1:
                    return r.Schlag[StrokeNumber].Spieler == "First";  //TODO Name der Spieler dynamisch???? && Letzter Schlag funktioniert so nicht...
                case Stroke.Server.Player2:
                    return r.Schlag[StrokeNumber].Spieler == "Second"; //TODO Name der Spieler dynamisch???? && Letzter Schlag funktioniert so nicht...
                case Stroke.Server.None:
                    return true;
                case Stroke.Server.Both:
                    return true;
                default:
                    return false;
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

        private bool HasRallyLength(Rally r)
        {
            List<bool> ORresults = new List<bool>();

            foreach (var rallylength in SelectedRallyLengths)
            {

                if (rallylength <= 5)
                {
                    ORresults.Add(Convert.ToInt32(r.Length) == rallylength);
                }

                else if (rallylength == 6)
                {
                    ORresults.Add(Convert.ToInt32(r.Length) >= rallylength);
                }


            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasCrunchTime(Rally r)
        {
            switch (this.Crunch)
            {
                case Stroke.Crunch.CrunchTime:
                    return (Convert.ToInt32(r.CurrentRallyScore.First) + Convert.ToInt32(r.CurrentRallyScore.Second)) >= 16;
                case Stroke.Crunch.Not:
                    return true;
                default:
                    return false;
            }
        }

        #endregion
    }
}
