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
using System.Windows.Forms;
using TT.Lib.Events;
using TT.Report.Generators;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class ReportSettingsViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx
    {
        private IWindowManager WindowManager;
        private IDialogCoordinator DialogCoordinator;
        private string issuedReportId;
        private Dictionary<string, object> generatedReport;
        private NotifyIcon ni;

        public IMatchManager MatchManager { get; private set; }
        public IEventAggregator Events { get; private set; }
        public IReportGenerationQueueManager ReportGenerationQueueManager { get; private set; }        
        public Dictionary<int, string[]> StrokeStats { get; private set; }
        public Dictionary<int, string[]> GeneralStats { get; private set; }
        public SortedDictionary<string, List<Rally>> Sets { get; set; }

        private int playerChoice;
        public int PlayerChoice {
            get
            {
                return playerChoice;
            }
            set
            {
                if ((playerChoice & value) == value)
                    playerChoice -= value;
                else
                    playerChoice += value;

                Debug.WriteLine("player choice: {0}", playerChoice);
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
        public List<int> SelectedCombis { get; set; }

        public ReportSettingsViewModel()
        {
            // default constructor for caliburn design time integration
        }

        public ReportSettingsViewModel(IMatchManager matchManager, IReportGenerationQueueManager reportGenerationQueueManager, IWindowManager windowManager, IEventAggregator events, IDialogCoordinator dialogCoordinator)
        {
            MatchManager = matchManager;
            ReportGenerationQueueManager = reportGenerationQueueManager;
            Events = events;
            WindowManager = windowManager;
            DialogCoordinator = dialogCoordinator;

            Sets = new SortedDictionary<string, List<Rally>>();
            if (MatchManager.Match != null)
            {
                foreach (var rally in MatchManager.Match.DefaultPlaylist.Rallies)
                {
                    var setNumber = (rally.CurrentSetScore.First + rally.CurrentSetScore.Second + 1).ToString();
                    List<Rally> rallyList;
                    if (!Sets.TryGetValue(setNumber, out rallyList))
                        Sets[setNumber] = new List<Rally>();
                    Sets[setNumber].Add(rally);
                }
            }

            generatedReport = new Dictionary<string, object>();

            StrokeStats = new Dictionary<int, string[]>
            {
                [1] = new[] {"side", Properties.Resources.report_settings_strokechoice_side},
                [2] = new[] {"steparound", Properties.Resources.report_settings_strokechoice_steparound},
                [4] = new[] {"spin", Properties.Resources.table_spin_title},
                [8] = new[] {"technique", Properties.Resources.report_settings_strokechoice_technique},
                [16] = new[] {"placement", Properties.Resources.report_settings_strokechoice_placement},
                [32] = new[] {"table", Properties.Resources.table_large_tab_title},
                [64] = new[] {"service", Properties.Resources.report_settings_strokechoice_service},
                [128] = new[] {"number", Properties.Resources.report_settings_strokechoice_number}
            };

            GeneralStats = new Dictionary<int, string[]>
            {
                [1] = new[] {"rallylength", Properties.Resources.report_settings_generalchoice_rallylength},
                [2] = new[] {"matchdynamics", Properties.Resources.report_settings_generalchoice_matchdynamics},
                [4] =
                new[] {"transitionmatrix", Properties.Resources.report_settings_generalchoice_transitionmatrix},
                [8] = new[] {"techefficiency", Properties.Resources.report_settings_generalchoice_techefficiency}
            };

            DisplayName = Properties.Resources.report_settings_window_title;
            AvailableCombis = new List<int>();
            SelectedCombis = new List<int>();

            Load();
            ReportGenerationQueueManager.Start();

            PropertyChanged += ReportSettingsViewModel_PropertyChanged;
            ReportGenerationQueueManager.ReportGenerated += ReportGenerationQueueManager_ReportGenerated;
        }

        private void ReportGenerationQueueManager_ReportGenerated(object sender, ReportGeneratedEventArgs e)
        {
            Debug.WriteLine("report generated [sender={0} report={1}]", sender, e.ReportPathTemp);
            generatedReport["match"] = e.MatchHash;
            generatedReport["reportid"] = e.ReportSettingsCode;
            generatedReport["path"] = e.ReportPathTemp;
            if (Events != null) // this is null in some scenarios 
                Events.PublishOnUIThread(new ReportPreviewChangedEvent(e.ReportPathTemp));
        }

        private void ReportSettingsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var ignoredProperties = new List<string> { "IsInitialized", "IsActive", "ExpandState" };
            if (!ignoredProperties.Contains(e.PropertyName))
            {
                Debug.WriteLine("property changed [sender={0} propname={1} propvalue={2}]", sender, e.PropertyName, sender.GetType().GetProperty(e.PropertyName).GetValue(sender));
                GenerateReport();
            }
        }
        
        public void GenerateReport()
        {
            bool matchOpened = MatchManager.Match != null;
            Events.PublishOnUIThread(new ReportSettingsChangedEvent(matchOpened));

            if (matchOpened)
            {
                CustomizedReportGenerator gen = new CustomizedReportGenerator()
                {
                    Customization = GetCustomizationDictionary(),
                    Match = MatchManager.Match
                };
                issuedReportId = MatchHashGenerator.GenerateMatchHash(MatchManager.Match) + gen.CustomizationId;
                ReportGenerationQueueManager.Enqueue(gen);
            }
        }

        public void OnOkClick()
        {
            Debug.WriteLine("OnOkClick");
            Save();
            ReportGenerationQueueManager.Stop(true);
            TryClose();
        }

        public void OnCancelClick()
        {
            Debug.WriteLine("OnCancelClick");
            ReportGenerationQueueManager.Stop(true);
            TryClose();
        }

        public IEnumerable<IResult> OnOkGenerateClick()
        {
            Debug.WriteLine("OnOkGenerateClick");
            Save();

            foreach (var r in SaveGeneratedReport(false))
                yield return r;

            TryClose();
        }

        public IEnumerable<IResult> SaveGeneratedReport(bool exitAfter)
        {
            if (issuedReportId == null && generatedReport.GetValueOrDefault("reportid") == null)
                Debug.WriteLine("Saving generated report failed. Report neither generated nor issued.");
            else
            {
                var dialog = new SaveFileDialogResult()
                {
                    Title = "Choose a target for PDF report",
                    Filter = "PDF reports|*.pdf",
                    DefaultFileName = MatchManager.Match.DefaultFilename(),
                };
                yield return dialog;
                
                ReportGenerationQueueManager.ReportPathUser = dialog.Result;
            }
            if (exitAfter)
                OnDeactivate(true);
        }

        private Dictionary<string, object> GetCustomizationDictionary()
        {
            Dictionary<string, object> customizations = new Dictionary<string, object>();
            var customizationId = "";

            List<object> players = new List<object>();
            if ((PlayerChoice & 1) == 1)
                players.Add(MatchManager.Match.FirstPlayer);
            if ((PlayerChoice & 2) == 2)
                players.Add(MatchManager.Match.SecondPlayer);
            if ((PlayerChoice & 4) == 4)
            {
                List<Player> bothPlayers = new List<Player>();
                bothPlayers.Add(MatchManager.Match.FirstPlayer);
                bothPlayers.Add(MatchManager.Match.SecondPlayer);
                players.Add(bothPlayers);
            }
            customizations["players"] = players;
            customizationId += "8" + PlayerChoice.ToString("X");
            // adding a number in front ensures any two setting choices that are the same (e.g. 00010) will still get different encodings

            Dictionary<string, List<Rally>> customizationSets = new Dictionary<string, List<Rally>>();
            // 'all' sets
            if ((SetChoice & 1) == 1)
                customizationSets["all"] = new List<Rally>(MatchManager.Match.DefaultPlaylist.Rallies);

            // sets 1-7
            foreach (var set in Sets.Keys)
            {
                var mask = 1 << int.Parse(set);
                if ((mask & setChoice) == mask)
                    customizationSets[set] = Sets[set];
            }
            customizationId += "9" + SetChoice.ToString("X");

            // set combis
            var selectedCombisSorted = new List<int>(SelectedCombis);
            selectedCombisSorted.Sort();
            customizationId += "A";
            foreach (var combi in selectedCombisSorted)
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
                customizationSets[combiName.Substring(0, combiName.Length - 1)] = rallyList;
                customizationId += combi.ToString("X");
            }

            customizations["sets"] = customizationSets;

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

            if (ServiceStatsChoice != 0)
                customizationId += "1" + ServiceStatsChoice.ToString("X");
            if (ReturnStatsChoice != 0)
                customizationId += "2" + ReturnStatsChoice.ToString("X");
            if (ThirdStatsChoice != 0)
                customizationId += "3" + ThirdStatsChoice.ToString("X");
            if (FourthStatsChoice != 0)
                customizationId += "4" + FourthStatsChoice.ToString("X");
            if (LastStatsChoice != 0)
                customizationId += "5" + LastStatsChoice.ToString("X");
            if (AllStatsChoice != 0)
                customizationId += "6" + AllStatsChoice.ToString("X");

            customizations["general"] = new List<string>();
            foreach (int key in GeneralStats.Keys)
            {
                if ((GeneralChoice & key) == key)
                    ((List<string>)customizations["general"]).Add(GeneralStats[key][0]);
            }
            customizationId += "7" + GeneralChoice.ToString("X");
            
            Debug.WriteLine("customization id={0}", customizationId, "");

            customizations["id"] = customizationId;
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
                {
                    SelectedCombis.Add(combi);
                    NotifyOfPropertyChange("SelectedCombis");
                }
            }
            else
            {
                SelectedCombis.Remove(combi);
                NotifyOfPropertyChange("SelectedCombis");
            }
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
                ReportGenerationQueueManager.ReportGenerated -= ReportGenerationQueueManager_ReportGenerated;
                Events.Unsubscribe(this);

                ReportGenerationQueueManager.Stop(false);

                WindowManager = null;
                DialogCoordinator = null;
                MatchManager = null;
                Events = null;
                ReportGenerationQueueManager = null;
            }
        }
    }
}
