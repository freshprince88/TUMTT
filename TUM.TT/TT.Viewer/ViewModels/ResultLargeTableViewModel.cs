using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TT.Lib.Events;
using TT.Lib.Managers;
using MahApps.Metro.Controls.Dialogs;
using TT.Models;
using System.Windows;
using System.Dynamic;
using System.Windows.Controls.Primitives;

namespace TT.Viewer.ViewModels
{
    public class ResultLargeTableViewModel : Screen, IResultViewTabItem,
        IHandle<ResultsChangedEvent>,
        IHandle<RallyLengthChangedEvent>,
        IHandle<MediaControlEvent>,
        IHandle<ActiveRallyChangedEvent>
    {
        
        private IEventAggregator Events;
        private IDialogCoordinator Dialogs;
        private IMatchManager Manager;
        private IWindowManager WindowManager;

        public ObservableCollection<Rally> Rallies { get; set; }
        private ObservableCollection<Stroke> strokes;
        public ObservableCollection<Stroke> Strokes
        {
            get
            {
                return strokes;
            }
            set
            {
                strokes = value;
                NotifyOfPropertyChange();
            }
        }

        private int rallyLength;
        public int RallyLength { get
            {
                return rallyLength;
            }
            set
            {
                rallyLength = value;
                NotifyOfPropertyChange();
            }
        }

        public Match Match { get; set; }

        private Rally activeRally;
        public Rally ActiveRally
        {
            get { return activeRally; }
            set
            {
                activeRally = value;
                NotifyOfPropertyChange();
            }
        }
        
        public ResultLargeTableViewModel()
        {
            // default constructor for caliburn design time integration
        }

        public ResultLargeTableViewModel(IEventAggregator e, IDialogCoordinator c, IMatchManager man, IWindowManager windowMan)
        {
            this.DisplayName = Properties.Resources.table_large_tab_title;
            Events = e;
            Dialogs = c;
            Manager = man;
            WindowManager = windowMan;
            RallyLength = 1;
            Strokes = new ObservableCollection<Stroke>();
            Match = Manager.Match;            
        }

        public byte GetOrderInResultView()
        {
            return 1;
        }

        public string GetTabTitle(bool getShortTitle)
        {
            if (getShortTitle)
                return Properties.Resources.table_large_tab_title_short;
            else
                return Properties.Resources.table_large_tab_title;
        }

        #region View Methods

        public void StrokeSelected(object dataContext)
        {
            if (dataContext == null)
            {
                Console.Out.WriteLine("No stroke selected");
                ActiveRally = null;
                return;
            }

            Stroke selectedStroke = (Stroke)dataContext;
            Console.Out.WriteLine("Selected stroke {1} of rally: {0}", selectedStroke.Rally.Number, selectedStroke.Number);
            ActiveRally = selectedStroke.Rally;
            Manager.ActiveRally = selectedStroke.Rally;
        }

        private void UpdateStrokeDisplay(IEnumerable<Rally> rallies)
        {
            Rallies = new ObservableCollection<Rally>(rallies);
            var strokes = new List<Stroke>();            
            foreach (var r in Rallies)
            {
                Stroke stroke;
                if (RallyLength == 5)
                {
                    stroke = r.LastWinnerStroke();
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

            // more elegant solution, doesn't seem to respect property change notifications though
            //Strokes.Clear();
            //strokes.ForEach(stroke => Strokes.Add(stroke));

            Strokes = new ObservableCollection<Stroke>(strokes);
        }

        public void ShowLegend(UIElement e)
        {            
            dynamic settings = new ExpandoObject();
            settings.StaysOpen = false;
            settings.PlacementTarget = e;
            settings.Placement = PlacementMode.Bottom;
            WindowManager.ShowPopup(new TableLegendViewModel(), null, settings);
        }

        #endregion

        #region Event Handlers

        public void Handle(ResultsChangedEvent message)
        {
            UpdateStrokeDisplay(message.Rallies);
        }

        public void Handle(RallyLengthChangedEvent message)
        {
            if (message != RallyLength)
            {
                RallyLength = message;
                UpdateStrokeDisplay(Rallies);
            }
        }

        public void Handle(MediaControlEvent message)
        {
            if (message.Source == Models.Util.Enums.Media.Source.Viewer)
            {
                var idx = Rallies.IndexOf(Manager.ActiveRally);
                switch (message.Ctrl)
                {
                    case Models.Util.Enums.Media.Control.Previous:
                        var rallyP = idx - 1 >= 0 ? Rallies[idx - 1] : null;
                        if (rallyP != null)
                        {
                            Manager.ActiveRally = rallyP;
                        }
                        break;
                    case Models.Util.Enums.Media.Control.Next:
                        if (Rallies.Count() != 0)
                        {
                            var rallyN = idx + 1 < Rallies.Count ? Rallies[idx + 1] : Rallies[0];
                            if (rallyN != null && rallyN != Rallies[0])
                            {
                                Manager.ActiveRally = rallyN;
                            }
                            else if (rallyN != null && rallyN == Rallies[0])
                            {
                                Events.PublishOnUIThread(new MediaControlEvent(Models.Util.Enums.Media.Control.Pause, Models.Util.Enums.Media.Source.Viewer));
                            }

                        }
                        else
                        {
                            Events.PublishOnUIThread(new MediaControlEvent(Models.Util.Enums.Media.Control.Stop, Models.Util.Enums.Media.Source.Viewer));
                        }
                        break;
                    case Models.Util.Enums.Media.Control.Stop:

                    default:
                        break;
                }
            }
        }

        public void Handle(ActiveRallyChangedEvent message)
        {
            ActiveRally = message.Current;
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
            base.OnDeactivate(close);
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            RallyLength = Manager.CurrentRallyLength;
            ActiveRally = Manager.ActiveRally;
            UpdateStrokeDisplay(Manager.SelectedRallies);
        }

        #endregion
    }
}
