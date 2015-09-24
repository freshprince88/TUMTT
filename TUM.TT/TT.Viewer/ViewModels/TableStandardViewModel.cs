using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class TableStandardViewModel : Screen
    {

        private IEventAggregator events;
        public HashSet<ETablePosition> SelectedPositions { get; set; }
        public HashSet<EStrokeLength> SelectedStrokeLength { get; set; }

        public enum ViewMode
        {
            Top,
            Bottom
        }

        #region Enums

        public enum EStrokeLength
        {
            Short,
            Half,
            Long,
            None
        }

        public enum ETablePosition
        {
            TopLeft = 1,
            TopMid,
            TopRight,
            MidLeft,
            MidMid,
            MidRight,
            BotLeft,
            BotMid,
            BotRight,
            None
        }

        #endregion

        private ViewMode _mode;
        public ViewMode Mode
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

        public TableStandardViewModel(IEventAggregator eventAggregator)
        {
            events = eventAggregator;
            SelectedStrokeLength = new HashSet<EStrokeLength>();
            SelectedPositions = new HashSet<ETablePosition>();
        }

        #region View Methods

        public void TablePositionClicked(ToggleButton toggle)
        {
            ETablePosition pos = GetTablePositionFromName(toggle.Name);
            if (toggle.IsChecked.Value)
            {
                if(pos != ETablePosition.None)
                    SelectedPositions.Add(pos);
            }
            else
            {
                if (pos != ETablePosition.None)
                    SelectedPositions.Remove(pos);
            }
            this.events.PublishOnUIThread(new TableStdViewSelectionChangedEvent(SelectedPositions, SelectedStrokeLength));
        }

        public void StrokeLengthClicked(ToggleButton toggle)
        {
            EStrokeLength pos = GetStrokeLengthFromName(toggle.Name);
            if (toggle.IsChecked.Value)
            {
                if (pos != EStrokeLength.None)
                    SelectedStrokeLength.Add(pos);
            }
            else
            {
                if (pos != EStrokeLength.None)
                    SelectedStrokeLength.Remove(pos);
            }
            this.events.PublishOnUIThread(new TableStdViewSelectionChangedEvent(SelectedPositions, SelectedStrokeLength));
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

        private ETablePosition GetTablePositionFromName(string name)
        {
            if (name.Contains("9"))
                return ETablePosition.BotRight;
            else if (name.Contains("8"))
                return ETablePosition.BotMid;
            else if (name.Contains("7"))
                return ETablePosition.BotLeft;
            else if (name.Contains("6"))
                return ETablePosition.MidRight;
            else if (name.Contains("5"))
                return ETablePosition.MidMid;
            else if (name.Contains("4"))
                return ETablePosition.MidLeft;
            else if (name.Contains("3"))
                return ETablePosition.TopRight;
            else if (name.Contains("2"))
                return ETablePosition.TopMid;
            else if (name.Contains("1"))
                return ETablePosition.TopLeft;
            else
                return ETablePosition.None;
        }

        private EStrokeLength GetStrokeLengthFromName(string name)
        {
            if (name.Contains("1"))
                return EStrokeLength.Short;
            else if (name.Contains("2"))
                return EStrokeLength.Half;
            else if (name.Contains("3"))
                return EStrokeLength.Long;
            else
                return EStrokeLength.None;
        }


        #endregion
    }
}
