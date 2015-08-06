using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.ViewModels
{
    class ServiceViewModel : Screen
    {
        public SpinControlViewModel SpinControl { get; set; }
        public TableViewModel TableView { get; set; }

        public ServiceViewModel(IEventAggregator eventAggregator)
        {
            SpinControl = new SpinControlViewModel();
            TableView = new TableViewModel(eventAggregator);
        }

        public void SwitchTable(object o)
        {
            ToggleSwitch toggle = o as ToggleSwitch;
            if (toggle.IsChecked.Value)
            {
                TableView.Mode = TableViewModel.ViewMode.Top;
            }
            else
            {
                TableView.Mode = TableViewModel.ViewMode.Bottom;
            }
        }

    }
}
