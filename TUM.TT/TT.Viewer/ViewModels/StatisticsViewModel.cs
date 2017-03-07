using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class StatisticsViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public ServiceStatisticsViewModel ServiceStatisticsView { get; set; }
        public ReceiveStatisticsViewModel ReceiveStatisticsView { get; set; }
        public ThirdBallStatisticsViewModel ThirdBallStatisticsView { get; set; }
        public FourthBallStatisticsViewModel FourthBallStatisticsView { get; set; }
        public LastBallStatisticsViewModel LastBallStatisticsView { get; set; }
        public TotalMatchStatisticsViewModel TotalMatchStatisticsView { get; set; }
        public int SelectedTab { get; set; }
        public Playlist ActivePlaylist { get; set; }
        private Match Match { get; set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public StatisticsViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            this.ActivePlaylist = new Playlist(Match);

            ServiceStatisticsView = new ServiceStatisticsViewModel(this.events, Manager);
            ReceiveStatisticsView = new ReceiveStatisticsViewModel(this.events, Manager);
            ThirdBallStatisticsView = new ThirdBallStatisticsViewModel(this.events, Manager);
            FourthBallStatisticsView = new FourthBallStatisticsViewModel(this.events, Manager);
            LastBallStatisticsView = new LastBallStatisticsViewModel(this.events, Manager);
            TotalMatchStatisticsView = new TotalMatchStatisticsViewModel(this.events, Manager);
            SelectedTab = 0;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            // Activate the welcome model
            if (this.ActiveItem == null)
            {
                this.ActivateItem(ServiceStatisticsView);
                
            }
        }

        protected override void OnDeactivate(bool close)
        {
            this.events.Unsubscribe(this);
            base.OnDeactivate(close);
        }


        public void FilterSelected(SelectionChangedEventArgs args)
        {
            TabItem selected = args.AddedItems[0] as TabItem;
            switch (selected.Name)
            {
                case "ServiceStatisticsTabHeader":
                    this.ActivateItem(ServiceStatisticsView);                  
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(1));
                    break;
                case "ReceiveStatisticsTabHeader":
                    this.ActivateItem(ReceiveStatisticsView);                     
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(2));
                    break;
                case "ThirdStatisticsTabHeader":
                    this.ActivateItem(ThirdBallStatisticsView);
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(3));
                    break;
                case "FourthStatisticsTabHeader":
                    this.ActivateItem(FourthBallStatisticsView);
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(4));
                    break;
                case "LastStatisticsTabHeader":
                    this.ActivateItem(LastBallStatisticsView);
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(5));
                    break;
                case "TotalMatchStatisticsTabHeader":
                    this.ActivateItem(TotalMatchStatisticsView);
                    this.events.PublishOnUIThread(new RallyLengthChangedEvent(1));
                    break;
                default:
                    break;
            }
        }
    }
}
