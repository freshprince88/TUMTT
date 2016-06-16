using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Windows.Controls;
using TT.Models.Util.Enums;
using TT.Lib.Events;
using TT.Lib.Managers;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class ResultViewModel : Conductor<IScreen>.Collection.AllActive
    {
        public ResultMiniStatisticViewModel MiniStatistic { get; set; }
        public ResultTabViewModel ResultTabView { get; set; }

        private IEventAggregator Events;
        private IMatchManager manager;

        public ResultViewModel(IEnumerable<IResultViewTabItem> tabs, IEventAggregator e, IMatchManager man)
        {
            Events = e;
            manager = man;
            //Items.AddRange(tabs);
            MiniStatistic = new ResultMiniStatisticViewModel(this.Events, manager);
            ResultTabView = new ResultTabViewModel(tabs);
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            // Subscribe ourself to the event bus
            this.Events.Subscribe(this);
            this.ActivateItem(MiniStatistic);
            this.ActivateItem(ResultTabView);
        }

        protected override void OnDeactivate(bool close)
        {
            Events.Unsubscribe(this);
            base.OnDeactivate(close);
        }
    }
}
