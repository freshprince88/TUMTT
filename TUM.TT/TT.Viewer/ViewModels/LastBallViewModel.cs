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
    public class LastBallViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<TableStdViewSelectionChangedEvent>,
        IHandle<BasicFilterSelectionChangedEvent>
    {
        public BasicFilterViewModel BasicFilterView { get; set; }
        public TableStandardViewModel TableView { get; set; }
        public List<Rally> SelectedRallies { get; private set; }
        public Playlist ActivePlaylist { get; private set; }
        public Stroke.Hand Hand { get; private set; }
        public HashSet<Positions.Length> SelectedStrokeLengths { get; set; }
        public HashSet<Positions.Table> SelectedTablePositions { get; set; }
        public Stroke.Quality Quality { get; private set; }
        public Stroke.WinnerOrNetOut Winner { get; private set; }
        public Stroke.StepAround StepAround { get; private set; }
     
        private HashSet<Stroke.Technique> _strokeTec;
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

        public LastBallViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            SelectedRallies = new List<Rally>();
            ActivePlaylist = new Playlist();
            Hand = Stroke.Hand.None;
            SelectedStrokeLengths = new HashSet<Positions.Length>();
            SelectedTablePositions = new HashSet<Positions.Table>();
            Quality = Stroke.Quality.None;
            SelectedStrokeTec = new HashSet<Stroke.Technique>();
            StepAround = Stroke.StepAround.Not;
            Winner = Stroke.WinnerOrNetOut.None;
            BasicFilterView = new BasicFilterViewModel(this.events, Manager)
            {
                MinRallyLength = 1,
                PlayerLabel = "Aufschlag:",
                LastStroke = true,
                StrokeNumber=0
            };

            TableView = new TableStandardViewModel(this.events);
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

        public void WinnerOrNetOut(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("winner"))
            {
                if (source.IsChecked.Value)
                {
                    if (Winner == Stroke.WinnerOrNetOut.None)
                        Winner = Stroke.WinnerOrNetOut.Winner;
                    else if (Winner == Stroke.WinnerOrNetOut.NetOut)
                        Winner = Stroke.WinnerOrNetOut.Both;
                }
                else
                {
                    if (Winner == Stroke.WinnerOrNetOut.Winner)
                        Winner = Stroke.WinnerOrNetOut.None;
                    else if (Winner == Stroke.WinnerOrNetOut.Both)
                        Winner = Stroke.WinnerOrNetOut.NetOut;
                }
            }
            else if (source.Name.ToLower().Contains("netout"))
            {
                if (source.IsChecked.Value)
                {
                    if (Winner == Stroke.WinnerOrNetOut.None)
                        Winner = Stroke.WinnerOrNetOut.NetOut;
                    else if (Winner == Stroke.WinnerOrNetOut.Winner)
                        Winner = Stroke.WinnerOrNetOut.Both;
                }
                else
                {
                    if (Winner == Stroke.WinnerOrNetOut.NetOut)
                        Winner = Stroke.WinnerOrNetOut.None;
                    else if (Winner == Stroke.WinnerOrNetOut.Both)
                        Winner = Stroke.WinnerOrNetOut.Winner;
                }
            }
            UpdateSelection(Manager.ActivePlaylist);
        }

        public void ForBackHand(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("Forehand"))
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
            if (source.Name.ToLower().Contains("tecpushbutton"))
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
            else if (source.Name.ToLower().Contains("tecpushaggressivebutton"))
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
            else if (source.Name.ToLower().Contains("tecflipbutton"))
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
            else if (source.Name.ToLower().Contains("tecflipbananabutton"))
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
            else if (source.Name.ToLower().Contains("tectopspinbutton"))
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
            else if (source.Name.ToLower().Contains("tectopspinspinbutton"))
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
            else if (source.Name.ToLower().Contains("tectopspintempobutton"))
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
            else if (source.Name.ToLower().Contains("tecblockbutton"))
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
            else if (source.Name.ToLower().Contains("tecblocktempobutton"))
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
            else if (source.Name.ToLower().Contains("tecblockchopbutton"))
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
            else if (source.Name.ToLower().Contains("teccounterbutton"))
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
            else if (source.Name.ToLower().Contains("tecsmashbutton"))
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
            else if (source.Name.ToLower().Contains("teclobbutton"))
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
            else if (source.Name.ToLower().Contains("tecchopbutton"))
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
            else if (source.Name.ToLower().Contains("tecspecialbutton"))
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

            UpdateSelection(Manager.ActivePlaylist);
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
                SelectedRallies = BasicFilterView.SelectedRallies.Where(r => HasWinner(r) && HasHand(r) && HasStepAround(r) && HasStrokeTec(r) && HasQuality(r) && HasTablePosition(r) && HasStrokeLength(r)).ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(SelectedRallies));
            }
        }

        private bool HasWinner(Rally r)
        {
            int StrokeNumber = Convert.ToInt32(r.Length) - 1;
            
           
            switch (this.Winner)
            {
                case Stroke.WinnerOrNetOut.Winner:
                    return r.Schlag[StrokeNumber].Verlauf == "Winner";
                case Stroke.WinnerOrNetOut.NetOut:
                    return r.Schlag[StrokeNumber].Verlauf == "Netz" || r.Schlag[StrokeNumber].Verlauf == "Aus";
                case Stroke.WinnerOrNetOut.None:
                    return true;
                case Stroke.WinnerOrNetOut.Both:
                    return true;
                default:
                    return false;
            }
        }

        private bool HasHand(Rally r)
        {
            int StrokeNumber;
            if (r.Schlag[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner)
            {
                StrokeNumber = Convert.ToInt32(r.Length) - 1;
            }
            else
            {
                StrokeNumber = Convert.ToInt32(r.Length) - 2;
            }
            switch (this.Hand)
            {
                case Stroke.Hand.Fore:
                    return r.Schlag[StrokeNumber].Schlägerseite == "Vorhand";
                case Stroke.Hand.Back:
                    return r.Schlag[StrokeNumber].Schlägerseite == "Rückhand";
                case Stroke.Hand.None:
                    return true;
                case Stroke.Hand.Both:
                    return true;
                default:
                    return false;
            }
        }

        private bool HasStepAround(Rally r)
        {
            int StrokeNumber;
            if (r.Schlag[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner)
            {
                StrokeNumber = Convert.ToInt32(r.Length) - 1;
            }
            else
            {
                StrokeNumber = Convert.ToInt32(r.Length) - 2;
            }
            switch (this.StepAround)
            {
                case Stroke.StepAround.StepAround:
                    return r.Schlag[StrokeNumber].Umlaufen == "ja";
                case Stroke.StepAround.Not:
                    return true;
                default:
                    return false;
            }
        }

        private bool HasStrokeTec(Rally r)
        {
            int StrokeNumber;
            if (r.Schlag[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner)
            {
                StrokeNumber = Convert.ToInt32(r.Length) - 1;
            }
            else
            {
                StrokeNumber = Convert.ToInt32(r.Length) - 2;
            }
            List<bool> ORresults = new List<bool>();

            foreach (var stroketec in SelectedStrokeTec)
            {
                switch (stroketec)
                {
                    case Stroke.Technique.Push:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Schupf");
                        break;
                    case Stroke.Technique.PushAggressive:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Schupf" && r.Schlag[StrokeNumber].Schlagtechnik.Option == "aggressiv");
                        break;
                    case Stroke.Technique.Flip:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Flip");
                        break;
                    case Stroke.Technique.Banana:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Flip" && r.Schlag[StrokeNumber].Schlagtechnik.Option == "Banane");
                        break;
                    case Stroke.Technique.Topspin:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Topspin");
                        break;
                    case Stroke.Technique.TopspinSpin:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Topspin" && r.Schlag[StrokeNumber].Schlagtechnik.Option == "Spin");
                        break;
                    case Stroke.Technique.TopspinTempo:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Topspin" && r.Schlag[StrokeNumber].Schlagtechnik.Option == "Tempo");
                        break;
                    case Stroke.Technique.Block:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Block");
                        break;
                    case Stroke.Technique.BlockTempo:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Block" && r.Schlag[StrokeNumber].Schlagtechnik.Option == "Tempo");
                        break;
                    case Stroke.Technique.BlockChop:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Block" && r.Schlag[StrokeNumber].Schlagtechnik.Option == "Chop");
                        break;
                    case Stroke.Technique.Counter:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Konter");
                        break;
                    case Stroke.Technique.Smash:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Schuss");
                        break;
                    case Stroke.Technique.Lob:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Ballonabwehr");
                        break;
                    case Stroke.Technique.Chop:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Schnittabwehr");
                        break;
                    case Stroke.Technique.Special:
                        ORresults.Add(r.Schlag[StrokeNumber].Schlagtechnik.Art == "Sonstige");
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasQuality(Rally r)
        {
            int StrokeNumber;
            if (r.Schlag[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner)
            {
                StrokeNumber = Convert.ToInt32(r.Length) -1;
            }
            else
            {
                StrokeNumber = Convert.ToInt32(r.Length) - 2;
            }
            switch (this.Quality)
            {
                case Stroke.Quality.Good:
                    return r.Schlag[StrokeNumber].Qualität == "gut";
                case Stroke.Quality.Bad:
                    return r.Schlag[StrokeNumber].Qualität == "schlecht";
                case Stroke.Quality.None:
                    return true;
                case Stroke.Quality.Both:
                    return r.Schlag[StrokeNumber].Qualität == "gut" || r.Schlag[StrokeNumber].Qualität == "schlecht";
                default:
                    return false;
            }
        }

        private bool HasTablePosition(Rally r)
        {
            int StrokeNumber;
            if (r.Schlag[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner)
            {
                StrokeNumber = Convert.ToInt32(r.Length);
            }
            else
            {
                StrokeNumber = Convert.ToInt32(r.Length) - 1;
            }
            List<bool> ORresults = new List<bool>();
            Schlag stroke = r.Schlag.Where(s => Convert.ToInt32(s.Nummer) == StrokeNumber).FirstOrDefault();
            foreach (var sel in SelectedTablePositions)
            {
                switch (sel)
                {
                    case Positions.Table.TopLeft:
                        ORresults.Add(stroke.IsTopLeft());
                        break;
                    case Positions.Table.TopMid:
                        ORresults.Add(stroke.IsTopMid());
                        break;
                    case Positions.Table.TopRight:
                        ORresults.Add(stroke.IsTopRight());
                        break;
                    case Positions.Table.MidLeft:
                        ORresults.Add(stroke.IsMidLeft());
                        break;
                    case Positions.Table.MidMid:
                        ORresults.Add(stroke.IsMidMid());
                        break;
                    case Positions.Table.MidRight:
                        ORresults.Add(stroke.IsMidRight());
                        break;
                    case Positions.Table.BotLeft:
                        ORresults.Add(stroke.IsBotLeft());
                        break;
                    case Positions.Table.BotMid:
                        ORresults.Add(stroke.IsBotMid());
                        break;
                    case Positions.Table.BotRight:
                        ORresults.Add(stroke.IsBotRight());
                        break;
                    default:
                        break;
                }
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

        private bool HasStrokeLength(Rally r)
        {
            int StrokeNumber;
            if (r.Schlag[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner)
            {
                StrokeNumber = Convert.ToInt32(r.Length);
            }
            else
            {
                StrokeNumber = Convert.ToInt32(r.Length) - 1;
            }
            List<bool> ORresults = new List<bool>();
            
            Schlag stroke = r.Schlag.Where(s => Convert.ToInt32(s.Nummer) == StrokeNumber).FirstOrDefault();

            foreach (var sel in SelectedStrokeLengths)
            {
                switch (sel)
                {
                    case Positions.Length.OverTheTable:
                        ORresults.Add(stroke.IsOverTheTable());
                        break;
                    case Positions.Length.AtTheTable:
                        ORresults.Add(stroke.IsAtTheTable());
                        break;
                    case Positions.Length.HalfDistance:
                        ORresults.Add(stroke.IsHalfDistance());
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
