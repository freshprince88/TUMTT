using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TT.Viewer.ViewModels
{
    class FilterViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private Expander _expander;
        public Screen ServiceView { get; set; }

        public bool IsExpanded
        {
            get
            {
                return _expander.IsExpanded;
            }
            set
            {
                _expander.IsExpanded = value;
            }
        }

        public FilterViewModel(IEventAggregator eventAggregator)
        {
            ServiceView = new ServiceViewModel(eventAggregator);
            this.ActivateItem(ServiceView);
        }
    }
}
