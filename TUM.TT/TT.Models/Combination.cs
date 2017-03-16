using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;

namespace TT.Models
{
    public class Combination : IFilter
    {
        public BasicFilter BasicFilter;
        private IFilterCombiBase manager;
        public HashSet<Guid> FilterGuids;
        public FilterList FilterList
        {
            get
            {
                return new FilterList(manager.FilterList.Where(f => FilterGuids.Contains(f.ID)).ToList());
            }
        }

        public FilterCombination.CombinationType FilterType;

        public Combination(IFilterCombiBase Manager)
        {
            this.manager = Manager;
            FilterGuids = new HashSet<Guid>();
            BasicFilter = new Models.BasicFilter();
        }


        public bool accepts(Rally rally)
        {
            return FilterList.accepts(FilterType, rally);
        }

        public Rally[] filter(IEnumerable<Rally> inputRallies)
        {
            return FilterList.filter(FilterType, inputRallies).ToArray();
        }

        public void AddFilter(Filter f)
        {
            FilterGuids.Add(f.ID);
        }

        public void RemoveFilter(Filter f)
        {
            FilterGuids.RemoveWhere(g => g == f.ID);
        }
    }
}
