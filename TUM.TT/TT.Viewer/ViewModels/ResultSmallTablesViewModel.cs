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
    public class ResultSmallTablesViewModel : Screen, IResultViewTabItem,
        IHandle<ResultsChangedEvent>,
        IHandle<RallyLengthChangedEvent>,
        IHandle<FullscreenEvent>,
        IHandle<MediaControlEvent>
    {
        
        private IEventAggregator Events;
        private IDialogCoordinator Dialogs;
        private IMatchManager Manager;

        public ObservableCollection<Rally> Rallies { get; set; }
        
        public int RallyLength { get; set; }

        public ResultSmallTablesViewModel()
        {
            // default constructor for caliburn design time integration
        }

        public ResultSmallTablesViewModel(IEventAggregator e, IDialogCoordinator c, IMatchManager man)
        {
            this.DisplayName = Properties.Resources.table_small_tab_title;
            Events = e;
            Dialogs = c;
            Manager = man;
            RallyLength = 1;
            Rallies = new ObservableCollection<Rally>();

            // Subscribe ourself to the event bus
            Events.Subscribe(this);            
        }

        public byte GetOrderInResultView()
        {
            return 2;
        }

        #region View Methods
        
        public void StrokeSelected(object dataContext)
        {
            Console.Out.WriteLine("Selected Rally: {0}", ((Stroke) dataContext).Rally.Number);
            Manager.ActiveRally = (dataContext as Stroke).Rally;
        }

        private void UpdateStrokeDisplay(IEnumerable<Rally> rallies)
        {
            var strokes = new List<Stroke>();            
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
        }

        #endregion

        #region Event Handlers

        public void Handle(ResultsChangedEvent message)
        {
            if (IsActive)
            {
                Rallies.Clear();
                message.Rallies.Apply(rally => Rallies.Add(rally));
            }
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
            Rallies.Clear();
            Manager.SelectedRallies.Apply(rally => Rallies.Add(rally));
        }

        #endregion
    }
}
