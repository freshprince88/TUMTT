using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using TT.Lib.Util;
using TT.Report.Generators;
using TT.Report.Renderers;

namespace TT.Lib.Managers
{
    public class ReportGeneratedEventArgs
    {
        public string Report { get; private set; }
        public string MatchHash { get; private set; }
        public string ReportSettingsCode { get; private set; }

        public ReportGeneratedEventArgs(string reportPath) : this(reportPath, MatchHashGenerator.GenerateMatchHash(null), "1")
        {
        }

        public ReportGeneratedEventArgs(string reportPath, string matchHash, string reportSettingsCode)
        {
            Report = reportPath;
            MatchHash = matchHash;
            ReportSettingsCode = reportSettingsCode;
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
            var enqueuing = new ReportGeneratorEnqueuing(this);
            enqueuing.reportGenerator = reportGenerator;
            new Thread(new ThreadStart(enqueuing.EnqueueReportGenerator)).Start();
        }

        private void GenerateReport()
        {
            Report.Report report = queue[0].GenerateReport(matchManager.Match);

            var renderer = IoC.Get<IReportRenderer>("PDF");
            string tmpReportPath = Path.GetTempPath() + "ttviewer_" + (MatchHashGenerator.GenerateMatchHash(null) + "1") + ".pdf";
            bool fileExists = File.Exists(tmpReportPath);
            Debug.WriteLine("report file (exists? {0}): {1}", fileExists, tmpReportPath);
            if (!fileExists)
            {
                using (var sink = File.Create(tmpReportPath))
                {
                    report.RenderToStream(renderer, sink);
                }
            }

            lock (thisLock)
            {
                if (queue.Count == 1)
                {
                    queue.Clear();
                    ReportGenerated(this, new ReportGeneratedEventArgs(tmpReportPath));
                } else
                {
                    queue[0] = queue[1];
                    queue[1] = null;
                    new Thread(new ThreadStart(GenerateReport)).Start();                
                }
            }
        }

        private class ReportGeneratorEnqueuing
        {
            internal IReportGenerator reportGenerator;
            private ReportSettingsQueueManager man;

            internal ReportGeneratorEnqueuing(ReportSettingsQueueManager manager)
            {
                man = manager;
            }

            internal void EnqueueReportGenerator()
            {
                lock (man.thisLock)
                {
                    if (man.queue.Count == 0)
                    {
                        man.queue.Add(reportGenerator);
                        new Thread(new ThreadStart(man.GenerateReport)).Start();
                    }
                    else
                    {
                        man.queue[1] = reportGenerator;
                    }
                }
            }
        }
    }
}
