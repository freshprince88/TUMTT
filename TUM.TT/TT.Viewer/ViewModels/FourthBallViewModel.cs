using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.Primitives;
using TT.Models;
using TT.Lib.Events;
using TT.Models.Util.Enums;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class FourthBallViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<TableStdViewSelectionChangedEvent>,
        IHandle<BasicFilterSelectionChangedEvent>
    {
        public BasicFilterViewModel BasicFilterView { get; set; }
        public TableStandardViewModel TableView { get; set; }
        public List<Rally> SelectedRallies { get; private set; }
        public HashSet<Positions.Length> SelectedStrokeLengths { get; set; }
        public HashSet<Positions.Table> SelectedTablePositions { get; set; }
        public Stroke.Quality Quality { get; private set; }
        private HashSet<Stroke.Aggression> _aggression;
        public HashSet<Stroke.Aggression> SelectedAggression
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
        public Stroke.Specials Specials { get; private set; }
        public Stroke.StepAround StepAround { get; private set; }
        private HashSet<Stroke.Technique> _strokeTec;
        public Stroke.Hand Hand { get; private set; }
        public HashSet<Stroke.Technique> SelectedStrokeTec
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

        public FourthBallViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            SelectedRallies = new List<Rally>();
            Hand = Stroke.Hand.None;
            SelectedStrokeLengths = new HashSet<Positions.Length>();
            SelectedTablePositions = new HashSet<Positions.Table>();
            Quality = Stroke.Quality.None;
            SelectedAggression = new HashSet<Stroke.Aggression>();
            Specials = Stroke.Specials.None;
            SelectedStrokeTec = new HashSet<Stroke.Technique>();
            StepAround = Stroke.StepAround.Not;
            BasicFilterView = new BasicFilterViewModel(this.events, Manager)
            {
                MinRallyLength = 3,
                PlayerLabel = "4.Schlag:",
                StrokeNumber = 3
            };
            TableView = new TableStandardViewModel(this.events,"Fourth");
            TableView.StrokeNumber = 3;
            TableView.lastStroke=false;

                
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
        public void StepAroundOrNot(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("steparoundbutton"))
            {
                if (source.IsChecked.Value)
                {
                    StepAround = Stroke.StepAround.StepAround;
                }
                else
                {
                    StepAround = Stroke.StepAround.Not;
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
                    SelectedStrokeTec.Add(Stroke.Technique.Push);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.Push);
                }
            }
            else if (source.Name.ToLower().Contains("pushaggressive"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.PushAggressive);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.PushAggressive);
                }
            }
            else if (source.Name.ToLower().Equals("flip"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.Flip);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.Flip);
                }
            }
            else if (source.Name.ToLower().Equals("banana"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.Banana);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.Banana);
                }
            }
            else if (source.Name.ToLower().Equals("topspin"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.Topspin);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.Topspin);
                }
            }
            else if (source.Name.ToLower().Equals("topspinspin"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.TopspinSpin);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.TopspinSpin);
                }
            }
            else if (source.Name.ToLower().Equals("topspintempo"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.TopspinTempo);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.TopspinTempo);
                }
            }
            else if (source.Name.ToLower().Equals("block"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.Block);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.Block);
                }
            }
            else if (source.Name.ToLower().Equals("blocktempo"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.BlockTempo);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.BlockTempo);
                }
            }
            else if (source.Name.ToLower().Equals("blockchop"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.BlockChop);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.BlockChop);
                }
            }
            else if (source.Name.ToLower().Equals("counter"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.Counter);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.Counter);
                }
            }
            else if (source.Name.ToLower().Equals("smash"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.Smash);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.Smash);
                }
            }
            else if (source.Name.ToLower().Equals("lob"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.Lob);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.Lob);
                }
            }
            else if (source.Name.ToLower().Equals("chop"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.Chop);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.Chop);
                }
            }
            else if (source.Name.ToLower().Equals("special"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(Stroke.Technique.Special);
                }
                else
                {
                    SelectedStrokeTec.Remove(Stroke.Technique.Special);
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
        public void AggressivePassiveControl(ToggleButton source)
        {
            if (source.Name.ToLower().Equals("aggressive"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedAggression.Add(Stroke.Aggression.Aggressive);
                }
                else
                {
                    SelectedAggression.Remove(Stroke.Aggression.Aggressive);
                }
            }
            else if (source.Name.ToLower().Equals("passive"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedAggression.Add(Stroke.Aggression.Passive);
                }
                else
                {
                    SelectedAggression.Remove(Stroke.Aggression.Passive);
                }
            }
            else if (source.Name.ToLower().Equals("control"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedAggression.Add(Stroke.Aggression.Control);
                }
                else
                {
                    SelectedAggression.Remove(Stroke.Aggression.Control);
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
                SelectedRallies = BasicFilterView.SelectedRallies.Where(r =>
                    r.Schläge[3].HasHand(this.Hand) &&
                    r.Schläge[3].HasStepAround(this.StepAround) &&
                    r.Schläge[3].HasStrokeTec(this.SelectedStrokeTec) &&
                    r.Schläge[3].HasQuality(this.Quality) &&
                    r.Schläge[3].HasTablePosition(this.SelectedTablePositions) &&
                    r.Schläge[3].HasStrokeLength(this.SelectedStrokeLengths) &&
                    r.Schläge[3].HasAggression(this.SelectedAggression) &&
                    r.Schläge[3].HasSpecials(this.Specials)).
                    ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(SelectedRallies));
            }
        }
        #endregion
    }
}
