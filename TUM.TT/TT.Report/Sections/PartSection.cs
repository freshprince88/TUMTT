using TT.Models;

namespace TT.Report.Sections
{
    public class PartSection : IReportSection
    {
        public enum PartType { General, Player, Appendix}
        public PartType Type { get; private set; }
        public string PartName { get; private set; }
        public Player Player { get; set; }

        public PartSection(string partName, PartType partType)
        {
            PartName = partName;
            Type = partType;
        }
    }
}