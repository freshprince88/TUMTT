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

        private Filter ServiceFilter;

        public BasicFilterViewModel BasicFilterView { get; set; }
        public SpinControlViewModel SpinControl { get; private set; }
        public TableServiceViewModel TableView { get; private set; }

        private List<Rally> selectedRalliesOnCreation;

        public Models.Util.Enums.Stroke.Hand Hand
        {
            get
            {
                return ServiceFilter.Hand;
            }
            private set
            {
                ServiceFilter.Hand = value;
            }
        }
        public Models.Util.Enums.Stroke.Quality Quality { get
            {
                return ServiceFilter.Quality;
            }
            private set
            {
                ServiceFilter.Quality = value;
            }
        }
        public HashSet<Models.Util.Enums.Stroke.Specials> SelectedSpecials
        {
            get
            {
                return ServiceFilter.Specials;
            }
            private set
            {
                ServiceFilter.Specials = value;
            }
        }
        public HashSet<Models.Util.Enums.Stroke.Services> SelectedServices
        {
            get
            {
                return ServiceFilter.Services;
            }
            private set
            {
                ServiceFilter.Services = value;
            }
        }
        public HashSet<Positions.Table> SelectedTablePositions
        {
            get
            {
                return ServiceFilter.TablePositions;
            }
            set
            {
                ServiceFilter.TablePositions = value;
            }
        }
        public HashSet<Positions.Server> SelectedServerPositions
        {
            get
            {
                return ServiceFilter.ServerPositions;
            }
            set
            {
                ServiceFilter.ServerPositions = value;
            }
        }

        public IEnumerable<Models.Util.Enums.Stroke.Spin> SelectedSpins {
            get
            {
                return ServiceFilter.Spins;
            }
        }

        public string Name
        {
            get
            {
                return ServiceFilter.Name;
            }
            set
            {
                ServiceFilter.Name = value;
            }
        }
        #endregion


        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public ServiceViewModel(IEventAggregator eventAggregator, IMatchManager man, Filter f, bool showBasicFilter = true)
        {
            this.events = eventAggregator;
            Manager = man;

            if (showBasicFilter)
            {
                BasicFilterView = new BasicFilterViewModel(this.events, Manager)
                {
                    MinRallyLength = 0,
                    PlayerLabel = f.Name,
                    LastStroke = false,
                    StrokeNumber = 0

                };
            }
            else
            {
                selectedRalliesOnCreation = man.SelectedRallies.ToList();
            }
            TableView = new TableServiceViewModel(events, f);
            SpinControl = new SpinControlViewModel(events, f);
            this.ServiceFilter = f;
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
                        Hand = Models.Util.Enums.Stroke.Hand.Forehand;
                    else if (Hand == Models.Util.Enums.Stroke.Hand.Backhand)
                        Hand = Models.Util.Enums.Stroke.Hand.Both;
                }
                else
                {
                    if (Hand == Models.Util.Enums.Stroke.Hand.Forehand)
                        Hand = Models.Util.Enums.Stroke.Hand.None;
                    else if (Hand == Models.Util.Enums.Stroke.Hand.Both)
                        Hand = Models.Util.Enums.Stroke.Hand.Backhand;
                }
            }
            else if (source.Name.ToLower().Contains("backhand"))
            {
                if (source.IsChecked.Value)
                {
                    if (Hand == Models.Util.Enums.Stroke.Hand.None)
                        Hand = Models.Util.Enums.Stroke.Hand.Backhand;
                    else if (Hand == Models.Util.Enums.Stroke.Hand.Forehand)
                        Hand = Models.Util.Enums.Stroke.Hand.Both;
                }
                else
                {
                    if (Hand == Models.Util.Enums.Stroke.Hand.Backhand)
                        Hand = Models.Util.Enums.Stroke.Hand.None;
                    else if (Hand == Models.Util.Enums.Stroke.Hand.Both)
                        Hand = Models.Util.Enums.Stroke.Hand.Forehand;
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
            if (source.Name.ToLower().Equals("edgetable"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSpecials.Add(Models.Util.Enums.Stroke.Specials.EdgeTable);
                }
                else
                {
                    SelectedSpecials.Remove(Models.Util.Enums.Stroke.Specials.EdgeTable);
                }
            }
            else if (source.Name.ToLower().Equals("edgenet"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSpecials.Add(Models.Util.Enums.Stroke.Specials.EdgeNet);
                }
                else
                {
                    SelectedSpecials.Remove(Models.Util.Enums.Stroke.Specials.EdgeNet);
                }
            }
            else if (source.Name.ToLower().Equals("edgenettable"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSpecials.Add(Models.Util.Enums.Stroke.Specials.EdgeNetTable);
                }
                else
                {
                    SelectedSpecials.Remove(Models.Util.Enums.Stroke.Specials.EdgeNetTable);
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
            UpdateSelection(Manager.ActivePlaylist);
        }

        #endregion

        #region Helper Methods

        private void UpdateSelection(Playlist list)
        {
            if (list.Rallies != null)
            {
                List<Rally> ralliesToFilter;
                if (BasicFilterView == null)
                {
                    ralliesToFilter = selectedRalliesOnCreation;
                }
                else
                {
                    ralliesToFilter = BasicFilterView.SelectedRallies;
                }
                var results = ServiceFilter.filter(ralliesToFilter).ToList();

                Manager.SelectedRallies = results;
            }
        }

        #endregion
        }
}
