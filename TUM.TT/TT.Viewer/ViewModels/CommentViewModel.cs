using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TT.Lib.Events;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class CommentViewModel : Screen, IHandle<VideoPlayEvent>
    {
        private IEventAggregator events;
        private IMatchManager Manager;
        public Rally CurrentRally { get; set; }

        public String Comment
        {
            get
            {
                return CurrentRally != null ? CurrentRally.Kommentar : string.Empty;
            }
            set
            {
                if (value != CurrentRally.Kommentar)
                {
                    CurrentRally.Kommentar = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public CommentViewModel(IEventAggregator e, IMatchManager man)
        {
            events = e;
            Manager = man;
            
        }

        #region Event Handlers
        public void Handle(VideoPlayEvent message)
        {
            CurrentRally = message.Current;
            Comment = CurrentRally.Kommentar;
            NotifyOfPropertyChange("Comment");
        }
        #endregion
        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
        }
        #endregion
        #region Helper Methods

        public void ChangeCommentOnEnter()
        {

        }

        #endregion





    }
}
