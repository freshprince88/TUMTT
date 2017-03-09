using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class StatisticsViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public Playlist ActivePlaylist { get; set; }
        public int SelectedTab { get; set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;
        private readonly Dictionary<string, object[]> _tabNameDictionary;

        public StatisticsViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            this.ActivePlaylist = new Playlist(Manager.Match);

            _tabNameDictionary = new Dictionary<string, object[]>()
            {
                ["ServiceStatisticsTab"] = new object[] {new ServiceStatisticsViewModel(this.events, Manager), 1},
                ["ReceiveStatisticsTab"] = new object[] {new ReceiveStatisticsViewModel(this.events, Manager), 2},
                ["ThirdStatisticsTab"] = new object[] {new ThirdBallStatisticsViewModel(this.events, Manager), 3},
                ["FourthStatisticsTab"] = new object[] {new FourthBallStatisticsViewModel(this.events, Manager), 4},
                ["LastStatisticsTab"] = new object[] {new LastBallStatisticsViewModel(this.events, Manager), 5},
                ["TotalMatchStatisticsTab"] = new object[] {new TotalMatchStatisticsViewModel(this.events, Manager), 1}
            };

            SelectedTab = 0;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            // Activate the welcome model
            if (this.ActiveItem == null)
            {
                var s = _tabNameDictionary["ServiceStatisticsTab"];
                this.ActivateItem((IScreen) s[0]);
                Manager.CurrentRallyLength = (int) s[1];
            }
            else
            {
                var sel = _tabNameDictionary.Where(vmAndRallyLength => ActiveItem == vmAndRallyLength.Value[0]);
                var first = sel.First();
                Manager.CurrentRallyLength = (int)first.Value[1];
            }
        }

        protected override void OnDeactivate(bool close)
        {
            this.events.Unsubscribe(this);
            base.OnDeactivate(close);
        }


        public void FilterSelected(SelectionChangedEventArgs args)
        {
            var selected = args.AddedItems[0] as TabItem;
            if (selected == null)
                return;

            var s = _tabNameDictionary[selected.Name];
            ActivateItem((IScreen) s[0]);
            Manager.CurrentRallyLength = (int)s[1];
        }
    }
}
