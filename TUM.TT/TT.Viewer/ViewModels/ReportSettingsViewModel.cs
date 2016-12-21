using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib;
using TT.Lib.Util;
using System.Diagnostics;
using TT.Lib.Managers;
using TT.Lib.Results;
using TT.Report.Renderers;
using System.IO;
using System.Threading;
using TT.Lib.Events;

namespace TT.Viewer.ViewModels
{
    public class ReportSettingsViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx
    {
        private static readonly string PreviewReportPath = "pack://siteoforigin:,,,/Resources/report_tmp.pdf#toolbar=0&amp;navpanes=0";

        public IEventAggregator Events { get; private set; }
        private readonly IWindowManager WindowManager;
        private IDialogCoordinator DialogCoordinator;
        public IMatchManager MatchManager { get; set; }
        public IReportSettingsQueueManager ReportSettingsQueueManager { get; set; }

        private string previewReport;
        public string PreviewReport
        {
            get
            {
                return previewReport;
            }
            set
            {
                previewReport = value;
                NotifyOfPropertyChange();
            }
        }

        private string playerChoice;
        public string PlayerChoice {
            get
            {
                return playerChoice;
            }
            set
            {
                playerChoice = value;
                NotifyOfPropertyChange();
            }
        }

        public ReportSettingsViewModel(IMatchManager matchManager, IReportSettingsQueueManager reportSettingsQueueManager, IWindowManager windowManager, IEventAggregator events, IDialogCoordinator dialogCoordinator)
        {
            this.MatchManager = matchManager;
            this.ReportSettingsQueueManager = reportSettingsQueueManager;
            this.Events = events;
            this.WindowManager = windowManager;
            this.DialogCoordinator = dialogCoordinator;
            
            DisplayName = "Report Settings";
            PropertyChanged += ReportSettingsViewModel_PropertyChanged;

            ReportSettingsQueueManager.ReportGenerated += ReportSettingsQueueManager_ReportGenerated;

            Load();

            PreviewReport = PreviewReportPath;
        }

        private void ReportSettingsQueueManager_ReportGenerated(object sender, ReportGeneratedEventArgs e)
        {
            Debug.WriteLine("report generated [sender={0} report={1}]", sender, e.Report);

            Events.PublishOnUIThread(new ReportPreviewChangedEvent(PreviewReportPath));
            //PreviewReport = null;
            //var renderer = IoC.Get<IReportRenderer>("PDF");
            //Thread.Sleep(1500);
            //using (var sink = File.Create("Resources/report_tmp.pdf"))
            //{
            //    e.Report.RenderToStream(renderer, sink);
            //    PreviewReport = PreviewReportPath;
            //}
            //PreviewReport = PreviewReportPath;
        }

        private void ReportSettingsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("property changed [sender={0} propname={1} propvalue={2}]", sender, e.PropertyName, sender.GetType().GetProperty(e.PropertyName).GetValue(sender));
        }

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name) ? Application.Current.Windows.OfType<T>().Any() : Application.Current.Windows.OfType<T>().Any(wde => wde.Name.Equals(name));
        }
        
        public void OnOkClick()
        {
            Debug.WriteLine("OnOkClick");
            Save();
            TryClose();
        }

        public void OnCancelClick()
        {
            Debug.WriteLine("OnCancelClick");
            TryClose();
        }

        public IEnumerable<IResult> OnOkGenerateClick()
        {
            Debug.WriteLine("OnOkGenerateClick");
            Save();

            //foreach (IResult result in MatchManager.GenerateReport("customized"))
            //    yield return result;
            //TryClose();

            ReportSettingsQueueManager.Enqueue(new TT.Report.Generators.CustomizedReportGenerator());
            return null;
        }

        private void Save()
        {
            Properties.Settings.Default.ReportGenerator_Playerchoice = PlayerChoice;
        }

        private void Load()
        {
            PlayerChoice = Properties.Settings.Default.ReportGenerator_Playerchoice;
        }
    }
}
