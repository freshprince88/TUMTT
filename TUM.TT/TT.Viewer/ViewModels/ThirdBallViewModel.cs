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
    public class ThirdBallViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<TableViewSelectionChangedEvent>,
        IHandle<FilterSwitchedEvent>
    {
        public BasicFilterViewModel BasicFilterView { get; set; }
        public TableStandardViewModel TableView { get; set; }
        public List<MatchRally> SelectedRallies { get; private set; }
        public Match Match { get; private set; }
        public EHand Hand { get; private set; }
        public EQuality Quality { get; private set; }
        public EStepAround StepAround { get; private set; }


        public enum EHand
        {
            Fore,
            Back,
            None,
            Both
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
            Counter,
            Smash,
            Lob,
            Chop,
            Special
        }

        private HashSet<StrokeTec> _strokeTec;

        public HashSet<StrokeTec> SelectedStrokeTec
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

        public enum EQuality
        {
            Bad,
            Good,
            None,
            Both
        }
        public enum EStepAround
        {
            StepAround,
            Not
        }


        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public ThirdBallViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedRallies = new List<MatchRally>();
            Match = new Match();           
            Hand = EHand.None;
            Quality = EQuality.None;
            SelectedStrokeTec = new HashSet<StrokeTec>();
            StepAround = EStepAround.Not;
            BasicFilterView = new BasicFilterViewModel(this.events)
            {
                MinRallyLength = 2,
                PlayerLabel="3.Schlag"
            };
            TableView = new TableStandardViewModel(this.events);
        }


        #region View Methods
        public void SwitchTable(bool check)
        {
            if (check)
            {
                TableView.Mode = TableStandardViewModel.ViewMode.Top;
            }
            else
            {
                TableView.Mode = TableStandardViewModel.ViewMode.Bottom;
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
        public void StepAroundOrNot(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("steparoundbutton"))
            {
                if (source.IsChecked.Value)
                {
                    StepAround = EStepAround.StepAround;
                }
                else
                {
                    StepAround = EStepAround.Not;
                }
            }
            UpdateSelection();
        }

        public void SelectStrokeTec(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("tecpushbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.Push);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.Push);
                }
            }
            else if (source.Name.ToLower().Contains("tecpushaggressivebutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.PushAggressive);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.PushAggressive);
                }
            }
            else if (source.Name.ToLower().Contains("tecflipbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.Flip);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.Flip);
                }
            }
            else if (source.Name.ToLower().Contains("tecflipbananabutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.Banana);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.Banana);
                }
            }
            else if (source.Name.ToLower().Contains("tectopspinbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.Topspin);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.Topspin);
                }
            }
            else if (source.Name.ToLower().Contains("tectopspinspinbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.TopspinSpin);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.TopspinSpin);
                }
            }
            else if (source.Name.ToLower().Contains("tectopspintempobutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.TopspinTempo);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.TopspinTempo);
                }
            }
            else if (source.Name.ToLower().Contains("tecblockbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.Block);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.Block);
                }
            }
            else if (source.Name.ToLower().Contains("tecblocktempobutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.BlockTempo);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.BlockTempo);
                }
            }
            else if (source.Name.ToLower().Contains("tecblockchopbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.BlockChop);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.BlockChop);
                }
            }
            else if (source.Name.ToLower().Contains("teccounterbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.Counter);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.Counter);
                }
            }
            else if (source.Name.ToLower().Contains("tecsmashbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.Smash);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.Smash);
                }
            }
            else if (source.Name.ToLower().Contains("teclobbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.Lob);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.Lob);
                }
            }
            else if (source.Name.ToLower().Contains("tecchopbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.Chop);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.Chop);
                }
            }
            else if (source.Name.ToLower().Contains("tecspecialbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedStrokeTec.Add(StrokeTec.Special);
                }
                else
                {
                    SelectedStrokeTec.Remove(StrokeTec.Special);
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
            this.ActivateItem(BasicFilterView);
        }
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            this.DeactivateItem(TableView, close);
            this.DeactivateItem(BasicFilterView, close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }
        #endregion

        #region Event Handlers
        public void Handle(FilterSwitchedEvent message)
        {
            this.Match = message.Match;
            if (this.Match.Rallies != null)
            {
                SelectedRallies = this.Match.Rallies.Where(r => r.Schlag.Length > BasicFilterView.MinRallyLength).ToList();
                this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
            }
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
                SelectedRallies = BasicFilterView.SelectedRallies.Where(r => HasHand(r) && HasStepAround(r) && HasStrokeTec(r) && HasQuality(r)).ToList();
                this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
            }
        }

      

       

       

        

        

        private bool HasHand(MatchRally r)
        {
            switch (this.Hand)
            {
                case EHand.Fore:
                    return r.Schlag[2].Schlägerseite == "Vorhand";
                case EHand.Back:
                    return r.Schlag[2].Schlägerseite == "Rückhand";
                case EHand.None:
                    return true;
                case EHand.Both:
                    return true;
                default:
                    return false;
            }
        }
        private bool HasStepAround(MatchRally r)
        {
            switch (this.StepAround)
            {
                case EStepAround.StepAround:
                    return r.Schlag[2].Umlaufen == "ja";
                case EStepAround.Not:
                    return true;
                default:
                    return false;
            }
        }

        private bool HasStrokeTec(MatchRally r)
        {
            List<bool> ORresults = new List<bool>();

            foreach (var stroketec in SelectedStrokeTec)
            {
                switch (stroketec)
                {
                    case StrokeTec.Push:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Schupf");
                        break;
                    case StrokeTec.PushAggressive:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Schupf" && r.Schlag[2].Schlagtechnik.Option == "aggressiv");
                        break;
                    case StrokeTec.Flip:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Flip");
                        break;
                    case StrokeTec.Banana:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Flip" && r.Schlag[2].Schlagtechnik.Option == "Banane");
                        break;
                    case StrokeTec.Topspin:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Topspin");
                        break;
                    case StrokeTec.TopspinSpin:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Topspin" && r.Schlag[2].Schlagtechnik.Option == "Spin");
                        break;
                    case StrokeTec.TopspinTempo:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Topspin" && r.Schlag[2].Schlagtechnik.Option == "Tempo");
                        break;
                    case StrokeTec.Block:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Block");
                        break;
                    case StrokeTec.BlockTempo:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Block" && r.Schlag[2].Schlagtechnik.Option == "Tempo");
                        break;
                    case StrokeTec.BlockChop:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Block" && r.Schlag[2].Schlagtechnik.Option == "Chop");
                        break;
                    case StrokeTec.Counter:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Konter");
                        break;
                    case StrokeTec.Smash:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Schuss");
                        break;
                    case StrokeTec.Lob:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Ballonabwehr");
                        break;
                    case StrokeTec.Chop:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Schnittabwehr");
                        break;
                    case StrokeTec.Special:
                        ORresults.Add(r.Schlag[2].Schlagtechnik.Art == "Sonstige");
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasQuality(MatchRally r)
        {
            switch (this.Quality)
            {
                case EQuality.Good:
                    return r.Schlag[2].Qualität == "gut";
                case EQuality.Bad:
                    return r.Schlag[2].Qualität == "schlecht";
                case EQuality.None:
                    return true;
                case EQuality.Both:
                    return r.Schlag[2].Qualität == "gut" || r.Schlag[2].Qualität == "schlecht";
                default:
                    return false;
            }
        }

        #endregion
    }
}
