using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Models;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class ServiceViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<MatchOpenedEvent>,
        IHandle<TableViewSelectionChangedEvent>,
        IHandle<SpinControlSelectionChangedEvent>
    {
        public SpinControlViewModel SpinControl { get; private set; }
        public TableViewModel TableView { get; private set; }
        public List<MatchRally> SelectedRallies { get; private set; }

        public Match Match { get; private set; }
        public List<SpinControlViewModel.Spins> SelectedSpins { get; private set; }
        public EHand Hand { get; private set; }
        public EPoint Point { get; private set; }
        public EServer Server { get; private set; }
        public EQuality Quality { get; private set; }
        public ESpecials Specials { get; private set; }
        public ECrunch Crunch { get; private set; }

        public HashSet<int> SelectedSets { get; private set; }
        public HashSet<int> SelectedRallyLengths { get; private set; }



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
        public enum EQuality
        {
            Bad,
            Good,
            None,
            Both
        }
        public enum ESpecials
        {
            EdgeTable,
            EdgeRacket,
            None,
            Both
        }
        public enum ECrunch
        {
            CrunshTime,
            Not
        }


        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public ServiceViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedRallies = new List<MatchRally>();
            SelectedSpins = new List<SpinControlViewModel.Spins>();
            Match = new Match();
            Hand = EHand.None;
            Point = EPoint.None;
            Server = EServer.None;
            Quality = EQuality.None;
            Specials = ESpecials.None;
            SelectedSets = new HashSet<int>();
            SelectedRallyLengths = new HashSet<int>();

            SpinControl = new SpinControlViewModel(events);
            TableView = new TableViewModel(events);
        }

        #region View Methods

        public void SwitchTable(bool check)
        {
            if (check)
            {
                TableView.Mode = TableViewModel.ViewMode.Top;
            }
            else
            {
                TableView.Mode = TableViewModel.ViewMode.Bottom;
            }
        }
        public void SetFilter(int set, bool isChecked)
        {
            if (set == 0)
            {
                if (!isChecked)
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
            else if (set == 1)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(1);
                }
                else
                {
                    SelectedSets.Remove(1);
                }
            }
            else if (set == 2)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(2);
                }
                else
                {
                    SelectedSets.Remove(2);
                }
            }
            else if (set == 3)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(3);
                }
                else
                {
                    SelectedSets.Remove(3);
                }
            }
            else if (set == 4)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(4);
                }
                else
                {
                    SelectedSets.Remove(4);
                }
            }
            else if (set == 5)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(5);
                }
                else
                {
                    SelectedSets.Remove(5);
                }
            }
            else if (set == 6)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(6);
                }
                else
                {
                    SelectedSets.Remove(6);
                }
            }
            else if (set == 7)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(7);
                }
                else
                {
                    SelectedSets.Remove(7);
                }
            }
        }

        public void RallyLengthFilter(int rallylength, bool isChecked)
        {
            if (rallylength == 0)
            {
                if (!isChecked)
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
            else if (rallylength == 1)
            {
                if (!isChecked)
                {
                    SelectedRallyLengths.Add(1);
                }
                else
                {
                    SelectedRallyLengths.Remove(1);
                }
            }
            else if (rallylength == 2)
            {
                if (!isChecked)
                {
                    SelectedRallyLengths.Add(2);
                }
                else
                {
                    SelectedRallyLengths.Remove(2);
                }
            }
            else if (rallylength == 3)
            {
                if (!isChecked)
                {
                    SelectedRallyLengths.Add(3);
                }
                else
                {
                    SelectedRallyLengths.Remove(3);
                }
            }
            else if (rallylength == 4)
            {
                if (!isChecked)
                {
                    SelectedRallyLengths.Add(4);
                }
                else
                {
                    SelectedRallyLengths.Remove(4);
                }
            }
            else if (rallylength == 5)
            {
                if (!isChecked)
                {
                    SelectedRallyLengths.Add(5);
                }
                else
                {
                    SelectedRallyLengths.Remove(5);
                }
            }
            else if (rallylength == 6)
            {
                if (!isChecked)
                {
                    SelectedRallyLengths.Add(6);
                }
                else
                {
                    SelectedRallyLengths.Remove(6);
                }
            }
            
        }
        public void CrunchOrNot (string crunch, bool isChecked)
        {
            if (crunch == "crunch")
            {
                if (!isChecked)
                {
                    Crunch = ECrunch.CrunshTime;
                }
                else
                {
                    Crunch = ECrunch.Not;
                }
            }
        }

        public void ForBackHand(string hand, bool isChecked)
        {
            if (hand == "fore")
            {
                if (!isChecked)
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
            else if (hand == "back")
            {
                if (!isChecked)
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
        }
        public void P1P2Point (string player, bool isChecked)
        {
            if (player == "player1")
            {
                if (!isChecked)
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
            else if (player == "player2")
            {
                if (!isChecked)
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
        }
        public void P1P2Server(string server, bool isChecked)
        {
            if (server == "player1")
            {
                if (!isChecked)
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
            else if (server == "player2")
            {
                if (!isChecked)
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
        }
        public void GoodBadQuality(string quality, bool isChecked)
        {
            if (quality == "good")
            {
                if (!isChecked)
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
            else if (quality == "bad")
            {
                if (!isChecked)
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
        }
        public void EdgeSpecials(string edge, bool isChecked)
        {
            if (edge == "edgeTable")
            {
                if (!isChecked)
                {
                    if (Specials == ESpecials.None)
                        Specials = ESpecials.EdgeTable;
                    else if (Specials == ESpecials.EdgeRacket)
                        Specials = ESpecials.Both;
                }
                else
                {
                    if (Specials == ESpecials.EdgeTable)
                        Specials = ESpecials.None;
                    else if (Specials == ESpecials.Both)
                        Specials = ESpecials.EdgeRacket;
                }
            }
            else if (edge == "edgeRacket")
            {
                if (!isChecked)
                {
                    if (Specials == ESpecials.None)
                        Specials = ESpecials.EdgeRacket;
                    else if (Specials == ESpecials.EdgeTable)
                        Specials = ESpecials.Both;
                }
                else
                {
                    if (Specials == ESpecials.EdgeRacket)
                        Specials = ESpecials.None;
                    else if (Specials == ESpecials.Both)
                        Specials = ESpecials.EdgeTable;
                }
            }
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

            this.ActivateItem(SpinControl);
            this.ActivateItem(TableView);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            this.DeactivateItem(SpinControl, close);
            this.DeactivateItem(TableView, close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }

        #endregion

        #region Event Handlers

        public void Handle(MatchOpenedEvent message)
        {
            this.Match = message.Match;
            SelectedRallies = this.Match.Rallies.Where(r => r.Schlag.Length > 0).ToList();
            this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
        }

        public void Handle(TableViewSelectionChangedEvent message)
        {
            throw new NotImplementedException();
        }

        public void Handle(SpinControlSelectionChangedEvent message)
        {
            SelectedSpins = message.Selected;
            UpdateSelection();
        }

        #endregion

        #region Helper Methods

        private void UpdateSelection()
        {
            if (this.Match.Rallies != null)
            {
                SelectedRallies = this.Match.Rallies.Where(r => Convert.ToInt32(r.Length) > 0 && HasSpins(r) && HasHand(r) && HasSet(r) && HasCrunchTime(r) && HasPoint(r) && HasServer(r) && HasQuality(r) && HasSpecials(r)).ToList();
                this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
            }
        }

        private bool HasSpins(MatchRally r)
        {
            List<bool> ORresults = new List<bool>();
            MatchRallySchlag service = r.Schlag.Where(s => s.Nummer == "1").FirstOrDefault();

            foreach (var spin in SelectedSpins)
            {
                switch (spin)
                {
                    case SpinControlViewModel.Spins.ÜS:
                        ORresults.Add(service.Spin.ÜS == "1");
                        break;
                    case SpinControlViewModel.Spins.SR:
                        ORresults.Add(service.Spin.SR == "1");
                        break;
                    case SpinControlViewModel.Spins.No:
                        ORresults.Add(service.Spin.No == "1");
                        break;
                    case SpinControlViewModel.Spins.SL:
                        ORresults.Add(service.Spin.SL == "1");
                        break;
                    case SpinControlViewModel.Spins.US:
                        ORresults.Add(service.Spin.US == "1");
                        break;
                    case SpinControlViewModel.Spins.USSL:
                        ORresults.Add(service.Spin.US == "1" && service.Spin.SL == "1");
                        break;
                    case SpinControlViewModel.Spins.USSR:
                        ORresults.Add(service.Spin.US == "1" && service.Spin.SR == "1");
                        break;
                    case SpinControlViewModel.Spins.ÜSSL:
                        ORresults.Add(service.Spin.ÜS == "1" && service.Spin.SL == "1");
                        break;
                    case SpinControlViewModel.Spins.ÜSSR:
                        ORresults.Add(service.Spin.ÜS == "1" && service.Spin.SR == "1");
                        break;
                    default:
                        break;
                }
            }

            return ORresults.Count == 0 ? false : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasHand(MatchRally r)
        {
            switch (this.Hand)
            {
                case EHand.Fore:
                    return r.Schlag[0].Schlägerseite == "Vorhand";
                case EHand.Back:
                    return r.Schlag[0].Schlägerseite == "Rückhand";
                case EHand.None:
                    return true;
                case EHand.Both:
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
        private bool HasServer(MatchRally r)
        {
            switch (this.Server)
            {
                case EServer.Player1:
                    return r.Schlag[0].Spieler == "First";  //TODO Name der Spieler dynamisch????
                case EServer.Player2:
                    return r.Schlag[0].Spieler == "Second"; //TODO Name der Spieler dynamisch????
                case EServer.None:
                    return true;
                case EServer.Both:
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
                    return r.Schlag[0].Qualität == "gut";  
                case EQuality.Bad:
                    return r.Schlag[0].Qualität == "schlecht"; 
                case EQuality.None:
                    return true;
                case EQuality.Both:
                    return r.Schlag[0].Qualität == "gut" || r.Schlag[0].Qualität == "schlecht";
                default:
                    return false;
            }
        }
        private bool HasSpecials(MatchRally r)
        {
            switch (this.Specials)
            {
                case ESpecials.EdgeTable:
                    return r.Schlag[0].Besonderes == "Tischkante";  
                case ESpecials.EdgeRacket:
                    return r.Schlag[0].Besonderes == "Schlägerkante"; 
                case ESpecials.None:
                    return true;
                case ESpecials.Both:
                    return r.Schlag[0].Besonderes == "Tischkante" || r.Schlag[0].Besonderes == "Schlägerkante";
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
            return ORresults.Count == 0 ? false : ORresults.Aggregate(false, (a, b) => a || b);
        }
        private bool HasRallyLength(MatchRally r)      //TODO Korrekt????
        {
            List<bool> ORresults = new List<bool>();

            foreach (var rallylength in SelectedSets)
            {  if (rallylength <= 5)
                {
                    ORresults.Add(Convert.ToInt32(r.Length) == rallylength);
                }
                
            else if (rallylength==6)
                {
                    ORresults.Add(Convert.ToInt32(r.Length) >= rallylength);
                }


            }
            return ORresults.Count == 0 ? false : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasCrunchTime(MatchRally r)
        {
            switch (this.Crunch)
            {
                case ECrunch.CrunshTime:
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
