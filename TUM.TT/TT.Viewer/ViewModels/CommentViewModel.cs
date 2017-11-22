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
    public class CommentViewModel : Screen, IHandle<ActiveRallyChangedEvent>
    {
        private IEventAggregator events;
        private IMatchManager Manager;
        public Rally CurrentRally { get; set; }

        public String Comment
        {
            get
            {
                return CurrentRally != null ? CurrentRally.Comment : string.Empty;
            }
            set
            {
                if (value != CurrentRally.Comment)
                {
                    CurrentRally.Comment = value;
                    Manager.MatchModified = true;
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
        public void Handle(ActiveRallyChangedEvent message)
        {
            CurrentRally = message.Current;
            Comment = CurrentRally.Comment;
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

        protected override void OnDeactivate(bool close)
        {
            events.Unsubscribe(this);
            base.OnDeactivate(close);
        }
        #endregion

        #region Helper Methods

        public void ChangeCommentOnEnter()
        {

        }

        #endregion





    }
}
