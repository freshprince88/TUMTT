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

namespace TT.Viewer.ViewModels
{
    public class ReportSettingsViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx
    {
        public IEventAggregator events { get; private set; }
        private readonly IWindowManager WindowManager;
        private IDialogCoordinator DialogCoordinator;
        public IMatchManager MatchManager { get; set; }
        public IReportSettingsQueueManager ReportSettingsQueueManager { get; set; }

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
            this.events = events;
            this.WindowManager = windowManager;
            this.DialogCoordinator = dialogCoordinator;
            
            DisplayName = "Report Settings";
            PropertyChanged += ReportSettingsViewModel_PropertyChanged;

            ReportSettingsQueueManager.ReportGenerated += ReportSettingsQueueManager_ReportGenerated;

            Load();
        }

        private void ReportSettingsQueueManager_ReportGenerated(object sender, ReportGeneratedEventArgs e)
        {
            Debug.WriteLine("sender={0} report={1}", sender, e.Report);
        }

        private void ReportSettingsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("sender={0} propname={1} propvalue={2}", sender, e.PropertyName, sender.GetType().GetProperty(e.PropertyName).GetValue(sender));
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
            ReportSettingsQueueManager.Enqueue(new TT.Report.Generators.CustomizedReportGenerator());
            //foreach (IResult result in MatchManager.GenerateReport("customized"))
            //    yield return result;
            TryClose();
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
