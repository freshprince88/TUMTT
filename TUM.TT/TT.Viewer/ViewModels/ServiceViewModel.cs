using Caliburn.Micro;
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
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class ServiceViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<TableViewSelectionChangedEvent>,
        IHandle<SpinControlSelectionChangedEvent>,
        IHandle<FilterSelectionChangedEvent>
    {

        #region Properties
        public BasicFilterViewModel BasicFilterView { get; set; }
        public SpinControlViewModel SpinControl { get; private set; }
        public TableServiceViewModel TableView { get; private set; }
        public List<Rally> SelectedRallies { get; private set; }
        public List<Stroke.Spin> SelectedSpins { get; private set; }
        public Stroke.Hand Hand { get; private set; }       
        public Stroke.Quality Quality { get; private set; }
        public Stroke.Specials Specials { get; private set; }

        public HashSet<Stroke.Services> SelectedServices { get; private set; }
        public HashSet<Positions.Table> SelectedTablePositions { get; set; }
        public HashSet<Positions.Server> SelectedServerPositions { get; set; }

        #endregion


        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public ServiceViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            SelectedRallies = new List<Rally>();
            SelectedSpins = new List<Stroke.Spin>();
            Hand = Stroke.Hand.None;       
            Quality = Stroke.Quality.None;
            Specials = Stroke.Specials.None;          
            SelectedServices = new HashSet<Stroke.Services>();
            SelectedServerPositions = new HashSet<Positions.Server>();
            SelectedTablePositions = new HashSet<Positions.Table>();
            SpinControl = new SpinControlViewModel(events);
            BasicFilterView = new BasicFilterViewModel(this.events)
            {
                MinRallyLength = 0,
                PlayerLabel = "Aufschlag:",
                LastStroke = false,
                StrokeNumber=0
                
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
                    SelectedServices.Add(Stroke.Services.Pendulum);
                }
                else
                {
                    SelectedServices.Remove(Stroke.Services.Pendulum);
                }
            }
            else if (source.Name.ToLower().Contains("reverse"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedServices.Add(Stroke.Services.Reverse);
                }
                else
                {
                    SelectedServices.Remove(Stroke.Services.Reverse);
                }
            }
            else if (source.Name.ToLower().Contains("tomahawk"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedServices.Add(Stroke.Services.Tomahawk);
                }
                else
                {
                    SelectedServices.Remove(Stroke.Services.Tomahawk);
                }
            }
            else if (source.Name.ToLower().Contains("special"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedServices.Add(Stroke.Services.Special);
                }
                else
                {
                    SelectedServices.Remove(Stroke.Services.Special);
                }
            }
            UpdateSelection(Manager.ActivePlaylist);

        }

        public void SwitchTable(bool check)
        {
            if (check)
            {
                TableView.Mode = ViewMode.Position.Top;
            }
            else
            {
                TableView.Mode = ViewMode.Position.Bottom;
            }
        }
       
        public void ForBackHand(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("forhand"))
            {
                if (source.IsChecked.Value)
                {
                    if (Hand == Stroke.Hand.None)
                        Hand = Stroke.Hand.Fore;
                    else if (Hand == Stroke.Hand.Back)
                        Hand = Stroke.Hand.Both;
                }
                else
                {
                    if (Hand == Stroke.Hand.Fore)
                        Hand = Stroke.Hand.None;
                    else if (Hand == Stroke.Hand.Both)
                        Hand = Stroke.Hand.Back;
                }
            }
            else if (source.Name.ToLower().Contains("backhand"))
            {
                if (source.IsChecked.Value)
                {
                    if (Hand == Stroke.Hand.None)
                        Hand = Stroke.Hand.Back;
                    else if (Hand == Stroke.Hand.Fore)
                        Hand = Stroke.Hand.Both;
                }
                else
                {
                    if (Hand == Stroke.Hand.Back)
                        Hand = Stroke.Hand.None;
                    else if (Hand == Stroke.Hand.Both)
                        Hand = Stroke.Hand.Fore;
                }
            }
            UpdateSelection(Manager.ActivePlaylist);
        }

        public void GoodBadQuality(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("goodq"))
            {
                if (source.IsChecked.Value)
                {
                    if (Quality == Stroke.Quality.None)
                        Quality = Stroke.Quality.Good;
                    else if (Quality == Stroke.Quality.Bad)
                        Quality = Stroke.Quality.Both;
                }
                else
                {
                    if (Quality == Stroke.Quality.Good)
                        Quality = Stroke.Quality.None;
                    else if (Quality == Stroke.Quality.Both)
                        Quality = Stroke.Quality.Bad;
                }
            }
            else if (source.Name.ToLower().Contains("badq"))
            {
                if (source.IsChecked.Value)
                {
                    if (Quality == Stroke.Quality.None)
                        Quality = Stroke.Quality.Bad;
                    else if (Quality == Stroke.Quality.Good)
                        Quality = Stroke.Quality.Both;
                }
                else
                {
                    if (Quality == Stroke.Quality.Bad)
                        Quality = Stroke.Quality.None;
                    else if (Quality == Stroke.Quality.Both)
                        Quality = Stroke.Quality.Good;
                }
            }
            UpdateSelection(Manager.ActivePlaylist);
        }

        public void EdgeSpecials(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("edgetable"))
            {
                if (source.IsChecked.Value)
                {
                    if (Specials == Stroke.Specials.None)
                        Specials = Stroke.Specials.EdgeTable;
                    else if (Specials == Stroke.Specials.EdgeRacket)
                        Specials = Stroke.Specials.Both;
                }
                else
                {
                    if (Specials == Stroke.Specials.EdgeTable)
                        Specials = Stroke.Specials.None;
                    else if (Specials == Stroke.Specials.Both)
                        Specials = Stroke.Specials.EdgeRacket;
                }
            }
            else if (source.Name.ToLower().Contains("edgeracket"))
            {
                if (source.IsChecked.Value)
                {
                    if (Specials == Stroke.Specials.None)
                        Specials = Stroke.Specials.EdgeRacket;
                    else if (Specials == Stroke.Specials.EdgeTable)
                        Specials = Stroke.Specials.Both;
                }
                else
                {
                    if (Specials == Stroke.Specials.EdgeRacket)
                        Specials = Stroke.Specials.None;
                    else if (Specials == Stroke.Specials.Both)
                        Specials = Stroke.Specials.EdgeTable;
                }
            }
            UpdateSelection(Manager.ActivePlaylist);
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

            UpdateSelection(Manager.ActivePlaylist);
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
            UpdateSelection(Manager.ActivePlaylist);
        }

        public void Handle(TableViewSelectionChangedEvent message)
        {
            SelectedServerPositions = message.PlayerPositions;
            SelectedTablePositions = message.Positions;
            UpdateSelection(Manager.ActivePlaylist);
        }

        public void Handle(SpinControlSelectionChangedEvent message)
        {
            SelectedSpins = message.Selected;
            UpdateSelection(Manager.ActivePlaylist);
        }

        #endregion

        #region Helper Methods

        private void UpdateSelection(Playlist list)
        {
            if (list.Rallies != null)
            {
                SelectedRallies = BasicFilterView.SelectedRallies.Where(r => HasSpins(r) && HasHand(r) && HasServices(r)  && HasQuality(r) && HasSpecials(r) && HasTablePosition(r) && HasServerPosition(r)).ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(SelectedRallies));
            }
        }

        private bool HasSpins(Rally r)
        {
            List<bool> ORresults = new List<bool>();
            Schlag service = r.Schlag.Where(s => s.Nummer == "1").FirstOrDefault();

            foreach (var spin in SelectedSpins)
            {
                switch (spin)
                {
                    case Stroke.Spin.Hidden:
                        ORresults.Add(service.Spin.ÜS == "" || service.Spin.SL == "" || service.Spin.SR == "" || service.Spin.US=="" || service.Spin.No=="" );
                        break;
                    case Stroke.Spin.ÜS:
                        ORresults.Add(service.Spin.ÜS == "1" && service.Spin.SL == "0" && service.Spin.SR == "0");
                        break;
                    case Stroke.Spin.SR:
                        ORresults.Add(service.Spin.SR == "1" && service.Spin.ÜS == "0" && service.Spin.US == "0");
                        break;
                    case Stroke.Spin.No:
                        ORresults.Add(service.Spin.No == "1" && service.Spin.SL == "0" && service.Spin.SR == "0" && service.Spin.ÜS == "0" && service.Spin.US == "0");
                        break;
                    case Stroke.Spin.SL:
                        ORresults.Add(service.Spin.SL == "1" && service.Spin.ÜS == "0" && service.Spin.US == "0");
                        break;
                    case Stroke.Spin.US:
                        ORresults.Add(service.Spin.US == "1" && service.Spin.SL == "0" && service.Spin.SR == "0");
                        break;
                    case Stroke.Spin.USSL:
                        ORresults.Add(service.Spin.US == "1" && service.Spin.SL == "1");
                        break;
                    case Stroke.Spin.USSR:
                        ORresults.Add(service.Spin.US == "1" && service.Spin.SR == "1");
                        break;
                    case Stroke.Spin.ÜSSL:
                        ORresults.Add(service.Spin.ÜS == "1" && service.Spin.SL == "1");
                        break;
                    case Stroke.Spin.ÜSSR:
                        ORresults.Add(service.Spin.ÜS == "1" && service.Spin.SR == "1");
                        break;
                    default:
                        break;

                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasServices(Rally r)
        {
            List<bool> ORresults = new List<bool>();

            foreach (var service in SelectedServices)
            {
                switch (service)
                {
                    case Stroke.Services.Pendulum:
                        ORresults.Add(r.Schlag[0].Aufschlagart=="Pendulum");
                        break;
                    case Stroke.Services.Reverse:
                        ORresults.Add(r.Schlag[0].Aufschlagart == "Gegenläufer");
                        break;
                    case Stroke.Services.Tomahawk:
                        ORresults.Add(r.Schlag[0].Aufschlagart == "Tomahawk");
                        break;
                    case Stroke.Services.Special:
                        ORresults.Add(r.Schlag[0].Aufschlagart == "Spezial");
                        break;
                    default:
                        break;

                }

            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasHand(Rally r)
        {
            switch (this.Hand)
            {
                case Stroke.Hand.Fore:
                    return r.Schlag[0].Schlägerseite == "Vorhand";
                case Stroke.Hand.Back:
                    return r.Schlag[0].Schlägerseite == "Rückhand";
                case Stroke.Hand.None:
                    return true;
                case Stroke.Hand.Both:
                    return true;
                default:
                    return false;
            }
        }

       
        private bool HasQuality(Rally r)
        {
            switch (this.Quality)
            {
                case Stroke.Quality.Good:
                    return r.Schlag[0].Qualität == "gut";  
                case Stroke.Quality.Bad:
                    return r.Schlag[0].Qualität == "schlecht"; 
                case Stroke.Quality.None:
                    return true;
                case Stroke.Quality.Both:
                    return r.Schlag[0].Qualität == "gut" || r.Schlag[0].Qualität == "schlecht";
                default:
                    return false;
            }
        }

        private bool HasSpecials(Rally r)
        {
            switch (this.Specials)
            {
                case Stroke.Specials.EdgeTable:
                    return r.Schlag[0].Besonderes == "Tischkante";  
                case Stroke.Specials.EdgeRacket:
                    return r.Schlag[0].Besonderes == "Schlägerkante"; 
                case Stroke.Specials.None:
                    return true;
                case Stroke.Specials.Both:
                    return r.Schlag[0].Besonderes == "Tischkante" || r.Schlag[0].Besonderes == "Schlägerkante";
                default:
                    return false;
            }
        }


        private bool HasTablePosition(Rally r)
        {
            List<bool> ORresults = new List<bool>();
            Schlag service = r.Schlag.Where(s => Convert.ToInt32(s.Nummer) == 1).FirstOrDefault();
            foreach (var sel in SelectedTablePositions)
            {
                switch (sel)
                {
                    case Positions.Table.TopLeft:
                        ORresults.Add(service.IsTopLeft());
                        break;
                    case Positions.Table.TopMid:
                        ORresults.Add(service.IsTopMid());
                        break;
                    case Positions.Table.TopRight:
                        ORresults.Add(service.IsTopRight());
                        break;
                    case Positions.Table.MidLeft:
                        ORresults.Add(service.IsMidLeft());
                        break;
                    case Positions.Table.MidMid:
                        ORresults.Add(service.IsMidMid());
                        break;
                    case Positions.Table.MidRight:
                        ORresults.Add(service.IsMidRight());
                        break;
                    case Positions.Table.BotLeft:
                        ORresults.Add(service.IsBotLeft());
                        break;
                    case Positions.Table.BotMid:
                        ORresults.Add(service.IsBotMid());
                        break;
                    case Positions.Table.BotRight:
                        ORresults.Add(service.IsBotRight());
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasServerPosition(Rally r)
        {
            List<bool> ORresults = new List<bool>();
            Schlag service = r.Schlag.Where(s => s.Nummer == "1").FirstOrDefault();
            double X;
            double Seite = service.Platzierung.WY == "" ? 999 : Convert.ToDouble(service.Platzierung.WY);
            if (Seite >= 137)
            {

                X = 152.5 - (service.Spielerposition == "" ? 999 : Convert.ToDouble(service.Spielerposition));
            }
            else
            {
                X = service.Spielerposition == "" ? 999 : Convert.ToDouble(service.Spielerposition);
            }
            foreach (var sel in SelectedServerPositions)
            {
                switch (sel)
                {
                    case Positions.Server.Left:
                        ORresults.Add(0 <=X && X <= 30.5);
                        break;
                    case Positions.Server.HalfLeft:
                        ORresults.Add(30.5 < X && X <= 61);
                        break;
                    case Positions.Server.Mid:
                        ORresults.Add(61 < X && X <= 91.5);
                        break;
                    case Positions.Server.HalfRight:
                        ORresults.Add(91.5 < X && X <= 122);
                        break;
                    case Positions.Server.Right:
                        ORresults.Add(122 < X && X <= 152.5);
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
