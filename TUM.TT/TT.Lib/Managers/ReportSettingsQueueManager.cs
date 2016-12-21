using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TT.Report.Generators;

namespace TT.Lib.Managers
{
    public class ReportGeneratedEventArgs
    {
        public Report.Report Report { get; private set; }
        public ReportGeneratedEventArgs(Report.Report report)
        {
            Report = report;
        }
    }    

    public class ReportSettingsQueueManager : IReportSettingsQueueManager
    {
        public event EventHandler<ReportGeneratedEventArgs> ReportGenerated;

        private IMatchManager matchManager;
        private List<IReportGenerator> queue;

        private object thisLock;

        public ReportSettingsQueueManager(IMatchManager matchManager)
        {
            this.matchManager = matchManager;
            this.queue = new List<IReportGenerator>(2);
            this.thisLock = new object();
        }

        public void Enqueue(IReportGenerator reportGenerator)
        {
            lock (thisLock)
            {
                if (queue.Count == 0)
                {
                    queue.Add(reportGenerator);
                    new Thread(new ThreadStart(GenerateReport)).Start();
                }
                else
                {
                    queue[1] = reportGenerator;
                }
            }
        }

        private void GenerateReport()
        {
            Report.Report report = queue[0].GenerateReport(matchManager.Match);
            lock (thisLock)
            {
                if (queue.Count == 1)
                {
                    queue.Clear();
                    ReportGenerated(this, new ReportGeneratedEventArgs(report));
                } else
                {
                    queue[0] = queue[1];
                    queue[1] = null;
                    new Thread(new ThreadStart(GenerateReport)).Start();                
                }
            }
        }
    }
}
