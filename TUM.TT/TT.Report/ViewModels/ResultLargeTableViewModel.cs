using Caliburn.Micro;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TT.Models;
using System.Windows;
using TT.Converters;

namespace TT.Report.ViewModels
{
    public class ResultLargeTableViewModel : Screen
    {
        
        private IEventAggregator Events;
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

        public ResultLargeTableViewModel(IEventAggregator e, Match match, IWindowManager windowMan)
        {
            Events = e;
            WindowManager = windowMan;
            RallyLength = 1;
            Strokes = new ObservableCollection<Stroke>();
            Match = match;            
        }

        public byte GetOrderInResultView()
        {
            return 1;
        }

        public string GetTabTitle(bool getShortTitle)
        {
            return "";
        }

        #region View Methods

        public void StrokeSelected(object dataContext)
        {
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
            //RallyLength = Manager.CurrentRallyLength;
            //ActiveRally = Manager.ActiveRally;
            //UpdateStrokeDisplay(Manager.SelectedRallies);
        }

        #endregion
    }
}
