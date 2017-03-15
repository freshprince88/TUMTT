using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;


namespace TT.Models
{
    public class FilterList : ObservableCollection<Filter>
    {
        public FilterList() : base() { }

        public FilterList(IEnumerable<Filter> collection) : base(collection) { }

        public FilterList(List<Filter> list) : base(list) { }

        public IEnumerable<Filter> EnabledFilter
        {
            get
            {
                return this.Where(f => f.Enabled);
            }
        }


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

        public void EnableFilter(Filter f)
        {
            if (this.Contains(f))
            {
                var oldFilter = new Filter(f);
                f.Enabled = true;
                OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Replace, f, oldFilter, this.IndexOf(f)));
            }
        }

        public void DisableFilter(Filter f)
        {
            if (this.Contains(f))
            {
                var oldFilter = new Filter(f);
                f.Enabled = false;
                OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Replace, f, oldFilter, this.IndexOf(f)));
            }
        }

        public void ToogleFilter(Filter f, bool enabled)
        {
            if (enabled)
                EnableFilter(f);
            else
                DisableFilter(f);
        }

        private IEnumerable<Rally> filterOr(IEnumerable<Rally> rallies)
        {
            List<Rally> allResults = new List<Rally>();
            foreach (Rally rally in rallies)
            {
                bool isAcceptedByOne = false;
                foreach(Filter f in EnabledFilter)
                {
                    if (f.accepts(rally)) isAcceptedByOne = true;
                }
                if (isAcceptedByOne || EnabledFilter.Count() == 0) allResults.Add(rally);
            }

            return allResults;
        }

        private IEnumerable<Rally> filterAnd(IEnumerable<Rally> rallies)
        {
            Rally[] temp = rallies.ToArray();
            foreach (Filter f in EnabledFilter)
            {
                temp = f.filter(temp);
            }
            return temp;
        }
    }
}
