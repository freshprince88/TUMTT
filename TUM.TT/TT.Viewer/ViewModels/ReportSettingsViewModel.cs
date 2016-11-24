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

namespace TT.Viewer.ViewModels
{
    public class ReportSettingsViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx
    {
        public IEventAggregator events { get; private set; }
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;

        public ReportSettingsViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IDialogCoordinator coordinator)
        {
            this.DisplayName = "Report Settings";
            this.events = eventAggregator;
            _windowManager = windowmanager;
            DialogCoordinator = coordinator;            
        }
        
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name) ? Application.Current.Windows.OfType<T>().Any() : Application.Current.Windows.OfType<T>().Any(wde => wde.Name.Equals(name));
        }
        
        public void OnOkClick()
        {
            Debug.WriteLine("OnOkClick");
            TryClose();
        }

        public void OnCancelClick()
        {
            Debug.WriteLine("OnCancelClick");
            TryClose();
        }

    }
}
