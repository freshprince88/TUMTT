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
            return FilterList.accepts(FilterType, rally);
        }

        public Rally[] filter(IEnumerable<Rally> inputRallies)
        {
            return FilterList.filter(FilterType, inputRallies).ToArray();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
