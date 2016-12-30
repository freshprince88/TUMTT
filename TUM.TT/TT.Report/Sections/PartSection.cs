using TT.Models;

namespace TT.Report.Sections
{
    public class PartSection : IReportSection
    {
        public string PartName { get; private set; }
        public Player Player { get; private set; }

        public PartSection(string partName)
        {
            this.PartName = partName;
        }

        public PartSection(string partName, Player player) : this(partName)
        {
            this.Player = player;
        }
    }
}