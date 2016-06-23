﻿using Caliburn.Micro;
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
using System.Windows.Shapes;

namespace TT.Viewer.ViewModels
{
    public class ResultLargeTableViewModel : Screen, IResultViewTabItem,
        IHandle<ResultsChangedEvent>,
        IHandle<FullscreenEvent>,
        IHandle<MediaControlEvent>
    {
        
        public string Header { get; set; }
        public string Player1 {get; set;}
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

        public ResultLargeTableViewModel()
        {

        }

        public ResultLargeTableViewModel(IEventAggregator e, IDialogCoordinator c, IMatchManager man)
        {
            this.DisplayName = Properties.Resources.table_large_tab_title;
            Header = "Hitlist (" + count + ")";
            Events = e;
            Dialogs = c;
            Manager = man;
            count = 0;
            Player1 = "Spieler 1";
            Player2 = "Spieler 2";
            PointsPlayer1 = 0;
            PointsPlayer2 = 0;
            totalRalliesCount = 0;
            Items = new ObservableCollection<ResultListItem>();
            Rallies = new List<Rally>();
        }

        public byte GetOrderInResultView()
        {
            return 1;
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
            //this.DisplayName = "Großer Tisch (" + count + ")";
            Header = "Hitlist (" + count + ")";
            NotifyOfPropertyChange("Header");

            //this.Items.Refresh();
            var strokes = new List<Schlag>();
            foreach (var r in message.Rallies)
            {
                strokes.Add(r.Schläge.First());
            }
            Events.PublishOnUIThread(new StrokesPaintEvent(strokes));
        }

        public void Handle(FullscreenEvent message)
        {
            switch (message.Fullscreen)
            {
                case true:
                    //this.DisplayName = "GT(" + count + ")";
                    Header = "R(" + count + ")";
                    break;
                case false:
                    //this.DisplayName += "Großer Tisch (" + count + ")";
                    Header = "Hitlist (" + count + ")";
                    break;
                default:
                    break;
            }
        }

        public void Handle(MediaControlEvent message)
        {
            if(message.Source == Media.Source.Viewer)
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
                        var rallyN = idx + 1 < Rallies.Count ? Rallies[idx + 1] : null;
                        if (rallyN != null)
                        {                            
                            Events.PublishOnUIThread(new ResultListControlEvent(rallyN));
                            Manager.ActiveRally = rallyN;
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
            //this.ActivateItem(MiniStatistic);
        }

        #endregion
    }
}