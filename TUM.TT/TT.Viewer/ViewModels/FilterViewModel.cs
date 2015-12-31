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
    public class FilterViewModel : Conductor<IScreen>.Collection.OneActive,
        IHandle<FilterSwitchedEvent>
    {
        public ServiceViewModel ServiceView { get; set; }
        public ReceiveViewModel ReceiveView { get; set; }
        public ThirdBallViewModel ThirdBallView { get; set; }
        public FourthBallViewModel FourthBallView { get; set; }
        public LastBallViewModel LastBallView { get; set; }
        public TotalMatchViewModel TotalMatchView { get; set; }
        public CombiViewModel CombiView { get; set; }
        private Playlist ActivePlaylist {  get; set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public FilterViewModel(IEventAggregator eventAggregator)
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
            LastBallView = new LastBallViewModel(this.events);
            FourthBallView = new FourthBallViewModel(this.events);
            ThirdBallView = new ThirdBallViewModel(this.events);
            ReceiveView = new ReceiveViewModel(this.events);
            ServiceView = new ServiceViewModel(this.events);
            TotalMatchView = new TotalMatchViewModel(this.events);
            CombiView = new CombiViewModel(this.events);

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
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(1));

                    break;
                case "ReceiveFilterTabHeader":
                    this.ActivateItem(ReceiveView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(2));
                    break;
                case "ThirdFilterTabHeader":
                    this.ActivateItem(ThirdBallView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(3));
                    break;
                case "FourthFilterTabHeader":
                    this.ActivateItem(FourthBallView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(4));
                    break;
                case "LastFilterTabHeader":
                    this.ActivateItem(LastBallView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(5));
                    break;
                case "TotalMatchFilterTabHeader":
                    this.ActivateItem(TotalMatchView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.BeginPublishOnUIThread(new RallyLengthChangedEvent(1));
                    break;
                case "KombiFilterTabHeader":
                    this.ActivateItem(CombiView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    this.events.BeginPublishOnUIThread(new RallyLengthChangedEvent(1));
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
