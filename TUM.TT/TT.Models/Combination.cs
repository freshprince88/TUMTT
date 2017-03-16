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
        public FilterList FilterList;

        public FilterCombination.CombinationType FilterType;

        public Combination()
        {
            FilterList = new FilterList();
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
    }
}
