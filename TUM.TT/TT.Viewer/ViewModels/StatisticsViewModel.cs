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
        IHandle<MatchOpenedEvent>,
        IHandle<PlaylistChangedEvent>
    {
        public ServiceStatisticsViewModel ServiceStatisticsView { get; set; }
        
      
        private Playlist ActivePlaylist;
        private Match match;

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
                //case "ReceiveTabHeader":
                //    this.ActivateItem();
                //    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                //    this.events.PublishOnUIThread(new RallyLengthChangedEvent(2));
                //    break;
                //case "ThirdTabHeader":
                //    this.ActivateItem();
                //    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                //    this.events.PublishOnUIThread(new RallyLengthChangedEvent(3));
                //    break;
                //case "FourthTabHeader":
                //    this.ActivateItem(FourthBallView);
                //    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                //    this.events.PublishOnUIThread(new RallyLengthChangedEvent(4));
                //    break;
                //case "LastTabHeader":
                //    this.ActivateItem(LastBallView);
                //    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                //    this.events.PublishOnUIThread(new RallyLengthChangedEvent(5));
                //    break;
                
                default:
                    break;
            }
        }

        public void Handle(MatchOpenedEvent message)
        {
            this.match = message.Match;
            this.ActivePlaylist = message.Match.Playlists.Where(p => p.Name == "Alle").FirstOrDefault();
            this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
        }

        public void Handle(PlaylistChangedEvent message)
        {
            this.ActivePlaylist = this.match.Playlists.Where(p => p.Name == message.PlaylistName).FirstOrDefault();
            this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
        }
    }
}
