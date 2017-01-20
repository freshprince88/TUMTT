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

        public ResultTabViewModel(IEnumerable<IResultViewTabItem> tabs, IEventAggregator e)
        {
            Items.AddRange(tabs);

            Events = e;
            Events.Subscribe(this);
        }

        public void Handle(FullscreenEvent message)
        {
            foreach (IResultViewTabItem tab in Items)
            {
                tab.DisplayName = tab.GetTabTitle(message.Fullscreen);
            }
            if (message.Fullscreen)
                _lastNonFullScreenItem = ActiveItem;
            else
                ActiveItem = _lastNonFullScreenItem;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            if (Items.Count() > 0)
                ActivateItem(Items[0]);
        }
    }
}