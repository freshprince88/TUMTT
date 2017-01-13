using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Report.Generators;

namespace TT.Lib.Managers
{
    public interface IReportGenerationQueueManager
    {
        void Enqueue(IReportGenerator reportGenerator);
        void SetReportUserPath(string userPath);
        void Start();
        void Stop();
        void Dispose();
        event EventHandler<ReportGeneratedEventArgs> ReportGenerated;
    }
}
