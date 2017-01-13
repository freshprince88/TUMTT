using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using TT.Lib.Properties;
using TT.Lib.Util;
using TT.Report.Generators;
using TT.Report.Renderers;
using static TT.Report.Generators.CustomizedReportGenerator;

namespace TT.Lib.Managers
{
    public class ReportGeneratedEventArgs
    {
        public string ReportPathTemp { get; private set; }
        public string MatchHash { get; private set; }
        public string ReportSettingsCode { get; private set; }
        
        public ReportGeneratedEventArgs(string reportPathTemp, string matchHash, string reportSettingsCode)
        {
            ReportPathTemp = reportPathTemp;
            MatchHash = matchHash;
            ReportSettingsCode = reportSettingsCode;
        }
    }

    public class ReportGenerationQueueManager : IReportGenerationQueueManager
    {
        public event EventHandler<ReportGeneratedEventArgs> ReportGenerated;

        private string ReportPathUser { get; set; }

        private readonly IMatchManager _matchManager;
        private readonly QueueWorker _queueWorker;
        private NotifyIcon _repGenNotification;

        public ReportGenerationQueueManager(IMatchManager matchManager)
        {
            _repGenNotification = new NotifyIcon()
            {
                Icon = Resources.olive_letter_v_512,
                Text = Resources.notification_generating
            };
            _matchManager = matchManager;
            _queueWorker = new QueueWorker(this);
            Start();
        }

        public void Enqueue(IReportGenerator reportGenerator)
        {
            _queueWorker.AddReportGenerator(reportGenerator);
        }

        public void SetReportUserPath(string userPath)
        {
            ReportPathUser = userPath;
            if (_queueWorker._renderedReportPath != null)
                _queueWorker.CopyRenderedReportToUserPath();
        }

        public void Start()
        {
            if (!_queueWorker.Run)
            {
                _queueWorker.Run = true;
                Thread workerThread = new Thread(_queueWorker.WorkTheQueue);
                workerThread.SetApartmentState(ApartmentState.STA);
                workerThread.Start();
            }
        }

        public void Stop()
        {
            _queueWorker.Run = false;
        }

        private class QueueWorker
        {
            private readonly List<IReportGenerator> _workList;
            private readonly ReportGenerationQueueManager _man;
            private CustomizedReportGenerator _custRepGen;
            internal string _renderedReportPath;
            internal bool Run;
            private static int _genId;

            internal QueueWorker(ReportGenerationQueueManager man)
            {
                _man = man;
                _workList = new List<IReportGenerator>();
            }

            internal void AddReportGenerator(IReportGenerator repGen)
            {
                lock(_man)
                {
                    Debug.WriteLine("*QueueWorker: adding repGen (Hash={0}) to queue", repGen.GetHashCode());
                    _workList.Add(repGen);
                }
            }

            internal void WorkTheQueue()
            {
                while (Run)
                {
                    if (_workList.Count != 0)
                    {
                        Debug.WriteLine("QueueWorker: queue not empty, starting report generation (queue.Count={0})", _workList.Count);

                        IReportGenerator repGen;
                        lock (_man)
                        {
                            repGen = _workList[_workList.Count - 1];
                            _workList.Clear();
                        }

                        if (_custRepGen != null)
                        {
                            _renderedReportPath = null;
                            _custRepGen.Abort = true;
                            _custRepGen.SectionsAdded -= CustomizedReportGenerator_SectionsAdded;
                            _custRepGen = null;
                        }

                        var gen = repGen as CustomizedReportGenerator;
                        if (gen != null)
                        {
                            _custRepGen = gen;
                            _custRepGen.SectionsAdded += CustomizedReportGenerator_SectionsAdded;
                            var custRepGenThread = new Thread(_custRepGen.GenerateReport)
                            {
                                Name = "CustomReportGenThread-" + _genId++
                            };
                            custRepGenThread.SetApartmentState(ApartmentState.STA);
                            Debug.WriteLine("QueueWorker: starting {0}", args: custRepGenThread.Name);
                            custRepGenThread.Start();

                            if (_man._repGenNotification != null)
                                _man._repGenNotification.Visible = true;
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
                var matchHash = MatchHashGenerator.GenerateMatchHash(_man._matchManager.Match);
                string tmpReportPath;
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
                    _renderedReportPath = tmpReportPath;
                    _man.ReportGenerated?.Invoke(_man, new ReportGeneratedEventArgs(tmpReportPath, matchHash, repGenCustomizationId));

                    if (_man.ReportPathUser != null)
                        CopyRenderedReportToUserPath();
                }
                else
                {
                    Debug.WriteLine("QueueWorker: repGen={0} aborted, discarding.", repGen.GetHashCode());
                }
            }

            private void _repGenNotification_BalloonTipClosed(object sender, EventArgs e)
            {
                _man._repGenNotification.Dispose();
            }

            internal void CopyRenderedReportToUserPath()
            {
                Debug.WriteLine($"userChosenPath={_man.ReportPathUser}");
                try
                {
                    File.Copy(_renderedReportPath, _man.ReportPathUser, true);
                }
                catch (Exception)
                {
                    // TODO alert when opened in another process 
                }

                _man._repGenNotification.BalloonTipText = "Report has been generated. Click here to open it.";
                _man._repGenNotification.BalloonTipTitle = "Report generation finished!";
                //_repGenNotification.BalloonTipClicked += (s, e) =>
                //{
                //    Process.Start(_man.ReportPathUser);
                //    _repGenNotification.Visible = false;
                //    _repGenNotification.Dispose();
                //    _repGenNotification = null;
                //};
                //_repGenNotification.BalloonTipClosed += _repGenNotification_BalloonTipClosed;
                _man._repGenNotification.ShowBalloonTip(30000);

                _man.ReportPathUser = null;
            }
        }
    }
}
