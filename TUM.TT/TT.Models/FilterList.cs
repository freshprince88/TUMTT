using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;


namespace TT.Models
{
    public class FilterList : ObservableCollection<Filter>,
        IRallyFilter
    {
        public FilterList() : base() { }
        public FilterList(List<Filter> list) : base(list) { }
        public FilterList(IEnumerable<Filter> collection) : base(collection) { }

        public Rally[] filter(FilterCombination.CombinationType combinationType, IEnumerable<Rally> rallies)
        {
            return RallyFilterListMethods.filter(combinationType, rallies, this);
        }

        public bool accepts(FilterCombination.CombinationType combinationType, Rally rally)
        {
            return RallyFilterListMethods.accepts(combinationType, rally, this);
        }

        /// <summary>
        /// Applies ALL FILTERS on the inputtype (CombinationType = AND)
        /// </summary>
        /// <param name="inputRallies">Rallies to filter.</param>
        public Rally[] filter(IEnumerable<Rally> inputRallies)
        {
            return this.filter(FilterCombination.CombinationType.And, inputRallies);
        }

        /// <summary>
        /// Checks if a Rally is accepted by ALL Filters
        /// </summary>
        /// <param name="inputRallies">Rally to filter.</param>
        public bool accepts(Rally rally)
        {
            return this.accepts(FilterCombination.CombinationType.And, rally);
        }
    }
}
