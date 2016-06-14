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
        IHandle<FullscreenEvent>,
        IHandle<MediaControlEvent>
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
        private int count;

        public ObservableCollection<ResultListItem> Items { get; set; }
        public List<Rally> Rallies { get; set; }


        public ResultListViewModel(IEventAggregator e, IDialogCoordinator c, IMatchManager man)
        {
            this.DisplayName = "Hitlist";
            Header = "Hitlist (" + count + ")";
            Events = e;
            Dialogs = c;
            Manager = man;
            count = 0;
            Player1 = "Player 1";
            Player2 = "Player 2";
            PointsPlayer1 = 0;
            PointsPlayer2 = 0;
            totalRalliesCount = 0;
            Items = new ObservableCollection<ResultListItem>();
            Rallies = new List<Rally>();
        }

        #region View Methods

        public void ListItemSelected(SelectionChangedEventArgs e)
        {
            ResultListItem item = e.AddedItems.Count > 0 ? (ResultListItem)e.AddedItems[0] : null;
            if (item != null)
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

            count = Items.Count();
            this.DisplayName = "Hitlist (" + count + ")";
            Header = "Hitlist (" + count + ")";
            NotifyOfPropertyChange("Header");

            //this.Items.Refresh();
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
                            Manager.ActiveRally = rallyP;
                        }
                        break;
                    case Media.Control.Next:
                        if (Rallies.Count() != 0)
                        {
                            var rallyN = idx + 1 < Rallies.Count ? Rallies[idx + 1] : Rallies[0];
                            if (rallyN != null && rallyN != Rallies[0])
                            {
                                Events.PublishOnUIThread(new ResultListControlEvent(rallyN));
                                Manager.ActiveRally = rallyN;
                            }
                            else if (rallyN != null && rallyN == Rallies[0])
                            {
                                Events.PublishOnUIThread(new ResultListControlEvent(rallyN));
                                Manager.ActiveRally = rallyN;
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



        #endregion

        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            Events.Subscribe(this);
            Player1 = Manager.Match.FirstPlayer.Name.Split(' ')[0];
            Player2 = Manager.Match.SecondPlayer.Name.Split(' ')[0];
        }

        #endregion
    }
}
