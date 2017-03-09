using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Lib.Events;
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
        private IMatchManager Manager;
        private readonly Dictionary<string, object[]> _tabNameDictionary;

        public FilterViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            this.Manager = man;

            _tabNameDictionary = new Dictionary<string, object[]>()
            {
                ["ServiceFilterTab"] = new object[] { new ServiceViewModel(this.events, Manager), 1 },
                ["ReceiveFilterTab"] = new object[] { new ReceiveViewModel(this.events, Manager), 2 },
                ["ThirdFilterTab"] = new object[] { new ThirdBallViewModel(this.events, Manager), 3 },
                ["FourthFilterTab"] = new object[] { new FourthBallViewModel(this.events, Manager), 4 },
                ["LastFilterTab"] = new object[] { new LastBallViewModel(this.events, Manager), 5 },
                ["TotalMatchFilterTab"] = new object[] { new TotalMatchViewModel(this.events, Manager), 1 },
                ["KombiFilterTab"] = new object[] { new CombiViewModel(this.events, Manager), 1 }
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
                Manager.CurrentRallyLength = (int)s[1];
            }
            else
            {
                var sel = _tabNameDictionary.Where(vmAndRallyLength => ActiveItem == vmAndRallyLength.Value[0]);
                var first = sel.First();
                Manager.CurrentRallyLength = (int)first.Value[1];
            }
        }

        public void FilterSelected(SelectionChangedEventArgs args)
        {
            TabItem selected = args.AddedItems[0] as TabItem;
            if (selected == null)
                return;

            var s = _tabNameDictionary[selected.Name];
            ActivateItem((IScreen)s[0]);
            Manager.CurrentRallyLength = (int)s[1];
        }
    }
}
