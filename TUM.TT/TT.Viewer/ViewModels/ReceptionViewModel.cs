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
    public class ReceptionViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<TableViewSelectionChangedEvent>,
        IHandle<FilterSwitchedEvent>
    {
     
        public TableServiceViewModel TableView { get; set; }
        public List<MatchRally> SelectedRallies { get; private set; }
        public Match Match { get; private set; }
        public EHand Hand { get; private set; }       
        public EPoint Point { get; private set; }
        public EServer Server { get; private set; }
        public ECrunch Crunch { get; private set; }
        public HashSet<int> SelectedSets { get; private set; }
        public HashSet<int> SelectedRallyLengths { get; private set; }
        public EQuality Quality { get; private set; }

        public enum EHand
        {
            Fore,
            Back,
            None,
            Both
        }
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

        public enum StrokeTec
        {
            Push,
            PushAggressive,
            Flip,
            Banana,
            Topspin,
            TopspinSpin,
            TopspinTempo,
            Block,
            BlockTempo,
            BlockChop,
        }
        public enum EQuality
        {
            Bad,
            Good,
            None,
            Both
        }


        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public ReceptionViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedRallies = new List<MatchRally>();
            Match = new Match();
            Hand = EHand.None;
            Point = EPoint.None;
            Server = EServer.None;
            Crunch = ECrunch.Not;
            SelectedSets = new HashSet<int>();
            SelectedRallyLengths = new HashSet<int>();
            Quality = EQuality.None;

            TableView = new TableServiceViewModel(this.events);
        }


        #region View Methods
        public void SwitchTable(bool check)
        {
            if (check)
            {
                TableView.Mode = TableServiceViewModel.ViewMode.Top;
            }
            else
            {
                TableView.Mode = TableServiceViewModel.ViewMode.Bottom;
            }
        }
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
        public void ForBackHand(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("forhand"))
            {
                if (source.IsChecked.Value)
                {
                    if (Hand == EHand.None)
                        Hand = EHand.Fore;
                    else if (Hand == EHand.Back)
                        Hand = EHand.Both;
                }
                else
                {
                    if (Hand == EHand.Fore)
                        Hand = EHand.None;
                    else if (Hand == EHand.Both)
                        Hand = EHand.Back;
                }
            }
            else if (source.Name.ToLower().Contains("backhand"))
            {
                if (source.IsChecked.Value)
                {
                    if (Hand == EHand.None)
                        Hand = EHand.Back;
                    else if (Hand == EHand.Fore)
                        Hand = EHand.Both;
                }
                else
                {
                    if (Hand == EHand.Back)
                        Hand = EHand.None;
                    else if (Hand == EHand.Both)
                        Hand = EHand.Fore;
                }
            }
            UpdateSelection();
        }
        public void GoodBadQuality(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("goodq"))
            {
                if (source.IsChecked.Value)
                {
                    if (Quality == EQuality.None)
                        Quality = EQuality.Good;
                    else if (Quality == EQuality.Bad)
                        Quality = EQuality.Both;
                }
                else
                {
                    if (Quality == EQuality.Good)
                        Quality = EQuality.None;
                    else if (Quality == EQuality.Both)
                        Quality = EQuality.Bad;
                }
            }
            else if (source.Name.ToLower().Contains("badq"))
            {
                if (source.IsChecked.Value)
                {
                    if (Quality == EQuality.None)
                        Quality = EQuality.Bad;
                    else if (Quality == EQuality.Good)
                        Quality = EQuality.Both;
                }
                else
                {
                    if (Quality == EQuality.Bad)
                        Quality = EQuality.None;
                    else if (Quality == EQuality.Both)
                        Quality = EQuality.Good;
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

            this.ActivateItem(TableView);
        }
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);            
            this.DeactivateItem(TableView, close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }
        #endregion

        #region Event Handlers
        public void Handle(FilterSwitchedEvent message)
        {
            this.Match = message.Match;
            SelectedRallies = this.Match.Rallies.Where(r => r.Schlag.Length > 1).ToList();
            this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
        }

        public void Handle(TableViewSelectionChangedEvent message)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helper Methods

        private void UpdateSelection()
        {
            if (this.Match.Rallies != null)
            {
                SelectedRallies = this.Match.Rallies.Where(r => Convert.ToInt32(r.Length) > 1 && HasSet(r) && HasCrunchTime(r) && HasPoint(r) && HasServer(r) && HasRallyLength(r) && HasHand(r) && HasQuality(r)  ).ToList();
                this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
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

        private bool HasServer(MatchRally r)
        {
            switch (this.Server)
            {
                case EServer.Player1:
                    return r.Schlag[1].Spieler == "First";  //TODO Name der Spieler dynamisch????
                case EServer.Player2:
                    return r.Schlag[1].Spieler == "Second"; //TODO Name der Spieler dynamisch????
                case EServer.None:
                    return true;
                case EServer.Both:
                    return true;
                default:
                    return false;
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

        private bool HasHand(MatchRally r)
        {
            switch (this.Hand)
            {
                case EHand.Fore:
                    return r.Schlag[1].Schlägerseite == "Vorhand";
                case EHand.Back:
                    return r.Schlag[1].Schlägerseite == "Rückhand";
                case EHand.None:
                    return true;
                case EHand.Both:
                    return true;
                default:
                    return false;
            }
        }

        private bool HasQuality(MatchRally r)
        {
            switch (this.Quality)
            {
                case EQuality.Good:
                    return r.Schlag[1].Qualität == "gut";
                case EQuality.Bad:
                    return r.Schlag[1].Qualität == "schlecht";
                case EQuality.None:
                    return true;
                case EQuality.Both:
                    return r.Schlag[1].Qualität == "gut" || r.Schlag[0].Qualität == "schlecht";
                default:
                    return false;
            }
        }

        #endregion
    }
}
