using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using TT.Lib.Util;
using TT.Report.Generators;
using TT.Report.Renderers;
using static TT.Report.Generators.CustomizedReportGenerator;

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
            internal CustomizedReportGenerator custRepGen;
            internal bool run;
            internal static int genId;

            internal QueueWorker(ReportGenerationQueueManager man)
            {
                this.man = man;
                this.workList = new List<IReportGenerator>();
            }

            internal void AddReportGenerator(IReportGenerator repGen)
            {
                lock(man)
                {
                    Debug.WriteLine("*QueueWorker: adding repGen (Hash={0}) to queue", repGen.GetHashCode());
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

                        IReportGenerator repGen;
                        lock (man)
                        {
                            repGen = workList[workList.Count - 1];
                            workList.Clear();
                        }

                        if (custRepGen != null)
                        {
                            custRepGen.Abort = true;
                            custRepGen.SectionsAdded -= CustomizedReportGenerator_SectionsAdded;
                            custRepGen = null;
                        }

                        if (repGen is CustomizedReportGenerator)
                        {
                            custRepGen = (CustomizedReportGenerator)repGen;
                            custRepGen.SectionsAdded += CustomizedReportGenerator_SectionsAdded;
                            var custRepGenThread = new Thread(new ThreadStart(custRepGen.GenerateReport));
                            custRepGenThread.Name = "CustomReportGenThread-" + genId++;
                            custRepGenThread.SetApartmentState(ApartmentState.STA);
                            Debug.WriteLine("QueueWorker: starting {0}", args: custRepGenThread.Name);
                            custRepGenThread.Start();
                        }
                        else
                        {
                            // TODO Generate report with non-customizable report generator
                            // repGen.GenerateReport(man.matchManager.Match);
                        }
                    }
                    Thread.Sleep(500);
                }
            }

            private void CustomizedReportGenerator_SectionsAdded(object sender, SectionsAddedEventArgs e)
            {
                Debug.WriteLine("QueueWorker [EVENT]: sections added [sender={0} report={1}]", sender, e.Report);

                CustomizedReportGenerator repGen = (CustomizedReportGenerator)sender;
                var repGenCustomizationId = repGen.CustomizationId;
                var matchHash = MatchHashGenerator.GenerateMatchHash(man.matchManager.Match);
                string tmpReportPath = null;
                if (!repGen.Abort)
                {
                    var renderer = IoC.Get<IReportRenderer>("PDF");
                    tmpReportPath = Path.GetTempPath() + "ttviewer_" + (matchHash + repGenCustomizationId) + ".pdf";
                    bool fileExists = File.Exists(tmpReportPath);
                    Debug.WriteLine("QueueWorker: report file (exists? {0}): {1}", fileExists, tmpReportPath);
                    if (!fileExists)
                    {
                        using (var sink = File.Create(tmpReportPath))
                        {
                            e.Report.RenderToStream(renderer, sink);
                        }
                    }
                }
                else
                    return;

                if (!repGen.Abort)
                {
                    Debug.WriteLine("QueueWorker: repGen={0} not aborted, invoking ReportGenerated event", repGen.GetHashCode());
                    man.ReportGenerated?.Invoke(man, new ReportGeneratedEventArgs(tmpReportPath, matchHash, repGenCustomizationId));
                }
                else
                {
                    Debug.WriteLine("QueueWorker: repGen={0} aborted, discarding.", repGen.GetHashCode());
                }
            }            
        }
    }
}
