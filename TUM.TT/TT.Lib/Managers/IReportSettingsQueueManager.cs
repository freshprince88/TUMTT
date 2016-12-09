﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Report.Generators;

namespace TT.Lib.Managers
{
    public interface IReportSettingsQueueManager
    {
        void Enqueue(IReportGenerator reportGenerator);
        event EventHandler<ReportGeneratedEventArgs> ReportGenerated;
    }
}