using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;

namespace TT.Models
{
    public interface IRallyFilter
    {
        Rally[] filter(IEnumerable<Rally> inputRallies);
        bool accepts(Rally rally);
    }

    public static class RallyFilterListMethods
    {
        public static Rally[] filter(FilterCombination.CombinationType combinationType, IEnumerable<Rally> rallies, IEnumerable<IRallyFilter> rallyFilters)
        {
            switch (combinationType)
            {
                case FilterCombination.CombinationType.And:
                    return filterAnd(rallies, rallyFilters).ToArray();
                case FilterCombination.CombinationType.Or:
                    return filterOr(rallies, rallyFilters).ToArray();
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool accepts(FilterCombination.CombinationType combinationType, Rally rally, IEnumerable<IRallyFilter> rallyFilters)
        {
            switch (combinationType)
            {
                case FilterCombination.CombinationType.And:
                    return IsAcceptedAnd(rally, rallyFilters);
                case FilterCombination.CombinationType.Or:
                    return isAcceptedOr(rally, rallyFilters);
                default:
                    throw new NotImplementedException();
            }
        }

        private static IEnumerable<Rally> filterOr(IEnumerable<Rally> rallies, IEnumerable<IRallyFilter> rallyFilters)
        {
            List<Rally> allResults = new List<Rally>();
            foreach (Rally rally in rallies)
            {
                if (isAcceptedOr(rally, rallyFilters))
                    allResults.Add(rally);
            }

            return allResults;
        }

        private static bool isAcceptedOr(Rally rally, IEnumerable<IRallyFilter> rallyFilters)
        {
            bool isAcceptedByOne = false;
            foreach (IRallyFilter f in rallyFilters)
            {
                if (f.accepts(rally)) isAcceptedByOne = true;
            }
            return isAcceptedByOne;
        }

        private static IEnumerable<Rally> filterAnd(IEnumerable<Rally> rallies, IEnumerable<IRallyFilter> rallyFilters)
        {
            Rally[] temp = rallies.ToArray();
            foreach (IRallyFilter f in rallyFilters)
            {
                temp = f.filter(temp);
            }
            return temp;
        }

        private static bool IsAcceptedAnd(Rally rally, IEnumerable<IRallyFilter> rallyFilters)
        {
            bool isAcceptedByAll = true;
            foreach (IRallyFilter f in rallyFilters)
            {
                if (!f.accepts(rally)) isAcceptedByAll = false;
            }
            return isAcceptedByAll;
        }
    }
}
