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

        #endregion
    }
}
