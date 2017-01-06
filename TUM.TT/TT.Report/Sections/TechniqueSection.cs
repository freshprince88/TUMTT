using System.Collections.Generic;
using System.Windows.Threading;
using TT.Models;
using TT.Report.Views;

namespace TT.Report.Sections
{
    public class TechniqueSection : ExistingStatisticsSection, IReportSection
    {
        public TechniqueSection(int strokeNumber, IDictionary<string, List<Rally>> sets, Match match, object p)
        {
            if (strokeNumber == 1)
                base.GetImageBitmapFrames(strokeNumber, sets, match, p, new ServiceTechniqueGridView());
            else
            {
                // Dispatcher is needed here because TechniqueGridView modifies the Grid (Column/Rows) based on ordering of counts
                Dispatcher.CurrentDispatcher.Invoke(() => base.GetImageBitmapFrames(strokeNumber, sets, match, p, new TechniqueGridView()));
            }
        }
    }
}