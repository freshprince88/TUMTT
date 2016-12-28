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
        private IWindowManager WindowManager;
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

        private int serviceStatsChoice;
        public int ServiceStatsChoice
        {
            get
            {
                return serviceStatsChoice;
            }
            set
            {
                if ((serviceStatsChoice & value) == value)
                    serviceStatsChoice -= value;
                else
                    serviceStatsChoice += value;
                Debug.WriteLine("service stats choice: {0}", serviceStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int returnStatsChoice;
        public int ReturnStatsChoice
        {
            get
            {
                return returnStatsChoice;
            }
            set
            {
                if ((returnStatsChoice & value) == value)
                    returnStatsChoice -= value;
                else
                    returnStatsChoice += value;
                Debug.WriteLine("return stats choice: {0}", returnStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int thirdStatsChoice;
        public int ThirdStatsChoice
        {
            get
            {
                return thirdStatsChoice;
            }
            set
            {
                if ((thirdStatsChoice & value) == value)
                    thirdStatsChoice -= value;
                else
                    thirdStatsChoice += value;
                Debug.WriteLine("third stats choice: {0}", thirdStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int fourthStatsChoice;
        public int FourthStatsChoice
        {
            get
            {
                return fourthStatsChoice;
            }
            set
            {
                if ((fourthStatsChoice & value) == value)
                    fourthStatsChoice -= value;
                else
                    fourthStatsChoice += value;
                Debug.WriteLine("fourth stats choice: {0}", fourthStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int lastStatsChoice;
        public int LastStatsChoice
        {
            get
            {
                return lastStatsChoice;
            }
            set
            {
                if ((lastStatsChoice & value) == value)
                    lastStatsChoice -= value;
                else
                    lastStatsChoice += value;
                Debug.WriteLine("last stats choice: {0}", lastStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int allStatsChoice;
        public int AllStatsChoice
        {
            get
            {
                return allStatsChoice;
            }
            set
            {
                if ((allStatsChoice & value) == value)
                    allStatsChoice -= value;
                else
                    allStatsChoice += value;
                Debug.WriteLine("all stats choice: {0}", allStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int expandState;
        public int ExpandState
        {
            get
            {
                return expandState;
            }
            set
            {
                if ((expandState & value) == value)
                    expandState -= value;
                else
                    expandState += value;
                Debug.WriteLine("expand state: {0}", allStatsChoice);
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

            DisplayName = Properties.Resources.report_settings_window_title;
            AvailableCombis = new List<int>();
            SelectedCombis = new List<int>();

            PropertyChanged += ReportSettingsViewModel_PropertyChanged;
            ReportSettingsQueueManager.ReportGenerated += ReportSettingsQueueManager_ReportGenerated;

            Load();
        }

        private void ReportSettingsQueueManager_ReportGenerated(object sender, ReportGeneratedEventArgs e)
        {
            Debug.WriteLine("report generated [this={2} sender={0} report={1}]", sender, e.Report, GetHashCode());

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
            Properties.Settings.Default.ReportGenerator_ServiceStatsChoice = ServiceStatsChoice;
            Properties.Settings.Default.ReportGenerator_ReturnStatsChoice = ReturnStatsChoice;
            Properties.Settings.Default.ReportGenerator_ThirdStatsChoice = ThirdStatsChoice;
            Properties.Settings.Default.ReportGenerator_FourthStatsChoice = FourthStatsChoice;
            Properties.Settings.Default.ReportGenerator_LastStatsChoice = LastStatsChoice;
            Properties.Settings.Default.ReportGenerator_AllStatsChoice = AllStatsChoice;
            Properties.Settings.Default.ReportGenerator_ExpandState = ExpandState;
        }

        private void Load()
        {
            PlayerChoice = Properties.Settings.Default.ReportGenerator_Playerchoice;
            SetChoice = Properties.Settings.Default.ReportGenerator_Setchoice;
            ServiceStatsChoice = Properties.Settings.Default.ReportGenerator_ServiceStatsChoice;
            ReturnStatsChoice = Properties.Settings.Default.ReportGenerator_ReturnStatsChoice;
            ThirdStatsChoice = Properties.Settings.Default.ReportGenerator_ThirdStatsChoice;
            FourthStatsChoice = Properties.Settings.Default.ReportGenerator_FourthStatsChoice;
            LastStatsChoice = Properties.Settings.Default.ReportGenerator_LastStatsChoice;
            AllStatsChoice = Properties.Settings.Default.ReportGenerator_AllStatsChoice;
            ExpandState = Properties.Settings.Default.ReportGenerator_ExpandState;

            var combis = Properties.Settings.Default.ReportGenerator_Combis;
            if (combis != null) AvailableCombis.AddRange(combis);
            combis = Properties.Settings.Default.ReportGenerator_Combis;
            if (combis != null) SelectedCombis.AddRange(combis);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            if (close)
            {
                PropertyChanged -= ReportSettingsViewModel_PropertyChanged;
                ReportSettingsQueueManager.ReportGenerated -= ReportSettingsQueueManager_ReportGenerated;
                Events.Unsubscribe(this);

                WindowManager = null;
                DialogCoordinator = null;
                MatchManager = null;
                Events = null;
                ReportSettingsQueueManager = null;
            }
        }
    }
}
