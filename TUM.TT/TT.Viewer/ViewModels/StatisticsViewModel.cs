using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class StatisticsViewModel : Conductor<IScreen>.Collection.OneActive,
        IHandle<FilterSwitchedEvent>
    {
        public ServiceStatisticsViewModel ServiceStatisticsView { get; set; }
        public ReceiveStatisticsViewModel ReceiveStatisticsView { get; set; }
        public ThirdBallStatisticsViewModel ThirdBallStatisticsView { get; set; }
        public FourthBallStatisticsViewModel FourthBallStatisticsView { get; set; }
        public LastBallStatisticsViewModel LastBallStatisticsView { get; set; }
        public TotalMatchStatisticsViewModel TotalMatchStatisticsView { get; set; }
        public Playlist ActivePlaylist { get; set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public StatisticsViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            this.ActivePlaylist = new Playlist();
        }

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Subscribe ourself to the event bus
            this.events.Subscribe(this);            
            ServiceStatisticsView = new ServiceStatisticsViewModel(this.events);
            ReceiveStatisticsView = new ReceiveStatisticsViewModel(this.events);
            ThirdBallStatisticsView = new ThirdBallStatisticsViewModel(this.events);
            FourthBallStatisticsView = new FourthBallStatisticsViewModel(this.events);
            LastBallStatisticsView = new LastBallStatisticsViewModel(this.events);
            TotalMatchStatisticsView = new TotalMatchStatisticsViewModel(this.events);

            // Activate the welcome model
            if (this.ActiveItem == null)
            {
                this.ActivateItem(ServiceStatisticsView);
            }
        }
       

        public void FilterSelected(SelectionChangedEventArgs args)
        {
            TabItem selected = args.AddedItems[0] as TabItem;
            switch (selected.Name)
            {
                case "ServiceStatisticsTabHeader":
                    this.ActivateItem(ServiceStatisticsView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(1));
                    break;
                case "ReceiveStatisticsTabHeader":
                    this.ActivateItem(ReceiveStatisticsView); 
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(2));
                    break;
                case "ThirdStatisticsTabHeader":
                    this.ActivateItem(ThirdBallStatisticsView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(3));
                    break;
                case "FourthStatisticsTabHeader":
                    this.ActivateItem(FourthBallStatisticsView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(4));
                    break;
                case "LastStatisticsTabHeader":
                    this.ActivateItem(LastBallStatisticsView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(5));
                    break;
                case "TotalMatchStatisticsTabHeader":
                    this.ActivateItem(TotalMatchStatisticsView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(1));
                    break;


                default:
                    break;
            }
        }

        public void Handle(FilterSwitchedEvent message)
        {
            this.ActivePlaylist = message.Playlist;
        }
    }
}
