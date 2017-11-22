using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{
    public class ReportPreviewChangedEvent
    {
        public string ReportPreviewPath { get; set; }
        public string CustomizationId { get; set; }

        public ReportPreviewChangedEvent(string reportPreviewPath, string customizationId)
        {
            ReportPreviewPath = reportPreviewPath;
            CustomizationId = customizationId;
        }
    }
}
