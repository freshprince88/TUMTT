namespace TT.Report.Sections
{
    public class StrokeStatsHeadingSection : IReportSection
    {
        public string StrokeName { get; private set; }

        public StrokeStatsHeadingSection(string strokeName)
        {
            this.StrokeName = strokeName;
        }
    }
}