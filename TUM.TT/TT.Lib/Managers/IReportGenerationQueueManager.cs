﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Util;
using TT.Report.Generators;

namespace TT.Lib.Managers
{
    public interface IReportGenerationQueueManager
    {
        void Enqueue(IReportGenerator reportGenerator);
        void Start();
        void Stop(bool hideNotifyIcon, bool runOnce);
        void Dispose();
        event EventHandler<ReportGeneratedEventArgs> ReportGenerated;
        string ReportPathUser { get; set; }
        TempFileScheme TempFileScheme { get; }
    }
}
