using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
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
        private readonly AsyncOperation _asyncOp;

        public ReportGenerationQueueManager(IMatchManager matchManager)
        {
            Debug.WriteLine($"ReportGenerationQueueManager: creating NotifyIcon (Thread '{Thread.CurrentThread.Name}')");
            _repGenNotification = new NotifyIcon()
            {
                Icon = Resources.olive_letter_v_512,
                Text = Resources.notification_generating
            };
            EventHandler dClickOrBalloonClick = (sender, args) =>
            {
                Debug.WriteLine($"QueueWorker (double-click on NotifyIcon or single-click on BallonTip): opening path '{((NotifyIcon)sender).Tag}' and hiding NotifyIcon (Thread '{Thread.CurrentThread.Name}')");
                Process.Start((string)((NotifyIcon)sender).Tag);
                _repGenNotification.Visible = false;
            };
            _repGenNotification.DoubleClick += dClickOrBalloonClick;

            _repGenNotification.BalloonTipText = Resources.notification_generated_text;
            _repGenNotification.BalloonTipTitle = Resources.notification_generated_title;
            _repGenNotification.BalloonTipClicked += dClickOrBalloonClick;

            _matchManager = matchManager;
            _asyncOp = AsyncOperationManager.CreateOperation(null);
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
        }

        public void Start()
        {
            if (!_queueWorker.Run)
            {
                _queueWorker.Run = true;
                Thread workerThread = new Thread(_queueWorker.WorkTheQueue)
                {
                    Name = "QueueWorkerThread"
                };
                workerThread.SetApartmentState(ApartmentState.STA);
                workerThread.Start();
            }
        }

        public void Stop()
        {
            _queueWorker.Run = false;
        }

        public void Dispose()
        {
            _queueWorker.Run = false;
            Debug.WriteLine($"ReportGenerationQueueManager: disposing of NotifyIcon (Thread '{Thread.CurrentThread.Name}')");
            _repGenNotification.Visible = false;
            _repGenNotification.Dispose();
            _repGenNotification = null;
        }

        private class QueueWorker
        {
            private readonly List<IReportGenerator> _workList;
            private readonly ReportGenerationQueueManager _man;
            private CustomizedReportGenerator _custRepGen;
            private string _renderedReportPath;
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
                while (Run || _man._repGenNotification != null && _man._repGenNotification.Visible)
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
                            if (_man._repGenNotification != null)
                            {
                                _man._asyncOp.Post(o =>
                                {
                                    Debug.WriteLine($"QueueWorker: making NotifyIcon visible (Thread '{Thread.CurrentThread.Name}')");
                                    _man._repGenNotification.Text = Resources.notification_generating;
                                    _man._repGenNotification.Visible = true;
                                }, EventArgs.Empty);
                            }

                            var matchHash = MatchHashGenerator.GenerateMatchHash(_man._matchManager.Match);
                            var tmpFileName = "ttviewer_" + (matchHash + gen.CustomizationId) + ".pdf";
                            var tmpReportPath = Path.GetTempPath() + tmpFileName;
                            if (File.Exists(tmpReportPath))
                            {
                                Debug.WriteLine($"QueueWorker: tmp file '{tmpFileName}' already exists, skipping generation and invoking ReportGenerated event (Thread '{Thread.CurrentThread.Name}')");
                                _renderedReportPath = tmpReportPath;
                                _man.ReportGenerated?.Invoke(_man, new ReportGeneratedEventArgs(tmpReportPath, matchHash, gen.CustomizationId));
                            }
                            else
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
                            }
                        }
                        else
                        {
                            // TODO Generate report with non-customizable report generator
                            // repGen.GenerateReport(man.matchManager.Match);
                        }
                    }
                    if (_man.ReportPathUser != null)
                        CopyRenderedReportToUserPath();

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
                }
                else
                {
                    Debug.WriteLine("QueueWorker: repGen={0} aborted, discarding.", repGen.GetHashCode());
                }
            }

            private void CopyRenderedReportToUserPath()
            {
                if (_renderedReportPath != null)
                {
                    Debug.WriteLine($"QueueWorker: userChosenPath={_man.ReportPathUser} (Thread '{Thread.CurrentThread.Name}')");
                    try
                    {
                        File.Copy(_renderedReportPath, _man.ReportPathUser, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"QueueWorker: {ex.GetType().Name} ({ex.Message}) (Thread '{Thread.CurrentThread.Name}')");
                        // TODO alert when opened in another process 
                    }

                    var reportPathUser = _man.ReportPathUser;
                    _man._asyncOp.Post(o =>
                    {
                        Debug.WriteLine($"QueueWorker: setting tag and text of NotifyIcon.BalloonTip (Thread '{Thread.CurrentThread.Name}')");
                        _man._repGenNotification.Tag = reportPathUser;
                        _man._repGenNotification.Text = Resources.notification_generated_doubleclick;

                        Debug.WriteLine($"QueueWorker: repGenNotification.Visible={_man._repGenNotification.Visible} (Thread '{Thread.CurrentThread.Name}')");
                        _man._repGenNotification.ShowBalloonTip(30000);
                    }, EventArgs.Empty);

                    _man.ReportPathUser = null;
                }
            }
        }
    }
}
