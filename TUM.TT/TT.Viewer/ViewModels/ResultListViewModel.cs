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
    public class ResultListViewModel : Screen, IResultViewTabItem,
        IHandle<ResultsChangedEvent>,
        IHandle<MediaControlEvent>,
        IHandle<FullscreenEvent>
    {
        public string Header { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public int PointsPlayer1 { get; set; }
        public int PointsPlayer2 { get; set; }
        public int totalRalliesCount { get; set; }

        private IEventAggregator Events;
        private IDialogCoordinator Dialogs;
        private IMatchManager Manager;


        public ObservableCollection<ResultListItem> Items { get; set; }
        public List<Rally> Rallies { get; set; }
        public bool IsFullScreen { get; private set; }

        public ResultListViewModel(IEventAggregator e, IDialogCoordinator c, IMatchManager man)
        {
            this.DisplayName = "Hitlist";
            Header = "Hitlist";
            Events = e;
            Dialogs = c;
            Manager = man;
            Player1 = "Player 1";
            Player2 = "Player 2";
            PointsPlayer1 = 0;
            PointsPlayer2 = 0;
            totalRalliesCount = 0;
            Items = new ObservableCollection<ResultListItem>();
            Rallies = new List<Rally>();
            
            // Subscribe ourself to the event bus
            Events.Subscribe(this);
        }

        public byte GetOrderInResultView()
        {
            return 0;
        }

        public string GetTabTitle(bool getShortTitle)
        {
            if (getShortTitle)
                return Properties.Resources.result_list_tab_title_short;
            else
                return Properties.Resources.result_list_tab_title;
        }

        #region View Methods

        public void ListItemSelected(SelectionChangedEventArgs e)
        {
            ResultListItem item = e.AddedItems.Count > 0 ? (ResultListItem)e.AddedItems[0] : null;
            if (item != null && Manager.ActiveRally != item.Rally)
            {
                Manager.ActiveRally = item.Rally;
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
            List<ResultListItem> temp = new List<ResultListItem>();
            Rallies = message.Rallies.ToList();
            foreach (var rally in Rallies)
            {
                temp.Add(new ResultListItem(rally));
            }

            Items = new ObservableCollection<ResultListItem>(temp);
            NotifyOfPropertyChange("Items");

            //for (int i = Items.Count - 1; i >= 0; i--)
            //{
            //    this.DeactivateItem(Items[i], true);
            //}

            //foreach (var rally in message.Rallies)
            //{
            //    this.ActivateItem(new ResultListItem(rally));
            //}
            //this.Items.Refresh();
        }

 

        public void Handle(MediaControlEvent message)
        {
            if (message.Source == Media.Source.Viewer)
            {
                var idx = Rallies.IndexOf(Manager.ActiveRally);
                switch (message.Ctrl)
                {
                    case Media.Control.Previous:
                        var rallyP = idx - 1 >= 0 ? Rallies[idx - 1] : null;
                        if (rallyP != null)
                        {
                            Events.PublishOnUIThread(new ResultListControlEvent(rallyP));
                        }
                        break;
                    case Media.Control.Next:
                        if (Rallies.Count() != 0)
                        {
                            var rallyN = idx + 1 < Rallies.Count ? Rallies[idx + 1] : Rallies[0];
                            if (rallyN != null && rallyN != Rallies[0])
                            {
                                Events.PublishOnUIThread(new ResultListControlEvent(rallyN));
                            }
                            else if (rallyN != null && rallyN == Rallies[0])
                            {
                                Events.PublishOnUIThread(new ResultListControlEvent(rallyN));
                                Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
                            }

                        }
                        else
                        {
                            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Stop, Media.Source.Viewer));
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        public void Handle(FullscreenEvent message)
        {
            IsFullScreen = message.Fullscreen;
        }

        #endregion

        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            Player1 = Manager.Match.FirstPlayer.Name.Split(' ')[0];
            Player2 = Manager.Match.SecondPlayer.Name.Split(' ')[0];
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
                Events.Unsubscribe(this);
            Items.Clear();
            base.OnDeactivate(close);
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            Items.Clear();
            Manager.SelectedRallies.Apply(rally => Items.Add(new ResultListItem(rally)));
            
            if (Manager.ActiveRally != null)
                Events.PublishOnUIThread(new ResultListControlEvent(Manager.ActiveRally));
        }

        #endregion
    }
}
