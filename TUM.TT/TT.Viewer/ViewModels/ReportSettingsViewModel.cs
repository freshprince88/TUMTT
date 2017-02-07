using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using TT.Lib;
using TT.Lib.Util;
using System.Diagnostics;
using TT.Lib.Managers;
using TT.Lib.Results;
using System.Threading;
using TT.Lib.Events;
using TT.Report.Generators;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public sealed class ReportSettingsViewModel : Conductor<IScreen>.Collection.AllActive, IShell
    {
        private string _issuedReportId;
        private readonly Dictionary<string, object> _generatedReport;
        private Timer _reportGenerationTimer;

        public IMatchManager MatchManager { get; private set; }
        private IEventAggregator Events { get; set; }
        private IReportGenerationQueueManager ReportGenerationQueueManager { get; set; }        
        public Dictionary<int, string[]> StrokeStats { get; }
        public Dictionary<int, string[]> GeneralStats { get; }
        public SortedDictionary<string, List<Rally>> Sets { get; set; }

        private int _playerChoice;
        public int PlayerChoice {
            get
            {
                return _playerChoice;
            }
            set
            {
                if ((_playerChoice & value) == value)
                    _playerChoice -= value;
                else
                    _playerChoice += value;

                Debug.WriteLine("player choice: {0}", _playerChoice);
                NotifyOfPropertyChange();
            }
        }

        private bool _crunchTimeChoice;
        public bool CrunchTimeChoice
        {
            get
            {
                return _crunchTimeChoice;
            }
            set
            {
                _crunchTimeChoice = value;

                Debug.WriteLine("crunch time choice: {0}", _crunchTimeChoice);
                NotifyOfPropertyChange();
            }
        }

        private int _setChoice;
        public int SetChoice
        {
            get
            {
                return _setChoice;
            }
            set
            {
                if ((_setChoice & value) == value)
                    _setChoice -= value;
                else
                    _setChoice += value;
                Debug.WriteLine("set choice: {0}", _setChoice);
                NotifyOfPropertyChange();
            }
        }

        private int _serviceStatsChoice;
        public int ServiceStatsChoice
        {
            get
            {
                return _serviceStatsChoice;
            }
            set
            {
                if ((_serviceStatsChoice & value) == value)
                    _serviceStatsChoice -= value;
                else
                    _serviceStatsChoice += value;
                Debug.WriteLine("service stats choice: {0}", _serviceStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int _returnStatsChoice;
        public int ReturnStatsChoice
        {
            get
            {
                return _returnStatsChoice;
            }
            set
            {
                if ((_returnStatsChoice & value) == value)
                    _returnStatsChoice -= value;
                else
                    _returnStatsChoice += value;
                Debug.WriteLine("return stats choice: {0}", _returnStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int _thirdStatsChoice;
        public int ThirdStatsChoice
        {
            get
            {
                return _thirdStatsChoice;
            }
            set
            {
                if ((_thirdStatsChoice & value) == value)
                    _thirdStatsChoice -= value;
                else
                    _thirdStatsChoice += value;
                Debug.WriteLine("third stats choice: {0}", _thirdStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int _fourthStatsChoice;
        public int FourthStatsChoice
        {
            get
            {
                return _fourthStatsChoice;
            }
            set
            {
                if ((_fourthStatsChoice & value) == value)
                    _fourthStatsChoice -= value;
                else
                    _fourthStatsChoice += value;
                Debug.WriteLine("fourth stats choice: {0}", _fourthStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int _lastStatsChoice;
        public int LastStatsChoice
        {
            get
            {
                return _lastStatsChoice;
            }
            set
            {
                if ((_lastStatsChoice & value) == value)
                    _lastStatsChoice -= value;
                else
                    _lastStatsChoice += value;
                Debug.WriteLine("last stats choice: {0}", _lastStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int _allStatsChoice;
        public int AllStatsChoice
        {
            get
            {
                return _allStatsChoice;
            }
            set
            {
                if ((_allStatsChoice & value) == value)
                    _allStatsChoice -= value;
                else
                    _allStatsChoice += value;
                Debug.WriteLine("all stats choice: {0}", _allStatsChoice);
                NotifyOfPropertyChange();
            }
        }

        private int _expandState;
        public int ExpandState
        {
            get
            {
                return _expandState;
            }
            set
            {
                if ((_expandState & value) == value)
                    _expandState -= value;
                else
                    _expandState += value;
                Debug.WriteLine("expand state: {0}", _expandState);
                NotifyOfPropertyChange();
            }
        }

        private int _generalChoice;
        public int GeneralChoice
        {
            get
            {
                return _generalChoice;
            }
            set
            {
                if ((_generalChoice & value) == value)
                    _generalChoice -= value;
                else
                    _generalChoice += value;
                Debug.WriteLine("general choice: {0}", _generalChoice);
                NotifyOfPropertyChange();
            }
        }

        public List<int> AvailableCombis{ get; }        
        public List<int> SelectedCombis { get; }

        // ReSharper disable once UnusedMember.Global
        public ReportSettingsViewModel()
        {
            // default constructor for caliburn design time integration
        }

        public ReportSettingsViewModel(IMatchManager matchManager, IReportGenerationQueueManager reportGenerationQueueManager, IEventAggregator events)
        {
            MatchManager = matchManager;
            ReportGenerationQueueManager = reportGenerationQueueManager;
            Events = events;

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

            _generatedReport = new Dictionary<string, object>();

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
            _generatedReport["match"] = e.MatchHash;
            _generatedReport["reportid"] = e.ReportSettingsCode;
            _generatedReport["path"] = e.ReportPathTemp;
            Events?.PublishOnUIThread(new ReportPreviewChangedEvent(e.ReportPathTemp, e.ReportSettingsCode));
        }

        private void ReportSettingsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (MatchManager.Match == null)
                return;

            var ignoredProperties = new List<string> { "IsInitialized", "IsActive", "ExpandState" };
            if (!ignoredProperties.Contains(e.PropertyName))
            {
                Debug.WriteLine("property changed [sender={0} propname={1} propvalue={2}]", sender, e.PropertyName, sender.GetType().GetProperty(e.PropertyName).GetValue(sender));
                Events.PublishOnUIThread(new ReportSettingsChangedEvent(true, (string)GetCustomizationDictionary()["id"]));
                if (_reportGenerationTimer == null)
                    _reportGenerationTimer = new Timer(GenerateReport, false, TimeSpan.FromSeconds(1.5), TimeSpan.FromMilliseconds(-1));
                else
                    _reportGenerationTimer.Change(TimeSpan.FromSeconds(1.5), TimeSpan.FromMilliseconds(-1));
            }
        }
        
        public void GenerateReport(object genEvent = null)
        {
            var matchOpened = MatchManager.Match != null;
            var customizationDict = matchOpened ? GetCustomizationDictionary() : null;
            var generateRepSettingsChangedEvent = genEvent as bool?;

            if (generateRepSettingsChangedEvent == null || generateRepSettingsChangedEvent.Value)
                Events.PublishOnUIThread(new ReportSettingsChangedEvent(matchOpened, (string)customizationDict?["id"]));

            if (matchOpened)
            {
                var gen = new CustomizedReportGenerator()
                {
                    Customization = customizationDict,
                    Match = MatchManager.Match
                };
                _issuedReportId = MatchHashGenerator.GenerateMatchHash(MatchManager.Match) + gen.CustomizationId;
                ReportGenerationQueueManager.Enqueue(gen);
            }
        }

        public void OnOkClick()
        {
            Debug.WriteLine("OnOkClick");
            Save();
            ReportGenerationQueueManager.Stop(true, true);
            TryClose();
        }

        public void OnCancelClick()
        {
            Debug.WriteLine("OnCancelClick");
            ReportGenerationQueueManager.Stop(true, false);
            TryClose();
        }

        public IEnumerable<IResult> OnOkGenerateClick()
        {
            Debug.WriteLine("OnOkGenerateClick");
            Save();

            foreach (var r in SaveGeneratedReport())
                yield return r;

            TryClose();
        }

        public IEnumerable<IResult> SaveGeneratedReport()
        {
            if (_issuedReportId == null && _generatedReport.GetValueOrDefault("reportid") == null)
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
        }

        private Dictionary<string, object> GetCustomizationDictionary()
        {
            var customizations = new Dictionary<string, object>();
            var customizationId = "";

            var players = new List<object>();
            if ((PlayerChoice & 1) == 1)
                players.Add(MatchManager.Match.FirstPlayer);
            if ((PlayerChoice & 2) == 2)
                players.Add(MatchManager.Match.SecondPlayer);
            if ((PlayerChoice & 4) == 4)
            {
                var bothPlayers = new List<Player> {MatchManager.Match.FirstPlayer, MatchManager.Match.SecondPlayer};
                players.Add(bothPlayers);
            }
            customizations["players"] = players;
            customizationId += "8" + PlayerChoice.ToString("X");
            // adding a number in front ensures any two setting choices that are the same (e.g. 00010) will still get different encodings

            var customizationSets = new Dictionary<string, List<Rally>>();
            // 'all' sets
            if ((SetChoice & 1) == 1)
                customizationSets["all"] = new List<Rally>(MatchManager.Match.DefaultPlaylist.Rallies);

            // 'crunch time sets
            if (CrunchTimeChoice)
            {
                customizationSets["crunchtime"] = new List<Rally>(MatchManager.Match.DefaultPlaylist.Rallies.Where(r => r.CurrentRallyScore.First + r.CurrentRallyScore.Second >= 16));
            }

            // sets 1-7
            foreach (var set in Sets.Keys)
            {
                var mask = 1 << int.Parse(set);
                if ((mask & _setChoice) == mask)
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
                    var mask = 1 << i;
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

            customizationId += "B" + (CrunchTimeChoice ? 1 : 0);

            customizations["sets"] = customizationSets;

            customizations["service_stats"] = new List<string>();
            customizations["return_stats"] = new List<string>();
            customizations["third_stats"] = new List<string>();
            customizations["fourth_stats"] = new List<string>();
            customizations["last_stats"] = new List<string>();
            customizations["all_stats"] = new List<string>();
            foreach (var key in StrokeStats.Keys)
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
            foreach (var key in GeneralStats.Keys)
            {
                if ((GeneralChoice & key) == key)
                    ((List<string>)customizations["general"]).Add(GeneralStats[key][0]);
            }
            customizationId += "7" + GeneralChoice.ToString("X");
            
            Debug.WriteLine($"customization id={customizationId}");

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
                    NotifyOfPropertyChange(nameof(SelectedCombis));
                }
            }
            else
            {
                SelectedCombis.Remove(combi);
                NotifyOfPropertyChange(nameof(SelectedCombis));
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
            Properties.Settings.Default.ReportGenerator_CrunchTimeChoice = CrunchTimeChoice;
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
            CrunchTimeChoice = Properties.Settings.Default.ReportGenerator_CrunchTimeChoice;

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
                DiscardViewModel(false);
            }
        }

        public void DiscardViewModel(bool finishLastReport)
        {
            PropertyChanged -= ReportSettingsViewModel_PropertyChanged;
            ReportGenerationQueueManager.ReportGenerated -= ReportGenerationQueueManager_ReportGenerated;
            Events.Unsubscribe(this);

            ReportGenerationQueueManager.Stop(false, finishLastReport);

            MatchManager = null;
            Events = null;
            ReportGenerationQueueManager = null;
        }
    }
}
