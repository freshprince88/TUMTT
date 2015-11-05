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
    public class ServiceViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<FilterSwitchedEvent>,
        IHandle<TableViewSelectionChangedEvent>,
        IHandle<SpinControlSelectionChangedEvent>,
        IHandle<FilterSelectionChangedEvent>
    {

        #region Properties
        public BasicFilterViewModel BasicFilterView { get; set; }
        public SpinControlViewModel SpinControl { get; private set; }
        public TableServiceViewModel TableView { get; private set; }
        public List<MatchRally> SelectedRallies { get; private set; }
        public Match Match { get; private set; }
        public List<SpinControlViewModel.Spins> SelectedSpins { get; private set; }
        public EHand Hand { get; private set; }       
        public EQuality Quality { get; private set; }
        public ESpecials Specials { get; private set; }

        public HashSet<Services> SelectedServices { get; private set; }
        public HashSet<TableServiceViewModel.ETablePosition> SelectedTablePositions { get; set; }
        public HashSet<TableServiceViewModel.EServerPosition> SelectedServerPositions { get; set; }

        #endregion

        #region Enums

        public enum EHand
        {
            Fore,
            Back,
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

       
        public enum Services
        {
            Pendulum,
            Reverse,
            Tomahawk,
            Special
        }

        #endregion

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
            Quality = EQuality.None;
            Specials = ESpecials.None;          
            SelectedServices = new HashSet<Services>();
            SelectedServerPositions = new HashSet<TableServiceViewModel.EServerPosition>();
            SelectedTablePositions = new HashSet<TableServiceViewModel.ETablePosition>();
            SpinControl = new SpinControlViewModel(events);
            BasicFilterView = new BasicFilterViewModel(this.events)
            {
                MinRallyLength = 0,
                PlayerLabel = "Aufschlag:"
            };
            TableView = new TableServiceViewModel(events);
        }

        #region View Methods
        public void SelectService(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("pendulum"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedServices.Add(Services.Pendulum);
                }
                else
                {
                    SelectedServices.Remove(Services.Pendulum);
                }
            }
            else if (source.Name.ToLower().Contains("reverse"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedServices.Add(Services.Reverse);
                }
                else
                {
                    SelectedServices.Remove(Services.Reverse);
                }
            }
            else if (source.Name.ToLower().Contains("tomahawk"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedServices.Add(Services.Tomahawk);
                }
                else
                {
                    SelectedServices.Remove(Services.Tomahawk);
                }
            }
            else if (source.Name.ToLower().Contains("special"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedServices.Add(Services.Special);
                }
                else
                {
                    SelectedServices.Remove(Services.Special);
                }
            }
            UpdateSelection();

        }

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

        public void EdgeSpecials(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("edgetable"))
            {
                if (source.IsChecked.Value)
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
            else if (source.Name.ToLower().Contains("edgeracket"))
            {
                if (source.IsChecked.Value)
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

            this.ActivateItem(SpinControl);
            this.ActivateItem(TableView);
            this.ActivateItem(BasicFilterView);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            this.DeactivateItem(SpinControl, close);
            this.DeactivateItem(TableView, close);
            this.DeactivateItem(BasicFilterView, close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }

        #endregion

        #region Event Handlers

        //FilterSelection in BasicFilter Changed
        //Get SelectedRallies and apply own filters
        public void Handle(FilterSelectionChangedEvent message)
        {
            UpdateSelection();
        }
        public void Handle(FilterSwitchedEvent message)
        {
            this.Match = message.Match;
            if (this.Match.FirstPlayer != null && this.Match.SecondPlayer != null)
            {
                
            }
            UpdateSelection();
        }

        public void Handle(TableViewSelectionChangedEvent message)
        {
            SelectedServerPositions = message.PlayerPositions;
            SelectedTablePositions = message.Positions;
            UpdateSelection();
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
                SelectedRallies = BasicFilterView.SelectedRallies.Where(r => HasSpins(r) && HasHand(r) && HasServices(r)  && HasQuality(r) && HasSpecials(r) && HasTablePosition(r) && HasServerPosition(r)).ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(SelectedRallies));
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
                    case SpinControlViewModel.Spins.Hidden:
                        ORresults.Add(service.Spin.ÜS == "" || service.Spin.SL == "" || service.Spin.SR == "" || service.Spin.US=="" || service.Spin.No=="" );
                        break;
                    case SpinControlViewModel.Spins.ÜS:
                        ORresults.Add(service.Spin.ÜS == "1" && service.Spin.SL == "0" && service.Spin.SR == "0");
                        break;
                    case SpinControlViewModel.Spins.SR:
                        ORresults.Add(service.Spin.SR == "1" && service.Spin.ÜS == "0" && service.Spin.US == "0");
                        break;
                    case SpinControlViewModel.Spins.No:
                        ORresults.Add(service.Spin.No == "1" && service.Spin.SL == "0" && service.Spin.SR == "0" && service.Spin.ÜS == "0" && service.Spin.US == "0");
                        break;
                    case SpinControlViewModel.Spins.SL:
                        ORresults.Add(service.Spin.SL == "1" && service.Spin.ÜS == "0" && service.Spin.US == "0");
                        break;
                    case SpinControlViewModel.Spins.US:
                        ORresults.Add(service.Spin.US == "1" && service.Spin.SL == "0" && service.Spin.SR == "0");
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
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasServices(MatchRally r)
        {
            List<bool> ORresults = new List<bool>();

            foreach (var service in SelectedServices)
            {
                switch (service)
                {
                    case Services.Pendulum:
                        ORresults.Add(r.Schlag[0].Aufschlagart=="Pendulum");
                        break;
                    case Services.Reverse:
                        ORresults.Add(r.Schlag[0].Aufschlagart == "Gegenläufer");
                        break;
                    case Services.Tomahawk:
                        ORresults.Add(r.Schlag[0].Aufschlagart == "Tomahawk");
                        break;
                    case Services.Special:
                        ORresults.Add(r.Schlag[0].Aufschlagart == "Spezial");
                        break;
                    default:
                        break;

                }

            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
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


        private bool HasTablePosition(MatchRally r)
        {
            List<bool> ORresults = new List<bool>();
            MatchRallySchlag service = r.Schlag.Where(s => Convert.ToInt32(s.Nummer) == 1).FirstOrDefault();
            foreach (var sel in SelectedTablePositions)
            {
                switch (sel)
                {
                    case TableServiceViewModel.ETablePosition.TopLeft:
                        ORresults.Add(service.IsTopLeft());
                        break;
                    case TableServiceViewModel.ETablePosition.TopMid:
                        ORresults.Add(service.IsTopMid());
                        break;
                    case TableServiceViewModel.ETablePosition.TopRight:
                        ORresults.Add(service.IsTopRight());
                        break;
                    case TableServiceViewModel.ETablePosition.MidLeft:
                        ORresults.Add(service.IsMidLeft());
                        break;
                    case TableServiceViewModel.ETablePosition.MidMid:
                        ORresults.Add(service.IsMidMid());
                        break;
                    case TableServiceViewModel.ETablePosition.MidRight:
                        ORresults.Add(service.IsMidRight());
                        break;
                    case TableServiceViewModel.ETablePosition.BotLeft:
                        ORresults.Add(service.IsBotLeft());
                        break;
                    case TableServiceViewModel.ETablePosition.BotMid:
                        ORresults.Add(service.IsBotMid());
                        break;
                    case TableServiceViewModel.ETablePosition.BotRight:
                        ORresults.Add(service.IsBotRight());
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasServerPosition(MatchRally r)
        {
            List<bool> ORresults = new List<bool>();
            MatchRallySchlag service = r.Schlag.Where(s => s.Nummer == "1").FirstOrDefault();
            double X = service.Spielerposition == "" ? 999 : Convert.ToDouble(service.Spielerposition);

            foreach (var sel in SelectedServerPositions)
            {
                switch (sel)
                {
                    case TableServiceViewModel.EServerPosition.Left:
                        ORresults.Add(X <= 30.5);
                        break;
                    case TableServiceViewModel.EServerPosition.HalfLeft:
                        ORresults.Add(X <= 61);
                        break;
                    case TableServiceViewModel.EServerPosition.Mid:
                        ORresults.Add(X <= 91.5);
                        break;
                    case TableServiceViewModel.EServerPosition.HalfRight:
                        ORresults.Add(X <= 122);
                        break;
                    case TableServiceViewModel.EServerPosition.Right:
                        ORresults.Add(X <= 152.5);
                        break;
                    default:
                        break;
                }
            }

            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        #endregion
        }
}
