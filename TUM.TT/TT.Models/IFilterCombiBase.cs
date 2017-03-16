using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models
{
    public interface IFilterCombiBase
    {
        ObservableCollection<Filter> FilterList { get; }
        ObservableCollection<Combination> CombinationList { get; }
    }
}
