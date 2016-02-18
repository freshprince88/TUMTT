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
        IHandle<BasicFilterSelectionChangedEvent>
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

            BasicFilterView = new BasicFilterViewModel(this.events, Manager)
            {
                MinRallyLength = 0,
                PlayerLabel = "Aufschlag:",
                LastStroke = false,
                StrokeNumber = 0

            };
            TableView = new TableServiceViewModel(events);

            SelectedRallies = new List<Rally>();
            SelectedSpins = new List<Stroke.Spin>();
            Hand = Stroke.Hand.None;       
            Quality = Stroke.Quality.None;
            Specials = Stroke.Specials.None;          
            SelectedServices = new HashSet<Stroke.Services>();
            SelectedServerPositions = new HashSet<Positions.Server>();
            SelectedTablePositions = new HashSet<Positions.Table>();
            SpinControl = new SpinControlViewModel(events);
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
            if (source.Name.ToLower().Contains("forehand"))
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
                    else if (Specials == Stroke.Specials.EdgeNet)
                        Specials = Stroke.Specials.Both;
                }
                else
                {
                    if (Specials == Stroke.Specials.EdgeTable)
                        Specials = Stroke.Specials.None;
                    else if (Specials == Stroke.Specials.Both)
                        Specials = Stroke.Specials.EdgeNet;
                }
            }
            else if (source.Name.ToLower().Contains("edgenet"))
            {
                if (source.IsChecked.Value)
                {
                    if (Specials == Stroke.Specials.None)
                        Specials = Stroke.Specials.EdgeNet;
                    else if (Specials == Stroke.Specials.EdgeTable)
                        Specials = Stroke.Specials.Both;
                }
                else
                {
                    if (Specials == Stroke.Specials.EdgeNet)
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

            if(Manager.Match != null)
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
        public void Handle(BasicFilterSelectionChangedEvent message)
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
                SelectedRallies = BasicFilterView.SelectedRallies
                    .Where(r =>
                    r.Schläge[0].HasHand(this.Hand) &&
                    r.Schläge[0].HasQuality(this.Quality) &&
                    r.Schläge[0].HasSpins(this.SelectedSpins) &&
                    r.Schläge[0].HasServices(this.SelectedServices) &&
                    r.Schläge[0].HasSpecials(this.Specials) &&
                    r.Schläge[0].HasServerPosition(this.SelectedServerPositions) &&
                    r.Schläge[0].HasTablePosition(this.SelectedTablePositions)
                    )
                    .ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(SelectedRallies));
            }
        }

        #endregion
        }
}
