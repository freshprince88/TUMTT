using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util.Enums;

namespace TT.Models
{
    public class Combination : IRallyFilter
    {
        public const string COMBINATION_PATH = "Combinations";

        public Guid ID;
        public DateTime CreationDate;

        public string Name;

        public BasicFilter BasicFilter;
        public FilterList FilterList;

        public FilterCombination.CombinationType FilterType;

        public Combination()
        {
            ID = Guid.NewGuid();
            CreationDate = DateTime.Now;
            Name = "";
            FilterList = new FilterList();
            BasicFilter = new Models.BasicFilter();
        }

        public Combination Copy()
        {
            var newCombi = new Combination();

            newCombi.ID = this.ID;
            newCombi.CreationDate = this.CreationDate;

            newCombi.Name = this.Name;

            newCombi.FilterList = new Models.FilterList(this.FilterList.Select(f => f.Copy()));
            newCombi.BasicFilter = this.BasicFilter.Copy();

            return newCombi;
        }


        public bool accepts(Rally rally)
        {
            if (FilterType == FilterCombination.CombinationType.Or)
                return FilterList.accepts(FilterType, rally) || BasicFilter.accepts(rally);
            else if(FilterType == FilterCombination.CombinationType.And)
                return FilterList.accepts(FilterType, rally) && BasicFilter.accepts(rally);

            throw new NotImplementedException("FilterCombination is only implemented for AND & OR");
        }

        public Rally[] filter(IEnumerable<Rally> inputRallies)
        {
            var result = new List<Rally>();
            foreach(Rally r in inputRallies)
            {
                if (this.accepts(r)) result.Add(r);
            }
            return result.ToArray();
        }

        public override string ToString()
        {
            return Name;
        }

        public void Load(Combination c)
        {
            this.ID = c.ID;
            this.CreationDate = c.CreationDate;

            this.Name = c.Name;

            this.FilterList = c.FilterList;
            this.BasicFilter = c.BasicFilter;
        }
    }
}
