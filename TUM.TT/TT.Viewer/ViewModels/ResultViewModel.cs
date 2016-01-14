using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.ViewModels
{
    public class ResultViewModel : Conductor<IResultViewTabItem>.Collection.OneActive
    {
        public ResultViewModel(IEnumerable<IResultViewTabItem> tabs)
        {
            Items.AddRange(tabs);            
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            if (Items.Count() > 0)
                ActivateItem(Items[0]);
        }
    }
}
