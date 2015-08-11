using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TT.Viewer.ViewModels
{
    public class FilterViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public ServiceViewModel ServiceView { get; set; }
        public IScreen ReceptionView { get; set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public FilterViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
        }

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Subscribe ourself to the event bus
            //this.events.Subscribe(this);

            // Activate the welcome model
            if (this.ActiveItem == null)
            {
                ServiceView = new ServiceViewModel(this.events);
                this.ActivateItem(ServiceView);
                ReceptionView = new ReceptionViewModel(this.events);
            }
        }

        public void FilterSelected(SelectionChangedEventArgs args)
        {
            TabItem selected = args.AddedItems[0] as TabItem;
            switch (selected.Name)
            {
                case "ServiceTabHeader":
                    this.ActivateItem(ServiceView);
                    break;
                case "ReceiveTabHeader":
                    this.ActivateItem(ReceptionView);
                    break;
                case "ThirdTabHeader":
                    break;
                case "FourthTabHeader":
                    break;
                case "LastTabHeader":
                    break;
                case "KombiTabHeader":
                    break;
                default:
                    break;
            }
        }
    }
}
