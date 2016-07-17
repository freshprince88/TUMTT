using Caliburn.Micro;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using TT.Lib.Events;
using TT.Lib.Managers;
using MahApps.Metro.Controls.Dialogs;
using TT.Models;
using System.Diagnostics;
using System.Windows;
using System.Dynamic;
using System.Windows.Controls.Primitives;
using TT.Models.Util.Enums;

namespace TT.Viewer.ViewModels
{
    public class ResultSmallTablesViewModel : Screen, IResultViewTabItem,
        IHandle<ResultsChangedEvent>,
        IHandle<RallyLengthChangedEvent>,
        IHandle<FullscreenEvent>,
        IHandle<MediaControlEvent>,
        IHandle<ActiveRallyChangedEvent>
    {
        
        private IEventAggregator Events;
        private IDialogCoordinator Dialogs;
        private IMatchManager Manager;
        private IWindowManager WindowManager;

        public ObservableCollection<Rally> Rallies { get; set; }

        private Rally activeRally;
        public Rally ActiveRally
        {
            get
            {
                return activeRally;
            }
            set
            {
                activeRally = value;
                NotifyOfPropertyChange();
            }
        }

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

        public ResultSmallTablesViewModel(IEventAggregator e, IDialogCoordinator c, IMatchManager man, IWindowManager winMan)
        {
            this.DisplayName = Properties.Resources.table_small_tab_title;
            Events = e;
            Dialogs = c;
            Manager = man;
            WindowManager = winMan;
            RallyLength = 1;
            Rallies = new ObservableCollection<Rally>();         
        }

        public byte GetOrderInResultView()
        {
            return 2;
        }
        
        public void RallySelected(object dataContext)
        {
            Debug.WriteLine("Selected rally {0}", ((Rally)dataContext).Number);
            ActiveRally = (Rally)dataContext;
            Manager.ActiveRally = (Rally)dataContext;
        }

        public void StrokeSelected(object dataContext)
        {
            Console.Out.WriteLine("Selected stroke {1} of rally: {0}", ((Models.Stroke)dataContext).Rally.Number, ((Models.Stroke)dataContext).Number);
            Manager.ActiveRally = (dataContext as Models.Stroke).Rally;
        }

        public void ShowLegend(UIElement e)
        {
            dynamic settings = new ExpandoObject();
            settings.StaysOpen = false;
            settings.PlacementTarget = e;
            settings.Placement = PlacementMode.Bottom;
            WindowManager.ShowPopup(new TableLegendViewModel(), null, settings);
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

        public void Handle(ActiveRallyChangedEvent message)
        {
            ActiveRally = message.Current;
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
            if (message.Source == Media.Source.Viewer)
            {
                var idx = Rallies.IndexOf(Manager.ActiveRally);
                switch (message.Ctrl)
                {
                    case Media.Control.Previous:
                        var rallyP = idx - 1 >= 0 ? Rallies[idx - 1] : null;
                        if (rallyP != null)
                        {
                            RallySelected(rallyP);
                        }
                        break;
                    case Media.Control.Next:
                        if (Rallies.Count() != 0)
                        {
                            var rallyN = idx + 1 < Rallies.Count ? Rallies[idx + 1] : Rallies[0];
                            if (rallyN != null && rallyN != Rallies[0])
                            {
                                RallySelected(rallyN);
                            }
                            else if (rallyN != null && rallyN == Rallies[0])
                            {
                                Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
                            }

                        }
                        else
                        {
                            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Stop, Media.Source.Viewer));
                        }
                        break;
                    case Media.Control.Stop:

                    default:
                        break;
                }
            }
        }

        #endregion

        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            Events.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            Events.Unsubscribe(this);
            Rallies.Clear();
            base.OnDeactivate(close);
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            Rallies.Clear();

            RallyLength = Manager.CurrentRallyLength;

            Manager.SelectedRallies.Apply(rally => Rallies.Add(rally));

            Rally activeRally = Manager.ActiveRally;
            if (activeRally != null)
                ActiveRally = activeRally;
        }

        #endregion
    }
}
