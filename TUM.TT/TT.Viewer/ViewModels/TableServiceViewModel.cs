using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Lib.Util.Enums;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Lib.Models;

namespace TT.Viewer.ViewModels
{
    public class TableServiceViewModel : Screen, IHandle<ResultsChangedEvent>
    {

        private IEventAggregator events;
        public HashSet<Positions.Table> SelectedPositions { get; set; }
        public HashSet<Positions.Server> SelectedServerPositions { get; set; }       

        #region Enums       

        #endregion

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

        public TableServiceViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedServerPositions = new HashSet<Positions.Server>();
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
            this.events.PublishOnUIThread(new TableViewSelectionChangedEvent(SelectedPositions, SelectedServerPositions));
        }

        public void ServerPositionClicked(ToggleButton toggle)
        {
            Positions.Server pos = Positions.GetServePositionFromName(toggle.Name);
            if (toggle.IsChecked.Value)
            {
                if (pos != Positions.Server.None)
                    SelectedServerPositions.Add(pos);
            }
            else
            {
                if (pos != Positions.Server.None)
                    SelectedServerPositions.Remove(pos);
            }
            this.events.PublishOnUIThread(new TableViewSelectionChangedEvent(SelectedPositions, SelectedServerPositions));
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


            if (rallies != null)
            {
                

                    topLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsTopLeft()).Count();
                    topMid = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsTopMid()).Count();
                    topRight = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsTopRight()).Count();

                    midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsMidLeft()).Count();
                    midMid = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsMidMid()).Count();
                    midRight = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsMidRight()).Count();

                    botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsBotLeft()).Count();
                    botMid = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsBotMid()).Count();
                    botRight = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsBotRight()).Count();
               
            }
            Dictionary<string, int> positionKeys = new Dictionary<string, int>();
            positionKeys.Add("BotLeft", botLeft);
            positionKeys.Add("BotMid", botMid);
            positionKeys.Add("BotRight", botRight);
            positionKeys.Add("MidLeft", midLeft);
            positionKeys.Add("MidMid", midMid);
            positionKeys.Add("MidRight", midRight);
            positionKeys.Add("TopLeft", topLeft);
            positionKeys.Add("TopMid", topMid);
            positionKeys.Add("TopRight", topRight);

            events.PublishOnUIThread(new ShowTableNumbersEvent(positionKeys));
        }

        #endregion
    }
}
