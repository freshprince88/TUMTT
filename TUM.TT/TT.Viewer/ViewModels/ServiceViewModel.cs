using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.ViewModels
{
    class ServiceViewModel : Conductor<IScreen>.Collection.AllActive
    {
        public SpinControlViewModel SpinControl { get; set; }
        public TableViewModel TableView { get; set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public ServiceViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
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

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Subscribe ourself to the event bus
            this.events.Subscribe(this);

            // Activate the welcome model
            SpinControl = new SpinControlViewModel();
            TableView = new TableViewModel(events);
            this.ActivateItem(SpinControl);
            this.ActivateItem(TableView);
        }

    }
}
