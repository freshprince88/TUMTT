using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Lib.Managers;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class ResultLargeTableViewModel : Screen, IResultViewTabItem,
        IHandle<ResultsChangedEvent>,
        IHandle<RallyLengthChangedEvent>,
        IHandle<FullscreenEvent>,
        IHandle<MediaControlEvent>
    {
        
        private IEventAggregator Events;
        private IDialogCoordinator Dialogs;
        private IMatchManager Manager;

        public ObservableCollection<ResultListItem> Items { get; set; }
        public List<Rally> Rallies { get; set; }

        private int rallyLength;
        public int RallyLength {
            get
            {
                return rallyLength;
            }
            set
            {
                rallyLength = value;
                NotifyOfPropertyChange();
            }
        }

        public ResultLargeTableViewModel()
        {
            // default constructor for caliburn design time integration
        }

        public ResultLargeTableViewModel(IEventAggregator e, IDialogCoordinator c, IMatchManager man)
        {
            this.DisplayName = Properties.Resources.table_large_tab_title;
            Events = e;
            Dialogs = c;
            Manager = man;
            RallyLength = 1;
            Items = new ObservableCollection<ResultListItem>();
            Rallies = new List<Rally>();

            // Subscribe ourself to the event bus
            Events.Subscribe(this);
        }

        public byte GetOrderInResultView()
        {
            return 1;
        }

        #region View Methods

        public void ListItemSelected(SelectionChangedEventArgs e)
        {
            ResultListItem item = e.AddedItems.Count > 0 ? (ResultListItem)e.AddedItems[0] : null;
            if (item != null)
            {
                Manager.ActiveRally = item.Rally;
            }           
        }

        public void RightMouseDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        
        public void StrokeSelected(object dataContext)
        {
            Console.Out.WriteLine("Selected Rally: {0}", ((Stroke) dataContext).Rally.Number);
            Manager.ActiveRally = (dataContext as Stroke).Rally;
        }

        private void UpdateStrokeDisplay(IEnumerable<Rally> rallies)
        {
            var strokes = new List<Models.Stroke>();            
            foreach (var r in rallies)
            {
                Stroke stroke;
                if (RallyLength == 5)
                {
                    stroke = r.Strokes.LastOrDefault();
                }
                else
                {
                    stroke = r.Strokes.SingleOrDefault(s =>
                    {
                        return s.Number == RallyLength;
                    });
                }
                if (stroke != null)
                    strokes.Add(stroke);
            }
            Events.PublishOnUIThread(new StrokesPaintEvent(strokes, RallyLength));
        }

        #endregion

        #region Event Handlers

        public void Handle(ResultsChangedEvent message)
        {
            if (IsActive)
                UpdateStrokeDisplay(message.Rallies);
        }

        public void Handle(RallyLengthChangedEvent message)
        {
            RallyLength = message;
        }

        public void Handle(FullscreenEvent message)
        {
        }

        public void Handle(MediaControlEvent message)
        {
        }

        #endregion

        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
                Events.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            UpdateStrokeDisplay(Manager. SelectedRallies);
        }

        #endregion
    }
}
