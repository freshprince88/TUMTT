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
    public class FilterStatisticsViewModel : Conductor<IScreen>.Collection.OneActive,
        IHandle<MatchOpenedEvent>,
        IHandle<PlaylistChangedEvent>
    {
        public FilterViewModel FilterView { get; set; }
        public StatisticsViewModel StatisticsView { get; set; }
        private Playlist ActivePlaylist { get; set; }
        private Match match;
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public FilterStatisticsViewModel(IEventAggregator eventAggregator)
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
            FilterView = new FilterViewModel(this.events);
            StatisticsView = new StatisticsViewModel(this.events);
            // Activate the welcome model
            if (this.ActiveItem == null)
            {
                this.ActivateItem(FilterView);
            }
        }

        public void FilterStatisticsSelected(SelectionChangedEventArgs args)
        {
            TabItem selected = args.AddedItems[0] as TabItem;
            switch (selected.Name)
            { //wie kann ich zwischen den 2 TabControls unterscheiden?

                case "FilterTab":
                    this.events.PublishOnUIThread(new MatchInformationEvent(this.match));
                    this.ActivateItem(FilterView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    
                    break;

                case "StatisticsTab":
                    this.events.PublishOnUIThread(new MatchInformationEvent(this.match));
                    this.ActivateItem(StatisticsView);
                    this.events.PublishOnUIThread(new FilterSwitchedEvent(this.ActivePlaylist));
                    
                    break;
            }
        }
        public void Handle(MatchOpenedEvent message)
        {
            this.match = message.Match;
            this.events.PublishOnUIThread(new MatchInformationEvent(this.match));
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
