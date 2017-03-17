using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models
{
    public class CombinationList : ObservableCollection<Combination>,
                                    IRallyFilter
    {
        public CombinationList() : base() { }
        public CombinationList(List<Combination> list) : base(list) { }
        public CombinationList(IEnumerable<Combination> collection) : base(collection) { }

        public bool accepts(Rally rally)
        {
            return RallyFilterListMethods.accepts(Util.Enums.FilterCombination.CombinationType.Or, rally, this);
        }

        public Rally[] filter(IEnumerable<Rally> inputRallies)
        {
            return RallyFilterListMethods.filter(Util.Enums.FilterCombination.CombinationType.And, inputRallies, this);
        }
    }
}
