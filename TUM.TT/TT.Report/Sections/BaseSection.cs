using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Report.Sections
{
    public class BaseSection : IReportSection
    {
        protected string GetSetTitleString(string setName)
        {
            if (setName == "all")
                return Properties.Resources.sets_all;
            else if (setName.Contains(","))
            {
                return string.Format("{0} {1}", Properties.Resources.sets_multiple, setName.Replace(',', '+'));
            }
            else
            {
                return string.Format("{0} {1}", Properties.Resources.sets_one, setName);
            }
        }
    }
}
