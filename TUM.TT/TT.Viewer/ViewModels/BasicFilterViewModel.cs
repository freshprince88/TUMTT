using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Lib.Models;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class BasicFilterViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<FilterSwitchedEvent>
       
    {
        #region Properties

        public string FilterPointPlayer1Button { get; set; }
        public string FilterPointPlayer2Button { get; set; }

        public SpinControlViewModel SpinControl { get; private set; }
        public TableServiceViewModel TableView { get; private set; }
        public List<MatchRally> SelectedRallies { get; private set; }

        public Match Match { get; private set; }
        public List<SpinControlViewModel.Spins> SelectedSpins { get; private set; }
        
        public EPoint Point { get; private set; }
        public EServer Server { get; private set; }
       
        public ECrunch Crunch { get; private set; }

        public HashSet<int> SelectedSets { get; private set; }
        public HashSet<int> SelectedRallyLengths { get; private set; }
        public int MinRallyLength { get; set; }
        public String PlayerLabel { get; set; }



        #endregion

        #region Enums



        public enum EPoint

        {
            Player1,
            Player2,
            None,
            Both
        }

        public enum EServer

        {
            Player1,
            Player2,
            None,
            Both
        }

       

        public enum ECrunch
        {
            CrunchTime,
            Not
        }

       

        #endregion

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public BasicFilterViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedRallies = new List<MatchRally>();            
            Match = new Match();            
            Point = EPoint.None;
            Server = EServer.None;           
            Crunch = ECrunch.Not;
            SelectedSets = new HashSet<int>();
            SelectedRallyLengths = new HashSet<int>();
            FilterPointPlayer1Button = "Spieler 1";
            FilterPointPlayer2Button = "Spieler 2";
            MinRallyLength = 0;
            PlayerLabel = "";
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
                    Crunch = ECrunch.CrunchTime;
                }
                else
                {
                    Crunch = ECrunch.Not;
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
                    if (Point == EPoint.None)
                        Point = EPoint.Player1;
                    else if (Point == EPoint.Player2)
                        Point = EPoint.Both;
                }
                else
                {
                    if (Point == EPoint.Player1)
                        Point = EPoint.None;
                    else if (Point == EPoint.Both)
                        Point = EPoint.Player2;
                }
            }
            else if (source.Name.ToLower().Contains("player2"))
            {
                if (source.IsChecked.Value)
                {
                    if (Point == EPoint.None)
                        Point = EPoint.Player2;
                    else if (Point == EPoint.Player1)
                        Point = EPoint.Both;
                }
                else
                {
                    if (Point == EPoint.Player2)
                        Point = EPoint.None;
                    else if (Point == EPoint.Both)
                        Point = EPoint.Player1;
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
                    if (Server == EServer.None)
                        Server = EServer.Player1;
                    else if (Server == EServer.Player2)
                        Server = EServer.Both;
                }
                else
                {
                    if (Server == EServer.Player1)
                        Server = EServer.None;
                    else if (Server == EServer.Both)
                        Server = EServer.Player2;
                }
            }
            else if (source.Name.ToLower().Contains("player2"))
            {
                if (source.IsChecked.Value)
                {
                    if (Server == EServer.None)
                        Server = EServer.Player2;
                    else if (Server == EServer.Player1)
                        Server = EServer.Both;
                }
                else
                {
                    if (Server == EServer.Player2)
                        Server = EServer.None;
                    else if (Server == EServer.Both)
                        Server = EServer.Player1;
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
            this.Match = message.Match;
            if (this.Match.FirstPlayer != null && this.Match.SecondPlayer != null)
            {
                FilterPointPlayer1Button = this.Match.FirstPlayer.Name.Split(' ')[0];
                FilterPointPlayer2Button = this.Match.SecondPlayer.Name.Split(' ')[0];
            }
            UpdateSelection();
        }



        #endregion

        #region Helper Methods

        private void showButtonsRallyLength(int minLength)
        {
            for (int i = minLength; i < 5; i++)
            {
                
            }

        }

        private void UpdateSelection()
        {
            if (this.Match.Rallies != null)
            {
                SelectedRallies = this.Match.Rallies.Where(r => Convert.ToInt32(r.Length) > MinRallyLength && HasSet(r) && HasRallyLength(r) && HasCrunchTime(r) && HasPoint(r) && HasServer(r)).ToList();
                this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
            }
        }


        private bool HasPoint(MatchRally r)
        {
            switch (this.Point)
            {
                case EPoint.Player1:
                    return r.Winner == "First";  //TODO Name der Spieler dynamisch????
                case EPoint.Player2:
                    return r.Winner == "Second"; //TODO Name der Spieler dynamisch????
                case EPoint.None:
                    return true;
                case EPoint.Both:
                    return true;
                default:
                    return false;
            }
        }

        private bool HasServer(MatchRally r)
        {
            switch (this.Server)
            {
                case EServer.Player1:
                    return r.Schlag[MinRallyLength].Spieler == "First";  //TODO Name der Spieler dynamisch????
                case EServer.Player2:
                    return r.Schlag[MinRallyLength].Spieler == "Second"; //TODO Name der Spieler dynamisch????
                case EServer.None:
                    return true;
                case EServer.Both:
                    return true;
                default:
                    return false;
            }
        }

       
        private bool HasSet(MatchRally r)
        {
            List<bool> ORresults = new List<bool>();

            foreach (var set in SelectedSets)
            {
                int setTotal = Convert.ToInt32(r.CurrentSetScore.First) + Convert.ToInt32(r.CurrentSetScore.Second) + 1;
                ORresults.Add(setTotal == set);
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasRallyLength(MatchRally r)
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

        private bool HasCrunchTime(MatchRally r)
        {
            switch (this.Crunch)
            {
                case ECrunch.CrunchTime:
                    return (Convert.ToInt32(r.CurrentRallyScore.First) + Convert.ToInt32(r.CurrentRallyScore.Second)) >= 16;
                case ECrunch.Not:
                    return true;
                default:
                    return false;
            }
        }

        #endregion
    }
}
