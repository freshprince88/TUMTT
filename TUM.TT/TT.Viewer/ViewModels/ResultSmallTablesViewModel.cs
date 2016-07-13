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
using System.Diagnostics;

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

        private int rallyLength;
        public int RallyLength
        {
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
        
        public void RallySelected(object dataContext)
        {
            Debug.WriteLine("Selected rally {0}", ((Rally)dataContext).Number);
            Manager.ActiveRally = (Rally)dataContext;
        }

        public void StrokeSelected(object dataContext)
        {
            Console.Out.WriteLine("Selected stroke {1} of rally: {0}", ((Stroke)dataContext).Rally.Number, ((Stroke)dataContext).Number);
            Manager.ActiveRally = (dataContext as Stroke).Rally;
        }

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
