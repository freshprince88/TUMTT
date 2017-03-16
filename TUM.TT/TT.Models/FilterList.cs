using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;


namespace TT.Models
{
    public class FilterList : ReadOnlyCollection<Filter>
    {
        public FilterList(List<Filter> list) : base(list) { }


        public IEnumerable<Rally> filter(FilterCombination.CombinationType combinationType, IEnumerable<Rally> rallies)
        {
            switch (combinationType)
            {
                case FilterCombination.CombinationType.And:
                    return filterAnd(rallies);
                case FilterCombination.CombinationType.Or:
                    return filterOr(rallies);
                default:
                    throw new NotImplementedException();
            }
        }

        public bool accepts(FilterCombination.CombinationType combinationType, Rally rally)
        {
            switch (combinationType)
            {
                case FilterCombination.CombinationType.And:
                    return IsAcceptedAnd(rally);
                case FilterCombination.CombinationType.Or:
                    return isAcceptedOr(rally);
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<Rally> filterOr(IEnumerable<Rally> rallies)
        {
            List<Rally> allResults = new List<Rally>();
            foreach (Rally rally in rallies)
            {
                if (isAcceptedOr(rally))
                    allResults.Add(rally);
            }

            return allResults;
        }

        private bool isAcceptedOr(Rally rally)
        {
            bool isAcceptedByOne = false;
            foreach (Filter f in this)
            {
                if (f.accepts(rally)) isAcceptedByOne = true;
            }
            return isAcceptedByOne;
        }

        private IEnumerable<Rally> filterAnd(IEnumerable<Rally> rallies)
        {
            Rally[] temp = rallies.ToArray();
            foreach (Filter f in this)
            {
                temp = f.filter(temp);
            }
            return temp;
        }

        private bool IsAcceptedAnd(Rally rally)
        {
            bool isAcceptedByAll = true;
            foreach (Filter f in this)
            {
                if (!f.accepts(rally)) isAcceptedByAll = false;
            }
            return isAcceptedByAll;
        }
    }
}
