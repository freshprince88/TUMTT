using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TT.Viewer.ViewModels
{
    public partial class PlacementStatisticsViewModel : Screen
    {
        private IEventAggregator events;

        public PlacementStatisticsViewModel (IEventAggregator eventAggregator)
        {
            events = eventAggregator;
        }
    }
}
