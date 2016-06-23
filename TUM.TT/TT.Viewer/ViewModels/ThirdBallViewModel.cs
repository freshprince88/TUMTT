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
    public class ThirdBallViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<TableStdViewSelectionChangedEvent>,
        IHandle<BasicFilterSelectionChangedEvent>
    {
        public BasicFilterViewModel BasicFilterView { get; set; }
        public TableStandardViewModel TableView { get; set; }
        public Models.Util.Enums.Stroke.Hand Hand { get; private set; }
        public HashSet<Positions.Length> SelectedStrokeLengths { get; set; }
        public HashSet<Positions.Table> SelectedTablePositions { get; set; }
        public Models.Util.Enums.Stroke.Quality Quality { get; private set; }
        private HashSet<Models.Util.Enums.Stroke.Aggression> _aggression;
        public HashSet<Models.Util.Enums.Stroke.Aggression> SelectedAggression
        {
            get
            {
                return _aggression;
            }
            private set
            {
                _aggression = value;
            }
        }
        public Models.Util.Enums.Stroke.Specials Specials { get; private set; }
        public Models.Util.Enums.Stroke.StepAround StepAround { get; private set; }

        private HashSet<Models.Util.Enums.Stroke.Technique> _strokeTec;

        public HashSet<Models.Util.Enums.Stroke.Technique> SelectedStrokeTec
        {
            get
            {
                return _strokeTec;
            }
            private set
            {
                _strokeTec = value;
            }
        }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public ThirdBallViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            Hand = Models.Util.Enums.Stroke.Hand.None;
            SelectedStrokeLengths = new HashSet<Positions.Length>();
            SelectedTablePositions = new HashSet<Positions.Table>();
            Quality = Models.Util.Enums.Stroke.Quality.None;
            SelectedAggression = new HashSet<Models.Util.Enums.Stroke.Aggression>();
            Specials = Models.Util.Enums.Stroke.Specials.None;
            SelectedStrokeTec = new HashSet<Models.Util.Enums.Stroke.Technique>();
            StepAround = Models.Util.Enums.Stroke.StepAround.Not;
            BasicFilterView = new BasicFilterViewModel(this.events, Manager)
            {
                MinRallyLength = 2,
                PlayerLabel="3rd Stroke:",
                StrokeNumber = 2
            };
            TableView = new TableStandardViewModel(this.events,"Third");
            TableView.StrokeNumber = 2;
            TableView.lastStroke = false;
        }


        #region View Methods
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

        public void StepAroundOrNot(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("steparoundbutton"))
            {
                if (source.IsChecked.Value)
                {
                    StepAround = Models.Util.Enums.Stroke.StepAround.StepAround;
                }
                else
                {
                    StepAround = Models.Util.Enums.Stroke.StepAround.Not;
                }
            }
            UpdateSelection(Manager.ActivePlaylist);
        }

        public void SelectStrokeTec(ToggleButton source)
        {
            if (source.Name.ToLower().Equals("push"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.Push);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.Push);
                }
            }
            else if (source.Name.ToLower().Contains("pushaggressive"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.PushAggressive);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.PushAggressive);
                }
            }
            else if (source.Name.ToLower().Equals("flip"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.Flip);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.Flip);
                }
            }
            else if (source.Name.ToLower().Equals("banana"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.Banana);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.Banana);
                }
            }
            else if (source.Name.ToLower().Equals("topspin"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.Topspin);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.Topspin);
                }
            }
            else if (source.Name.ToLower().Equals("topspinspin"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.TopspinSpin);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.TopspinSpin);
                }
            }
            else if (source.Name.ToLower().Equals("topspintempo"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.TopspinTempo);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.TopspinTempo);
                }
            }
            else if (source.Name.ToLower().Equals("block"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.Block);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.Block);
                }
            }
            else if (source.Name.ToLower().Equals("blocktempo"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.BlockTempo);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.BlockTempo);
                }
            }
            else if (source.Name.ToLower().Equals("blockchop"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.BlockChop);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.BlockChop);
                }
            }
            else if (source.Name.ToLower().Equals("counter"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.Counter);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.Counter);
                }
            }
            else if (source.Name.ToLower().Equals("smash"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.Smash);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.Smash);
                }
            }
            else if (source.Name.ToLower().Equals("lob"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.Lob);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.Lob);
                }
            }
            else if (source.Name.ToLower().Equals("chop"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.Chop);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.Chop);
                }
            }
            else if (source.Name.ToLower().Equals("special"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Models.Util.Enums.Stroke.Technique.Special);
                }
                else
                {
                    SelectedStrokeTec.Remove(Models.Util.Enums.Stroke.Technique.Special);
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
        public void AggressivePassiveControl(ToggleButton source)
        {
            if (source.Name.ToLower().Equals("aggressive"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedAggression.Add(Models.Util.Enums.Stroke.Aggression.Aggressive);
                }
                else
                {
                    SelectedAggression.Remove(Models.Util.Enums.Stroke.Aggression.Aggressive);
                }
            }
            else if (source.Name.ToLower().Equals("passive"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedAggression.Add(Models.Util.Enums.Stroke.Aggression.Passive);
                }
                else
                {
                    SelectedAggression.Remove(Models.Util.Enums.Stroke.Aggression.Passive);
                }
            }
            else if (source.Name.ToLower().Equals("control"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedAggression.Add(Models.Util.Enums.Stroke.Aggression.Control);
                }
                else
                {
                    SelectedAggression.Remove(Models.Util.Enums.Stroke.Aggression.Control);
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
            this.ActivateItem(TableView);
            this.ActivateItem(BasicFilterView);

            //UpdateSelection(Manager.ActivePlaylist);
        }
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            this.DeactivateItem(TableView, close);
            this.DeactivateItem(BasicFilterView, close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
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

        public void Handle(TableStdViewSelectionChangedEvent message)
        {
            SelectedStrokeLengths = message.StrokeLengths;
            SelectedTablePositions = message.Positions;
            UpdateSelection(Manager.ActivePlaylist);
        }

        #endregion

        #region Helper Methods

        private void UpdateSelection(Playlist list)
        {
            if (list.Rallies != null)
            {
                var results = BasicFilterView.SelectedRallies.Where(r =>
                    r.Strokes[2].HasHand(this.Hand) &&
                    r.Strokes[2].HasStepAround(this.StepAround) &&
                    r.Strokes[2].HasStrokeTec(this.SelectedStrokeTec) &&
                    r.Strokes[2].HasQuality(this.Quality) &&
                    r.Strokes[2].HasTablePosition(this.SelectedTablePositions) &&
                    r.Strokes[2].HasStrokeLength(this.SelectedStrokeLengths) &&
                    r.Strokes[2].HasAggression(this.SelectedAggression) &&
                    r.Strokes[2].HasSpecials(this.Specials)).
                    ToList();
                Manager.SelectedRallies = results;
            }
        }
        #endregion
    }
}
