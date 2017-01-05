using System.Collections.Generic;
using TT.Models;
using TT.Report.Views;

namespace TT.Report.Sections
{
    public class TechniqueSection : ExistingStatisticsSection, IReportSection
    {
        public TechniqueSection(int strokeNumber, IDictionary<string, List<Rally>> sets, Match match, object p)
        {
            base.GetImageBitmapFrames(strokeNumber, sets, match, p, new ServiceTechniqueGridView());
        }
    }
}