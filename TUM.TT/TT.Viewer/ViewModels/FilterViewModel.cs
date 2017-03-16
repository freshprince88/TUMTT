using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class FilterViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public int SelectedTab { get; set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IViewManager Manager;
        private readonly Dictionary<string, object[]> _tabNameDictionary;

        public FilterViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            this.Manager = new ViewManager(man);

            _tabNameDictionary = new Dictionary<string, object[]>()
            {
                ["ServiceFilterTab"] = new object[] { new ServiceViewModel(this.events, Manager, new Models.Filter(0, "Service")), 1 },
                ["ReceiveFilterTab"] = new object[] { new BallFilterViewModel(this.events, Manager, 1, "Recieve"), 2 },
                ["ThirdFilterTab"] = new object[] { new BallFilterViewModel(this.events, Manager, 2), 3 },
                ["FourthFilterTab"] = new object[] { new BallFilterViewModel(this.events, Manager, 3), 4 },
                ["LastFilterTab"] = new object[] { new LastBallViewModel(this.events, Manager), 5 },
                ["TotalMatchFilterTab"] = new object[] { new TotalMatchViewModel(this.events, Manager), 6 },
                ["KombiFilterTab"] = new object[] { new CombinationsViewModel(this.events, Manager.MatchManager, this), 7 }
            };

            SelectedTab = 0;
        }

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            
            if (this.ActiveItem == null)
            {
                var s = _tabNameDictionary["ServiceFilterTab"];
                this.ActivateItem((IScreen)s[0]);
                Manager.MatchManager.CurrentRallyLength = (int)s[1];
            }
            else
            {
                var sel = _tabNameDictionary.Where(vmAndRallyLength => ActiveItem == vmAndRallyLength.Value[0]);
                var first = sel.First();
                Manager.MatchManager.CurrentRallyLength = (int)first.Value[1];
            }
        }

        public void FilterSelected(SelectionChangedEventArgs args)
        {
            TabItem selected = args.AddedItems[0] as TabItem;
            if (selected == null)
                return;

            var s = _tabNameDictionary[selected.Name];
            ActivateItem((IScreen)s[0]);
            Manager.MatchManager.CurrentRallyLength = (int)s[1];
        }
    }
}
