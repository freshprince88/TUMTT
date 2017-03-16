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
    public class BasicFilterViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<PlaylistSelectionChangedEvent>
    {
        #region Properties

        public string Player1 { get; set; }
        public string Player2 { get; set; }

        public SpinControlViewModel SpinControl { get; private set; }
        public TableServiceViewModel TableView { get; private set; }
        public List<Rally> SelectedRallies { get; private set; }


        public Models.Util.Enums.Stroke.Point Point { get
            {
                return Filter.Point;
            }
            private set {
                Filter.Point = value;
            }
        }
        public Models.Util.Enums.Stroke.Crunch Crunch {
            get {
                return Filter.Crunch;
            }
            private set {
                Filter.Crunch = value;
            }
        }

        public HashSet<int> SelectedSets
        {
            get
            {
                return Filter.Sets;
            }
            private set
            {
                Filter.Sets = value;
            }
        }
        public HashSet<int> SelectedRallyLengths
        {
            get
            {
                return Filter.RallyLengths;
            }
            private set
            {
                Filter.RallyLengths = value;
            }
        }
        
        public int MinRallyLength {
            get
            {
                return Filter.MinRallyLength;
            }
        }

        private BasicFilter Filter { get; set; }

        #endregion

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IViewManager Manager;

        public BasicFilterViewModel(IEventAggregator eventAggregator, IViewManager man, BasicFilter filter)
        {
            this.events = eventAggregator;
            Manager = man;
            this.Filter = filter;
            SelectedRallies = new List<Rally>();
            Player1 = "Spieler 1";
            Player2 = "Spieler 2";
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
                }
            }
            UpdateSelection(Manager.ActivePlaylist);
        }

        public void P1P2Point(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("player1"))
            {
                if (source.IsChecked.Value)
                {
                    if (Point == Models.Util.Enums.Stroke.Point.None)
                        Point = Models.Util.Enums.Stroke.Point.Player1;
                    else if (Point == Models.Util.Enums.Stroke.Point.Player2)
                        Point = Models.Util.Enums.Stroke.Point.Both;
                }
                else
                {
                    if (Point == Models.Util.Enums.Stroke.Point.Player1)
                        Point = Models.Util.Enums.Stroke.Point.None;
                    else if (Point == Models.Util.Enums.Stroke.Point.Both)
                        Point = Models.Util.Enums.Stroke.Point.Player2;
                }
            }
            else if (source.Name.ToLower().Contains("player2"))
            {
                if (source.IsChecked.Value)
                {
                    if (Point == Models.Util.Enums.Stroke.Point.None)
                        Point = Models.Util.Enums.Stroke.Point.Player2;
                    else if (Point == Models.Util.Enums.Stroke.Point.Player1)
                        Point = Models.Util.Enums.Stroke.Point.Both;
                }
                else
                {
                    if (Point == Models.Util.Enums.Stroke.Point.Player2)
                        Point = Models.Util.Enums.Stroke.Point.None;
                    else if (Point == Models.Util.Enums.Stroke.Point.Both)
                        Point = Models.Util.Enums.Stroke.Point.Player1;
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

            Player1 = Manager.MatchManager.Match.FirstPlayer.Name.Split(' ')[0];
            Player2 = Manager.MatchManager.Match.SecondPlayer.Name.Split(' ')[0];
            if (Manager.MatchManager.Match != null)
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
                SelectedRallies = Filter.filter(list.Rallies).ToList();
                events.PublishOnUIThread(new BasicFilterSelectionChangedEvent(SelectedRallies));
            }
        }

        #endregion
    }
}
