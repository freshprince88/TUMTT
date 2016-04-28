using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Models.Events;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class FilterViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public ServiceViewModel ServiceView { get; set; }
        public ReceiveViewModel ReceiveView { get; set; }
        public ThirdBallViewModel ThirdBallView { get; set; }
        public FourthBallViewModel FourthBallView { get; set; }
        public LastBallViewModel LastBallView { get; set; }
        public TotalMatchViewModel TotalMatchView { get; set; }
        public CombiViewModel CombiView { get; set; }
        public int SelectedTab { get; set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public FilterViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            this.Manager = man;
            SelectedTab = 0;
        }

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            LastBallView = new LastBallViewModel(this.events, Manager);
            FourthBallView = new FourthBallViewModel(this.events, Manager);
            ThirdBallView = new ThirdBallViewModel(this.events, Manager);
            ReceiveView = new ReceiveViewModel(this.events, Manager);
            ServiceView = new ServiceViewModel(this.events, Manager);
            TotalMatchView = new TotalMatchViewModel(this.events, Manager);
            CombiView = new CombiViewModel(this.events, Manager);

            // Activate the welcome model
            if (this.ActiveItem == null)
            {
                this.ActivateItem(ServiceView);
            }
        }


        public void FilterSelected(SelectionChangedEventArgs args)
        {
            TabItem selected = args.AddedItems[0] as TabItem;
            switch (selected.Name)
            {
                case "ServiceFilterTabHeader":
                    this.ActivateItem(ServiceView);
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(1));
                    break;
                case "ReceiveFilterTabHeader":
                    this.ActivateItem(ReceiveView);
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(2));
                    break;
                case "ThirdFilterTabHeader":
                    this.ActivateItem(ThirdBallView);
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(3));
                    break;
                case "FourthFilterTabHeader":
                    this.ActivateItem(FourthBallView);
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(4));
                    break;
                case "LastFilterTabHeader":
                    this.ActivateItem(LastBallView);
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(5));
                    break;
                case "TotalMatchFilterTabHeader":
                    this.ActivateItem(TotalMatchView);
                    this.events.BeginPublishOnUIThread(new RallyLengthChangedEvent(1));
                    break;
                case "KombiFilterTabHeader":
                    this.ActivateItem(CombiView);
                    this.events.BeginPublishOnUIThread(new RallyLengthChangedEvent(1));
                    break;
                default:
                    break;
            }
        }
    }
}
