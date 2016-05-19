using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Models.Util.Enums;
using TT.Lib.Events;
using TT.Lib.Managers;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class ResultListViewModel : Conductor<IScreen>.Collection.AllActive, IResultViewTabItem,
        IHandle<ResultsChangedEvent>,
        IHandle<FullscreenEvent>
    {
        public string Header { get; set; }
        public string Player1 {get; set;}
        public string Player2 { get; set; }
        public int PointsPlayer1 { get; set; }
        public int PointsPlayer2 { get; set; }
        public int totalRalliesCount { get; set; }
        //public ResultMiniStatisticViewModel MiniStatistic { get; set; }
        

        private IEventAggregator events;
        private IDialogCoordinator dialogs;
        private IMatchManager manager;
        private int count;


        public ResultListViewModel(IEventAggregator e, IDialogCoordinator c, IMatchManager man)
        {
            this.DisplayName = "Hitlist";
            Header = "Hitlist (" + count + ")";
            events = e;
            dialogs = c;
            manager = man;
            count = 0;
            Player1 = "Spieler 1";
            Player2 = "Spieler 2";
            PointsPlayer1 = 0;
            PointsPlayer2 = 0;
            totalRalliesCount = 0;

            //MiniStatistic = new ResultMiniStatisticViewModel(this.events, manager);

        }

        #region View Methods

        public void ListItemSelected(SelectionChangedEventArgs e)
        {
            ResultListItem item = e.AddedItems.Count > 0 ? (ResultListItem)e.AddedItems[0] : null;
            if (item != null)
            {
                this.events.PublishOnUIThread(new VideoPlayEvent()
                {
                    Current = item.Rally
                });
            }           
        }

        public void RightMouseDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        

        #endregion

        #region Event Handlers

        public void Handle(ResultsChangedEvent message)
        {

            for (int i = Items.Count - 1; i >= 0; i--)
            {
                this.DeactivateItem(Items[i], true);
            }

            foreach (var rally in message.Rallies)
            {
                this.ActivateItem(new ResultListItem(rally));
            }
            
            count=this.Items.Count();
            this.DisplayName = "Hitlist (" + count + ")";
            Header = "Hitlist (" + count + ")";
            NotifyOfPropertyChange("Header");

            this.Items.Refresh();
            



        }

        public void Handle(FullscreenEvent message)
        {
            switch (message.Fullscreen)
            {
                case true:
                    this.DisplayName = "R(" + count + ")";
                    Header = "R(" + count + ")";
                    break;
                case false:
                    this.DisplayName = "Hitlist (" + count + ")";
                    Header = "Hitlist (" + count + ")";
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            Player1 = manager.Match.FirstPlayer.Name.Split(' ')[0];
            Player2 = manager.Match.SecondPlayer.Name.Split(' ')[0];
            //this.ActivateItem(MiniStatistic);

        }
        #endregion
    }
}
