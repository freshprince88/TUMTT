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
        private readonly IWindowManager WindowManager;
        private IDialogCoordinator DialogCoordinator;
        public IMatchManager MatchManager { get; private set; }
        public IEventAggregator Events { get; private set; }
        public IReportSettingsQueueManager ReportSettingsQueueManager { get; private set; }
        
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

        private int setChoice;
        public int SetChoice
        {
            get
            {
                return setChoice;
            }
            set
            {
                if ((setChoice & value) == value)
                    setChoice -= value;
                else
                    setChoice += value;
                Debug.WriteLine("set choice: {0}", setChoice);
                NotifyOfPropertyChange();
            }
        }
        
        public List<int> AvailableCombis{ get; set; }        
        public List<int> SelectedCombis{ get; set; }

        public ReportSettingsViewModel(IMatchManager matchManager, IReportSettingsQueueManager reportSettingsQueueManager, IWindowManager windowManager, IEventAggregator events, IDialogCoordinator dialogCoordinator)
        {
            MatchManager = matchManager;
            ReportSettingsQueueManager = reportSettingsQueueManager;
            Events = events;
            WindowManager = windowManager;
            DialogCoordinator = dialogCoordinator;

            DisplayName = "Report Settings";
            AvailableCombis = new List<int>();
            SelectedCombis = new List<int>();

            PropertyChanged += ReportSettingsViewModel_PropertyChanged;
            ReportSettingsQueueManager.ReportGenerated += ReportSettingsQueueManager_ReportGenerated;

            Load();
        }

        private void ReportSettingsQueueManager_ReportGenerated(object sender, ReportGeneratedEventArgs e)
        {
            Debug.WriteLine("report generated [sender={0} report={1}]", sender, e.Report);

            var renderer = IoC.Get<IReportRenderer>("PDF");
            string tmpReportPath = Path.GetTempPath() + "ttviewer_" + (e.MatchHash + e.ReportSettingsCode) + ".pdf";
            bool fileExists = File.Exists(tmpReportPath);
            Debug.WriteLine("report file (exists? {0}): {1}", fileExists, tmpReportPath);
            if (!fileExists)
            {
                using (var sink = File.Create(tmpReportPath))
                {
                    e.Report.RenderToStream(renderer, sink);
                }
            }
            Events.PublishOnUIThread(new ReportPreviewChangedEvent(tmpReportPath));
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

        public void AddCombi(int newCombi)
        {
            Debug.WriteLine("adding combi: {0}", newCombi);
            if (!AvailableCombis.Contains(newCombi))
                AvailableCombis.Add(newCombi);
        }

        public void SelectCombi(int combi, bool select)
        {
            Debug.WriteLine("selecting combi: {0} ({1})", combi, select);
            if (select)
            {
                if (!SelectedCombis.Contains(combi))
                    SelectedCombis.Add(combi);
            }
            else
                SelectedCombis.Remove(combi);
        }

        private void Save()
        {
            Properties.Settings.Default.ReportGenerator_Playerchoice = PlayerChoice;
            Properties.Settings.Default.ReportGenerator_Setchoice = SetChoice;
            Properties.Settings.Default.ReportGenerator_Combis = SelectedCombis.ToArray();
        }

        private void Load()
        {
            PlayerChoice = Properties.Settings.Default.ReportGenerator_Playerchoice;
            SetChoice = Properties.Settings.Default.ReportGenerator_Setchoice;

            var combis = Properties.Settings.Default.ReportGenerator_Combis;
            if (combis != null) AvailableCombis.AddRange(combis);
            combis = Properties.Settings.Default.ReportGenerator_Combis;
            if (combis != null) SelectedCombis.AddRange(combis);
        }
    }
}
