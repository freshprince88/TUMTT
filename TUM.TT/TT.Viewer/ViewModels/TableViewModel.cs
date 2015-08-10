using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class TableViewModel : Screen
    {

        private IEventAggregator events;

        public enum ViewMode
        {
            Top,
            Bottom
        }

        public enum TablePosition
        {
            TopLeft = 1,
            TopMid,
            TopRight,
            MidLeft,
            MidMid,
            MidRight,
            BotLeft,
            BotMid,
            BotRight
        }

        public enum StrokeLength
        {
            Short,
            Half,
            Long
        }

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
                    events.PublishOnUIThread(new TableViewModeChangedEvent(value));

                _mode = value;
                
            }
        }

        public TableViewModel(IEventAggregator eventAggregator)
        {
            events = eventAggregator;
        }

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
    }
}
