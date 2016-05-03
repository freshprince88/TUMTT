using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.ViewModels
{
    public partial class PlacementStatisticsTableViewModel : Screen
    {
        private IEventAggregator events;


        public PlacementStatisticsTableViewModel(IEventAggregator eventAggregator)
        {
            events = eventAggregator;
            
        }
    }
}
