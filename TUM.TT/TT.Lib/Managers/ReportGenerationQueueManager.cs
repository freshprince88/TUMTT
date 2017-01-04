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
        public string ReportPath { get; private set; }
        public string MatchHash { get; private set; }
        public string ReportSettingsCode { get; private set; }
        
        public ReportGeneratedEventArgs(string reportPath, string matchHash, string reportSettingsCode)
        {
            ReportPath = reportPath;
            MatchHash = matchHash;
            ReportSettingsCode = reportSettingsCode;
        }
    }

    public class ReportGenerationQueueManager : IReportGenerationQueueManager
    {
        public event EventHandler<ReportGeneratedEventArgs> ReportGenerated;

        private IMatchManager matchManager;
        private QueueWorker queueWorker;

        public ReportGenerationQueueManager(IMatchManager matchManager)
        {
            this.matchManager = matchManager;
            this.queueWorker = new QueueWorker(this);
            Start();
        }

        public void Enqueue(IReportGenerator reportGenerator)
        {
            this.queueWorker.AddReportGenerator(reportGenerator);
        }

        public void Start()
        {
            if (!queueWorker.run)
            {
                queueWorker.run = true;
                Thread workerThread = new Thread(new ThreadStart(this.queueWorker.WorkTheQueue));
                workerThread.SetApartmentState(ApartmentState.STA);
                workerThread.Start();
            }
        }

        public void Stop()
        {
            queueWorker.run = false;
        }

        private class QueueWorker
        {
            internal List<IReportGenerator> workList;
            internal ReportGenerationQueueManager man;
            internal bool run;

            internal QueueWorker(ReportGenerationQueueManager man)
            {
                this.man = man;
                this.workList = new List<IReportGenerator>();
            }

            internal void AddReportGenerator(IReportGenerator repGen)
            {
                lock(man)
                {
                    Debug.WriteLine("*QueueWorker: adding repGen={0} to queue", repGen);
                    workList.Add(repGen);
                }
            }

            internal void WorkTheQueue()
            {
                while (run)
                {
                    if (workList.Count != 0)
                    {
                        Debug.WriteLine("QueueWorker: queue not empty, starting report generation (queue.Count={0})", workList.Count);
                        var repGen = workList[workList.Count - 1];
                        var repGenCustomizationId = (repGen is CustomizedReportGenerator ? ((CustomizedReportGenerator)repGen).CustomizationId : "1");
                        var matchHash = MatchHashGenerator.GenerateMatchHash(man.matchManager.Match);

                        Report.Report report = repGen.GenerateReport(man.matchManager.Match);
                        var renderer = IoC.Get<IReportRenderer>("PDF");
                        string tmpReportPath = Path.GetTempPath() + "ttviewer_" + (matchHash + repGenCustomizationId) + ".pdf";
                        bool fileExists = File.Exists(tmpReportPath);
                        Debug.WriteLine("QueueWorker: report file (exists? {0}): {1}", fileExists, tmpReportPath);
                        if (!fileExists)
                        {
                            using (var sink = File.Create(tmpReportPath))
                            {
                                report.RenderToStream(renderer, sink);
                            }
                        }

                        lock(man)
                        {
                            if (workList.Count > 1)
                            {
                                Debug.WriteLine("QueueWorker: queue.Count={0}, reducing to last", workList.Count);
                                var lastRg = workList[workList.Count - 1];
                                workList.Clear();
                                workList.Add(lastRg);
                            }
                            else
                            {
                                Debug.WriteLine("QueueWorker: queue not grown since, invoking ReportGenerated event");
                                man.ReportGenerated(man, new ReportGeneratedEventArgs(tmpReportPath, matchHash, repGenCustomizationId));
                                workList.Clear();
                            }
                        }
                    }
                    Thread.Sleep(50);
                }
            }
        }
    }
}
