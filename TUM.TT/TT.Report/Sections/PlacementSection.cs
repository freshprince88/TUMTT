using System.Collections.Generic;
using TT.Models;
using TT.Report.Views;

namespace TT.Report.Sections
{
    public class PlacementSection : ExistingStatisticsSection
    {
        protected override string SectionName => "Placement section";

        public PlacementSection(int strokeNumber, IDictionary<string, List<Rally>> sets, Match match, object p)
        {
            base.GetImageBitmapFrames(strokeNumber, sets, match, p, typeof(StrokePlacementGridView));
        }
    }
}