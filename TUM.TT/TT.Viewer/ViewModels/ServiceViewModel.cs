using Caliburn.Micro;
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
        public List<Models.Util.Enums.Stroke.Spin> SelectedSpins { get; private set; }
        public Models.Util.Enums.Stroke.Hand Hand { get; private set; }       
        public Models.Util.Enums.Stroke.Quality Quality { get; private set; }
        public Models.Util.Enums.Stroke.Specials Specials { get; private set; }

        public HashSet<Models.Util.Enums.Stroke.Services> SelectedServices { get; private set; }
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
                PlayerLabel = "Service:",
                LastStroke = false,
                StrokeNumber = 0

            };
            TableView = new TableServiceViewModel(events);

            SelectedSpins = new List<Models.Util.Enums.Stroke.Spin>();
            Hand = Models.Util.Enums.Stroke.Hand.None;
            Quality = Models.Util.Enums.Stroke.Quality.None;
            Specials = Models.Util.Enums.Stroke.Specials.None;
            SelectedServices = new HashSet<Models.Util.Enums.Stroke.Services>();
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
                    SelectedServices.Add(Models.Util.Enums.Stroke.Services.Pendulum);
                }
                else
                {
                    SelectedServices.Remove(Models.Util.Enums.Stroke.Services.Pendulum);
                }
            }
            else if (source.Name.ToLower().Contains("reverse"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedServices.Add(Models.Util.Enums.Stroke.Services.Reverse);
                }
                else
                {
                    SelectedServices.Remove(Models.Util.Enums.Stroke.Services.Reverse);
                }
            }
            else if (source.Name.ToLower().Contains("tomahawk"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedServices.Add(Models.Util.Enums.Stroke.Services.Tomahawk);
                }
                else
                {
                    SelectedServices.Remove(Models.Util.Enums.Stroke.Services.Tomahawk);
                }
            }
            else if (source.Name.ToLower().Contains("special"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedServices.Add(Models.Util.Enums.Stroke.Services.Special);
                }
                else
                {
                    SelectedServices.Remove(Models.Util.Enums.Stroke.Services.Special);
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
       
        public void ForeBackHand(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("forehand"))
            {
                if (source.IsChecked.Value)
                {
                    if (Hand == Models.Util.Enums.Stroke.Hand.None)
                        Hand = Models.Util.Enums.Stroke.Hand.Fore;
                    else if (Hand == Models.Util.Enums.Stroke.Hand.Back)
                        Hand = Models.Util.Enums.Stroke.Hand.Both;
                }
                else
                {
                    if (Hand == Models.Util.Enums.Stroke.Hand.Fore)
                        Hand = Models.Util.Enums.Stroke.Hand.None;
                    else if (Hand == Models.Util.Enums.Stroke.Hand.Both)
                        Hand = Models.Util.Enums.Stroke.Hand.Back;
                }
            }
            else if (source.Name.ToLower().Contains("backhand"))
            {
                if (source.IsChecked.Value)
                {
                    if (Hand == Models.Util.Enums.Stroke.Hand.None)
                        Hand = Models.Util.Enums.Stroke.Hand.Back;
                    else if (Hand == Models.Util.Enums.Stroke.Hand.Fore)
                        Hand = Models.Util.Enums.Stroke.Hand.Both;
                }
                else
                {
                    if (Hand == Models.Util.Enums.Stroke.Hand.Back)
                        Hand = Models.Util.Enums.Stroke.Hand.None;
                    else if (Hand == Models.Util.Enums.Stroke.Hand.Both)
                        Hand = Models.Util.Enums.Stroke.Hand.Fore;
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
                    if (Quality == Models.Util.Enums.Stroke.Quality.None)
                        Quality = Models.Util.Enums.Stroke.Quality.Good;
                    else if (Quality == Models.Util.Enums.Stroke.Quality.Bad)
                        Quality = Models.Util.Enums.Stroke.Quality.Both;
                }
                else
                {
                    if (Quality == Models.Util.Enums.Stroke.Quality.Good)
                        Quality = Models.Util.Enums.Stroke.Quality.None;
                    else if (Quality == Models.Util.Enums.Stroke.Quality.Both)
                        Quality = Models.Util.Enums.Stroke.Quality.Bad;
                }
            }
            else if (source.Name.ToLower().Contains("badq"))
            {
                if (source.IsChecked.Value)
                {
                    if (Quality == Models.Util.Enums.Stroke.Quality.None)
                        Quality = Models.Util.Enums.Stroke.Quality.Bad;
                    else if (Quality == Models.Util.Enums.Stroke.Quality.Good)
                        Quality = Models.Util.Enums.Stroke.Quality.Both;
                }
                else
                {
                    if (Quality == Models.Util.Enums.Stroke.Quality.Bad)
                        Quality = Models.Util.Enums.Stroke.Quality.None;
                    else if (Quality == Models.Util.Enums.Stroke.Quality.Both)
                        Quality = Models.Util.Enums.Stroke.Quality.Good;
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
                    if (Specials == Models.Util.Enums.Stroke.Specials.None)
                        Specials = Models.Util.Enums.Stroke.Specials.EdgeTable;
                    else if (Specials == Models.Util.Enums.Stroke.Specials.EdgeNet)
                        Specials = Models.Util.Enums.Stroke.Specials.Both;
                }
                else
                {
                    if (Specials == Models.Util.Enums.Stroke.Specials.EdgeTable)
                        Specials = Models.Util.Enums.Stroke.Specials.None;
                    else if (Specials == Models.Util.Enums.Stroke.Specials.Both)
                        Specials = Models.Util.Enums.Stroke.Specials.EdgeNet;
                }
            }
            else if (source.Name.ToLower().Contains("edgenet"))
            {
                if (source.IsChecked.Value)
                {
                    if (Specials == Models.Util.Enums.Stroke.Specials.None)
                        Specials = Models.Util.Enums.Stroke.Specials.EdgeNet;
                    else if (Specials == Models.Util.Enums.Stroke.Specials.EdgeTable)
                        Specials = Models.Util.Enums.Stroke.Specials.Both;
                }
                else
                {
                    if (Specials == Models.Util.Enums.Stroke.Specials.EdgeNet)
                        Specials = Models.Util.Enums.Stroke.Specials.None;
                    else if (Specials == Models.Util.Enums.Stroke.Specials.Both)
                        Specials = Models.Util.Enums.Stroke.Specials.EdgeTable;
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

            //if(Manager.Match != null)
            //UpdateSelection(Manager.ActivePlaylist);
        }

        protected override void OnDeactivate(bool close)
        {
            this.DeactivateItem(SpinControl, close);
            this.DeactivateItem(TableView, close);
            this.DeactivateItem(BasicFilterView, close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            UpdateSelection(Manager.ActivePlaylist);
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
                var results = BasicFilterView.SelectedRallies
                    .Where(r =>
                    r.Strokes[0].HasHand(this.Hand) &&
                    r.Strokes[0].HasQuality(this.Quality) &&
                    r.Strokes[0].HasSpins(this.SelectedSpins) &&
                    r.Strokes[0].HasServices(this.SelectedServices) &&
                    r.Strokes[0].HasSpecials(this.Specials) &&
                    r.Strokes[0].HasServerPosition(this.SelectedServerPositions) &&
                    r.Strokes[0].HasTablePosition(this.SelectedTablePositions)
                    )
                    .ToList();
                Manager.SelectedRallies = results;
            }
        }

        #endregion
        }
}
