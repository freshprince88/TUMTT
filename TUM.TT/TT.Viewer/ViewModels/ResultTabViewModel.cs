using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using TT.Lib.Events;

namespace TT.Viewer.ViewModels
{
    public class ResultTabViewModel : Conductor<IResultViewTabItem>.Collection.OneActive,
        IHandle<FullscreenEvent>
    {
        private IEventAggregator Events;
        private IResultViewTabItem _lastNonFullScreenItem;
        private IEnumerable<IResultViewTabItem> _allTabs;

        public ResultTabViewModel(IEnumerable<IResultViewTabItem> tabs, IEventAggregator e)
        {
            _allTabs = tabs;
            Items.AddRange(_allTabs);

            Events = e;
            Events.Subscribe(this);
        }

        public void Handle(FullscreenEvent message)
        {
            if (message.Fullscreen)
            {
                _lastNonFullScreenItem = ActiveItem;
                Items.Clear();
                Items.Add(_allTabs.First(t => t is ResultListViewModel));
                ActiveItem = Items.First();

            }
            else
            {
                Items.Clear();
                Items.AddRange(_allTabs);
                ActiveItem = _lastNonFullScreenItem;
            }

            foreach (IResultViewTabItem tab in Items)
            {
                tab.DisplayName = tab.GetTabTitle(message.Fullscreen);
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            if (Items.Count() > 0)
                ActivateItem(Items[0]);
        }
    }
}