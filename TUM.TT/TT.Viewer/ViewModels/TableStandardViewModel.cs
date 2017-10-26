using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Models.Util.Enums;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using System.Windows.Controls;

namespace TT.Viewer.ViewModels
{
    public class TableStandardViewModel : Screen, IHandle<ResultsChangedEvent>
    {

        private IEventAggregator events;
        public HashSet<Positions.Table> SelectedPositions { get; set; }
        public HashSet<Positions.Length> SelectedStrokeLength { get; set; }
        public int StrokeNumber { get; set; }  
        public int lastStrokeOrOpeningShot { get; set; }
        public Dictionary<string, int> PositionCounts { get; set; }
        public string name
        {
            get; set;
        }

        private ViewMode.Position _mode;
        public ViewMode.Position Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                if (!_mode.Equals(value))
                    //events.PublishOnUIThread(new TableViewModeChangedEvent(value));

                _mode = value;
                
            }
        }

        public TableStandardViewModel(IEventAggregator eventAggregator, string n)
        {
            name = n;
            StrokeNumber = 0;
            lastStrokeOrOpeningShot = 0;
            this.events = eventAggregator;
            PositionCounts = new Dictionary<string, int>();
            PositionCounts.Add("BotLeft", 0);
            PositionCounts.Add("BotMid", 0);
            PositionCounts.Add("BotRight", 0);
            PositionCounts.Add("MidLeft", 0);
            PositionCounts.Add("MidMid", 0);
            PositionCounts.Add("MidRight", 0);
            PositionCounts.Add("TopLeft", 0);
            PositionCounts.Add("TopMid", 0);
            PositionCounts.Add("TopRight", 0);
            SelectedStrokeLength = new HashSet<Positions.Length>();
            SelectedPositions = new HashSet<Positions.Table>();
        }

        #region View Methods

        public void TablePositionClicked(ToggleButton toggle)
        {
            Positions.Table pos = Positions.GetTablePositionFromName(toggle.Name);
            if (toggle.IsChecked.Value)
            {
                if (pos != Positions.Table.None)
                    SelectedPositions.Add(pos);
            }
            else
            {
                if (pos != Positions.Table.None)
                    SelectedPositions.Remove(pos);
            }
            this.events.PublishOnUIThread(new TableStdViewSelectionChangedEvent(SelectedPositions, SelectedStrokeLength));
        }

        public void StrokeLengthClicked(ToggleButton toggle)
        {
            Positions.Length pos = Positions.GetStrokeLengthFromName(toggle.Name);
            if (toggle.IsChecked.Value)
            {
                if (pos != Positions.Length.None)
                    SelectedStrokeLength.Add(pos);
            }
            else
            {
                if (pos != Positions.Length.None)
                    SelectedStrokeLength.Remove(pos);
            }
            this.events.PublishOnUIThread(new TableStdViewSelectionChangedEvent(SelectedPositions, SelectedStrokeLength));
        }

        #endregion

        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnActivate()
        {
            this.events.Subscribe(this);
            base.OnActivate();
        }

        /// <summary>
        /// Handles deactivation of this view model.
        /// </summary>
        /// <param name="close">Whether the view model is closed</param>
        protected override void OnDeactivate(bool close)
        {
            this.events.Unsubscribe(this);
            base.OnDeactivate(close);
        }



        #endregion

        #region Helper Methods       

        public void Handle(ResultsChangedEvent message)
        {
            var rallies = new LinkedList<Rally>(message.Rallies);

            int topLeft = 0;
            int topMid = 0;
            int topRight = 0;

            int midLeft = 0;
            int midMid = 0;
            int midRight = 0;

            int botLeft = 0;
            int botMid = 0;
            int botRight = 0;

            int t1 = StrokeNumber;
            int t2 = lastStrokeOrOpeningShot;

            if (rallies != null)
            {
                if (lastStrokeOrOpeningShot == 0)
                {

                    topLeft = rallies.Where(r => Convert.ToInt32(r.Length) > StrokeNumber && r.Strokes[StrokeNumber].IsTopLeft()).Count();
                    topMid = rallies.Where(r => Convert.ToInt32(r.Length) > StrokeNumber && r.Strokes[StrokeNumber].IsTopMid()).Count();
                    topRight = rallies.Where(r => Convert.ToInt32(r.Length) > StrokeNumber && r.Strokes[StrokeNumber].IsTopRight()).Count();

                    midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > StrokeNumber && r.Strokes[StrokeNumber].IsMidLeft()).Count();
                    midMid = rallies.Where(r => Convert.ToInt32(r.Length) > StrokeNumber && r.Strokes[StrokeNumber].IsMidMid()).Count();
                    midRight = rallies.Where(r => Convert.ToInt32(r.Length) > StrokeNumber && r.Strokes[StrokeNumber].IsMidRight()).Count();

                    botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > StrokeNumber && r.Strokes[StrokeNumber].IsBotLeft()).Count();
                    botMid = rallies.Where(r => Convert.ToInt32(r.Length) > StrokeNumber && r.Strokes[StrokeNumber].IsBotMid()).Count();
                    botRight = rallies.Where(r => Convert.ToInt32(r.Length) > StrokeNumber && r.Strokes[StrokeNumber].IsBotRight()).Count();
                }
                else if (lastStrokeOrOpeningShot == 1)
                {
                    topLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.LastWinnerStroke().IsTopLeft()).Count();
                    topMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.LastWinnerStroke().IsTopMid()).Count();
                    topRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.LastWinnerStroke().IsTopRight()).Count();

                    midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.LastWinnerStroke().IsMidLeft()).Count();
                    midMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.LastWinnerStroke().IsMidMid()).Count();
                    midRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.LastWinnerStroke().IsMidRight()).Count();

                    botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.LastWinnerStroke().IsBotLeft()).Count();
                    botMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.LastWinnerStroke().IsBotMid()).Count();
                    botRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.LastWinnerStroke().IsBotRight()).Count();

                }
                else
                {
                    topLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.OpeningShot().IsTopLeft()).Count();
                    topMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.OpeningShot().IsTopMid()).Count();
                    topRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.OpeningShot().IsTopRight()).Count();

                    midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.OpeningShot().IsMidLeft()).Count();
                    midMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.OpeningShot().IsMidMid()).Count();
                    midRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.OpeningShot().IsMidRight()).Count();

                    botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.OpeningShot().IsBotLeft()).Count();
                    botMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.OpeningShot().IsBotMid()).Count();
                    botRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.OpeningShot().IsBotRight()).Count();
                }
            }
            PositionCounts["BotLeft"] = botLeft;
            PositionCounts["BotMid"] = botMid;
            PositionCounts["BotRight"] = botRight;
            PositionCounts["MidLeft"] = midLeft;
            PositionCounts["MidMid"] = midMid;
            PositionCounts["MidRight"] = midRight;
            PositionCounts["TopLeft"] = topLeft;
            PositionCounts["TopMid"] = topMid;
            PositionCounts["TopRight"] = topRight;
            NotifyOfPropertyChange("PositionCounts");
        }


        #endregion
    }
}
