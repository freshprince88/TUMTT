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
using TT.Report.Generators;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class ReportSettingsViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx
    {
        private IWindowManager WindowManager;
        private IDialogCoordinator DialogCoordinator;
        public IMatchManager MatchManager { get; private set; }
        public IEventAggregator Events { get; private set; }
        public IReportSettingsQueueManager ReportSettingsQueueManager { get; private set; }
        
        public Dictionary<int, string[]> StrokeStats { get; private set; }
        public Dictionary<int, string[]> GeneralStats { get; private set; }

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
                Debug.WriteLine("expand state: {0}", expandState);
                NotifyOfPropertyChange();
            }
        }

        private int generalChoice;
        public int GeneralChoice
        {
            get
            {
                return generalChoice;
            }
            set
            {
                if ((generalChoice & value) == value)
                    generalChoice -= value;
                else
                    generalChoice += value;
                Debug.WriteLine("general choice: {0}", generalChoice);
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

            StrokeStats = new Dictionary<int, string[]>();
            StrokeStats[1] = new string[] { "placement", Properties.Resources.report_settings_strokechoice_placement };
            StrokeStats[2] = new string[] { "technique", Properties.Resources.report_settings_strokechoice_technique };
            StrokeStats[4] = new string[] { "table", Properties.Resources.table_large_tab_title };
            StrokeStats[8] = new string[] { "steparound", Properties.Resources.report_settings_strokechoice_steparound };
            StrokeStats[16] = new string[] { "spin", Properties.Resources.table_spin_title };
            StrokeStats[32] = new string[] { "service", Properties.Resources.report_settings_strokechoice_service };
            StrokeStats[64] = new string[] { "number", Properties.Resources.report_settings_strokechoice_number };

            GeneralStats = new Dictionary<int, string[]>();
            GeneralStats[1] = new string[] { "rallylength", Properties.Resources.report_settings_generalchoice_rallylength };
            GeneralStats[2] = new string[] { "matchdynamics", Properties.Resources.report_settings_generalchoice_matchdynamics };
            GeneralStats[4] = new string[] { "transitionmatrix", Properties.Resources.report_settings_generalchoice_transitionmatrix };
            GeneralStats[8] = new string[] { "techefficiency", Properties.Resources.report_settings_generalchoice_techefficiency };

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

            CustomizedReportGenerator gen = new CustomizedReportGenerator();
            gen.Customization = GetCustomizationDictionary();
            ReportSettingsQueueManager.Enqueue(gen);

            return null;
        }

        private Dictionary<string, object> GetCustomizationDictionary()
        {
            Dictionary<string, object> customizations = new Dictionary<string, object>();

            List<Player> players = new List<Player>();
            if (PlayerChoice == "first")
                players.Add(MatchManager.Match.FirstPlayer);
            else if (PlayerChoice == "second")
                players.Add(MatchManager.Match.SecondPlayer);
            else if (PlayerChoice == "both")
            {
                players.Add(MatchManager.Match.FirstPlayer);
                players.Add(MatchManager.Match.SecondPlayer);
            }
            customizations["players"] = players;


            Dictionary<string, List<Rally>> sets = new Dictionary<string, List<Rally>>();
            // 'all' sets
            if ((SetChoice & 1) == 1)
                sets["all"] = new List<Rally>(MatchManager.Match.DefaultPlaylist.Rallies);

            // sets 1-7
            for (var i = 1; i < Math.Ceiling(Math.Log(SetChoice, 2)); i++)
            {
                int mask = 1 << i;
                if ((mask & SetChoice) == mask)
                {
                    var rallyList = new List<Rally>();
                    foreach (var rally in MatchManager.Match.DefaultPlaylist.Rallies)
                    {
                        if (rally.CurrentSetScore.First + rally.CurrentSetScore.Second + 1 == i)
                        {
                            rallyList.Add(rally);
                        }
                    }
                    sets[i.ToString()] = rallyList;
                }
            }

            // set combis
            foreach (var combi in SelectedCombis)
            {
                var combiName = "";
                var rallyList = new List<Rally>();
                for (var i = 1; i < Math.Ceiling(Math.Log(combi, 2)); i++)
                {
                    int mask = 1 << i;
                    if ((mask & combi) == mask)
                    {
                        combiName += i + ",";
                        foreach (var rally in MatchManager.Match.DefaultPlaylist.Rallies)
                        {
                            if (rally.CurrentSetScore.First + rally.CurrentSetScore.Second + 1 == i)
                            {
                                rallyList.Add(rally);
                            }
                        }
                    }
                }
                sets[combiName.Substring(0, combiName.Length - 1)] = rallyList;
            }

            customizations["sets"] = sets;

            customizations["service_stats"] = new List<string>();
            customizations["return_stats"] = new List<string>();
            customizations["third_stats"] = new List<string>();
            customizations["fourth_stats"] = new List<string>();
            customizations["last_stats"] = new List<string>();
            customizations["all_stats"] = new List<string>();
            foreach (int key in StrokeStats.Keys)
            {
                if ((ServiceStatsChoice & key) == key)
                    ((List<string>)customizations["service_stats"]).Add(StrokeStats[key][0]);
                if ((ReturnStatsChoice & key) == key)
                    ((List<string>)customizations["return_stats"]).Add(StrokeStats[key][0]);
                if ((ThirdStatsChoice & key) == key)
                    ((List<string>)customizations["third_stats"]).Add(StrokeStats[key][0]);
                if ((FourthStatsChoice & key) == key)
                    ((List<string>)customizations["fourth_stats"]).Add(StrokeStats[key][0]);
                if ((LastStatsChoice & key) == key)
                    ((List<string>)customizations["last_stats"]).Add(StrokeStats[key][0]);
                if ((AllStatsChoice & key) == key)
                    ((List<string>)customizations["all_stats"]).Add(StrokeStats[key][0]);
            }

            customizations["general"] = new List<string>();
            foreach (int key in GeneralStats.Keys)
            {
                if ((GeneralChoice & key) == key)
                    ((List<string>)customizations["general"]).Add(GeneralStats[key][0]);
            }

            return customizations;
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
            Properties.Settings.Default.ReportGenerator_GeneralChoice = GeneralChoice;
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
            GeneralChoice = Properties.Settings.Default.ReportGenerator_GeneralChoice;

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
