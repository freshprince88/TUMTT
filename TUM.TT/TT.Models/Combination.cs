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

        public bool accepts(Rally rally)
        {
            throw new NotImplementedException();
        }

        public Rally[] filter(IEnumerable<Rally> inputRallies)
        {
            throw new NotImplementedException();
        }
    }
}
