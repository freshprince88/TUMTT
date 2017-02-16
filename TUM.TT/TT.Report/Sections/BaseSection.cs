using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Report.Sections
{
    public abstract class BaseSection : IReportSection
    {
        protected abstract string SectionName { get; }

        protected string GetSetTitleString(string setName)
        {
            if (setName == "all")
                return Properties.Resources.sets_all;
            else if (setName == "crunchtime")
                return Properties.Resources.sets_crunchtime;
            else if (setName.Contains(","))
            {
                return string.Format("{0} {1}", Properties.Resources.sets_multiple, setName.Replace(',', '+'));
            }
            else
            {
                return string.Format("{0} {1}", Properties.Resources.sets_one, setName);
            }
        }

        protected string GetStrokeNumberString(int strokeNumber)
        {
            switch (strokeNumber)
            {
                case -1:
                    return "'all'";
                case int.MaxValue:
                    return "'last'";
                default:
                    return strokeNumber.ToString();
            }
        }
    }
}
