using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;
using TT.Models.Util.Enums;

namespace TT.Report.Sections
{
    public class SectionEmptyWarningSection : BaseSection
    {
        protected override string SectionName => "Section Empty Warning section";
        public Player Player { get; }

        public SectionEmptyWarningSection(Player player)
        {
            Player = player;
        }
    }
}
