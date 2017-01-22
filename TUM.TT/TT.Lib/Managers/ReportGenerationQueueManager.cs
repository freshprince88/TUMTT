using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TT.Lib.Properties;
using TT.Lib.Util;
using TT.Report.Generators;
using TT.Report.Renderers;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib.Views;
using static TT.Report.Generators.CustomizedReportGenerator;
using TempFileScheme = TT.Models.Util.TempFileScheme;
using TempFileType = TT.Models.Util.TempFileType;

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

        public string ReportPathUser { get; set; }
        
        public TempFileScheme TempFileScheme
        {
            get
            {
                TempFileScheme tfs;
                tfs.TempPath = Path.GetTempPath();
                tfs.NameScheme = "ttviewer_{0}.pdf";
                tfs.Type = TempFileType.ReportPreview;
                return tfs;
            }
        }

        private readonly IMatchManager _matchManager;
        private readonly QueueWorker _queueWorker;
        private readonly AsyncOperation _asyncOp;
        private ReportGenerationNotifyIcon _notifyIcon;

        public ReportGenerationQueueManager(IMatchManager matchManager)
        {
            _matchManager = matchManager;

            Debug.WriteLine($"ReportGenerationQueueManager: creating ReportGenerationNotifyIcon (Thread '{Thread.CurrentThread.Name}')");
            _notifyIcon = new ReportGenerationNotifyIcon();

            // AsyncOperation being created in the constructor gives us a way of letting code run on the UI (main) thread,
            // which is needed for NotifyIcon (& its BalloonTip) to generate click events
            _asyncOp = AsyncOperationManager.CreateOperation(null);
            _queueWorker = new QueueWorker(this);
            Start();
        }

        public void Enqueue(IReportGenerator reportGenerator)
        {
            _queueWorker.AddReportGenerator(reportGenerator);
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

        public void Stop(bool hideNotifyIcon, bool runOnce)
        {
            _queueWorker.RunOnce = runOnce;
            if (hideNotifyIcon)
                _notifyIcon.Visible = false;
        }

        public void Dispose()
        {
            _queueWorker.Run = false;
            Debug.WriteLine($"ReportGenerationQueueManager: disposing of NotifyIcon (Thread '{Thread.CurrentThread.Name}')");
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private class QueueWorker
        {
            private static readonly object WorkListLock = new object();

            private readonly List<IReportGenerator> _workList;
            private readonly ReportGenerationQueueManager _man;
            private CustomizedReportGenerator _custRepGen;
            private string _renderedReportPath;
            internal bool Run;
            internal bool RunOnce;
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
                    var gen = repGen as CustomizedReportGenerator;
                    if (gen == null)
                        throw new NotImplementedException("Queueing of non-customizable report generators not implemented");

                    if (_workList.Contains(repGen) || (_custRepGen != null && !_custRepGen.Done && string.Equals(gen.CustomizationId, _custRepGen.CustomizationId)))
                    {
                        Debug.WriteLine("*QueueWorker: NOT adding repGen (Hash={0}) to queue - already in queue/work", repGen.GetHashCode());
                        return;
                    }

                    Debug.WriteLine("*QueueWorker: adding repGen (Hash={0}) to queue", repGen.GetHashCode());
                    _workList.Add(repGen);
                }
            }

            internal void WorkTheQueue()
            {
                while (Run || RunOnce || _man._notifyIcon != null && _man._notifyIcon.Visible)
                {
                    if (_workList.Count != 0)
                    {
                        Debug.WriteLine("QueueWorker: queue not empty, starting report generation (queue.Count={0})", _workList.Count);

                        IReportGenerator repGen;
                        lock (WorkListLock)
                        {
                            repGen = _workList[_workList.Count - 1];
                            _workList.Clear();
                        }

                        _renderedReportPath = null;

                        if (_custRepGen != null)
                        {
                            _custRepGen.Abort = true;
                            _custRepGen.SectionsAdded -= CustomizedReportGenerator_SectionsAdded;
                            _custRepGen = null;
                        }

                        var gen = repGen as CustomizedReportGenerator;
                        if (gen != null)
                        {
                            var matchHash = MatchHashGenerator.GenerateMatchHash(_man._matchManager.Match);
                            var tmpFileName = string.Format(_man.TempFileScheme.NameScheme, matchHash + gen.CustomizationId);
                            var tmpReportPath = _man.TempFileScheme.TempPath + tmpFileName;
                            if (File.Exists(tmpReportPath))
                            {
                                Debug.WriteLine($"QueueWorker: tmp file '{tmpFileName}' already exists, skipping generation and invoking ReportGenerated event (Thread '{Thread.CurrentThread.Name}')");
                                _renderedReportPath = tmpReportPath;
                                _man.ReportGenerated?.Invoke(_man, new ReportGeneratedEventArgs(tmpReportPath, matchHash, gen.CustomizationId));
                            }
                            else
                            {
                                if (gen.ShowNotification)
                                    MakeNotifyIconVisible();

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

                    CopyRenderedReportToUserPath();

                    if (RunOnce)
                        RunOnce = false;
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
                    tmpReportPath = _man.TempFileScheme.TempPath + string.Format(_man.TempFileScheme.NameScheme, matchHash + repGenCustomizationId);
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
                    repGen.Done = true;
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
                if (_man.ReportPathUser != null)
                {
                    MakeNotifyIconVisible();

                    if (_renderedReportPath != null)
                    {
                        Debug.WriteLine($"QueueWorker: userChosenPath={_man.ReportPathUser} (Thread '{Thread.CurrentThread.Name}')");
                        try
                        {
                            File.Copy(_renderedReportPath, _man.ReportPathUser, true);

                            var reportPathUser = _man.ReportPathUser;
                            NotifyUser(reportPathUser);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"QueueWorker: {ex.GetType().Name} ({ex.Message}) (Thread '{Thread.CurrentThread.Name}')");
                            if (ShowFileInUseDialog(_man.ReportPathUser).Result == MessageDialogResult.Affirmative)
                                return;
                        }
                        _man.ReportPathUser = null;
                        _man._notifyIcon.Visible = false;
                    }
                }
            }

            private static Task<MessageDialogResult> ShowFileInUseDialog(string reportPathUser)
            {
                var dialogSettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = Resources.dialog_file_in_use_button_ok,
                    NegativeButtonText = Resources.dialog_file_in_use_button_cancel,
                    AnimateShow = true,
                    AnimateHide = false
                };
                var coordinator = IoC.Get<IDialogCoordinator>();
                return coordinator.ShowMessageAsync(IoC.Get<IShell>(), Resources.dialog_file_in_use_title, 
                    string.Format(Resources.dialog_file_in_use_message, reportPathUser),
                    MessageDialogStyle.AffirmativeAndNegative, dialogSettings);
            }

            private void MakeNotifyIconVisible()
            {
                if (_man._notifyIcon != null && !_man._notifyIcon.Visible)
                {
                    _man._asyncOp.Post(o =>
                    {
                        // this has to be done on the UI thread
                        Debug.WriteLine($"QueueWorker: making NotifyIcon visible (Thread '{Thread.CurrentThread.Name}')");
                        _man._notifyIcon.Animate();
                    }, EventArgs.Empty);
                }
            }

            private void NotifyUser(string reportPathUser)
            {
                _man._asyncOp.Post(o =>
                {
                    // this has to be done on the UI thread
                    Debug.WriteLine($"QueueWorker: setting tag and text of NotifyIcon.BalloonTip and showing it (Thread '{Thread.CurrentThread.Name}')");
                    _man._notifyIcon.Tag = reportPathUser;
                    _man._notifyIcon.StopAnimating();

                    /* Use the first line to show a balloontip XOR the second for a toast - they look the same,
                       the toast _could_ however go to the action center, if there is shortcut for the app
                       in the start menu.
                       See http://stackoverflow.com/questions/36165102/toast-notifications-does-not-appear-in-action-center-after-time-out
                       and https://msdn.microsoft.com/en-us/library/windows/apps/hh802762.aspx
                       and https://msdn.microsoft.com/en-us/library/windows/apps/hh802768.aspx
                       (If for some reason the necessary usings & references for toasts were deleted from this project, 
                       see: http://stackoverflow.com/questions/12745703/how-can-i-use-the-windows-ui-namespace-from-a-regular-non-store-win32-net-app
                    */
                    _man._notifyIcon.ShowBaloonTip();
                    //ShowToast();
                }, EventArgs.Empty);
            }

            // ReSharper disable once UnusedMember.Local
            private void ShowToast()
            {
                string toastVisual =
                    $@"<visual>
                      <binding template='ToastGeneric'>
                        <text>{"Report generation finished"}</text>
                        <text>{"Click here to open the newly generated PDF with the Report."}</text>
                      </binding>
                    </visual>";

                string toastActions =
                    $@"<actions> 
                          <action
                              content='Open'
                              arguments='{"action=open"}'
                              activationType='background'
                              hint-inputId='tbReply'/>
 
                    </actions>";

                string toastXmlString =
                    $@"<toast launch='{"action=open"}'>
                        {toastVisual}
                        {toastActions}
                    </toast>";

                // Parse to XML
                var toastXml = new XmlDocument();
                toastXml.LoadXml(toastXmlString);
                
                // Generate toast
                var toast = new ToastNotification(toastXml);
                ToastNotificationManager.CreateToastNotifier("ttlib").Show(toast);
            }
        }
    }
}
