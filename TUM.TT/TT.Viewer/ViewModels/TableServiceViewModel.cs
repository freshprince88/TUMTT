using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Lib.Util.Enums;
using TT.Lib.Events;

namespace TT.Viewer.ViewModels
{
    public class TableServiceViewModel : Screen
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
            events = eventAggregator;
            SelectedServerPositions = new HashSet<Positions.Server>();
            SelectedPositions = new HashSet<Positions.Table>();
        }

        #region View Methods

        public void TablePositionClicked(ToggleButton toggle)
        {
            Positions.Table pos = GetTablePositionFromName(toggle.Name);
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
            Positions.Server pos = GetServePositionFromName(toggle.Name);
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
        protected override void OnInitialize()
        {
            base.OnInitialize();
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

        private Positions.Table GetTablePositionFromName(string name)
        {
            if (name.Contains("9"))
                return Positions.Table.BotRight;
            else if (name.Contains("8"))
                return Positions.Table.BotMid;
            else if (name.Contains("7"))
                return Positions.Table.BotLeft;
            else if (name.Contains("6"))
                return Positions.Table.MidRight;
            else if (name.Contains("5"))
                return Positions.Table.MidMid;
            else if (name.Contains("4"))
                return Positions.Table.MidLeft;
            else if (name.Contains("3"))
                return Positions.Table.TopRight;
            else if (name.Contains("2"))
                return Positions.Table.TopMid;
            else if (name.Contains("1"))
                return Positions.Table.TopLeft;
            else
                return Positions.Table.None;
        }

        private Positions.Server GetServePositionFromName(string name)
        {
            if (name.Contains("1"))
                return Positions.Server.Left;
            else if (name.Contains("2"))
                return Positions.Server.HalfLeft;
            else if (name.Contains("3"))
                return Positions.Server.Mid;
            else if (name.Contains("4"))
                return Positions.Server.HalfRight;
            else if (name.Contains("5"))
                return Positions.Server.Right;
            else
                return Positions.Server.None;
        }


        #endregion
    }
}
