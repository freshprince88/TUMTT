using System.Linq;
using Caliburn.Micro;
using TT.Lib.Managers;
using TT.Models;
using System.Collections.ObjectModel;
using TT.Lib.Interfaces;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using TT.Lib.Util;
using TT.Lib.Results;
using System.Collections.Generic;
using System.Windows.Input;
using System.Reflection;

namespace TT.Scouter.ViewModels
{
    public class LiveScoutingViewModel : Screen
    {
        ///// <summary>
        ///// Sets key bindings for ControlWithBindableKeyGestures
        ///// </summary>
        //public Dictionary<string, KeyBinding> KeyBindings
        //{
        //    get
        //    {
        //        //get all method names of this class
        //        var methodNames = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Select(info => info.Name);

        //        //get all existing key gestures that match the method names
        //        var keyGesture = ShortcutFactory.Instance.KeyGestures.Where(pair => methodNames.Contains(pair.Key));

        //        //return relevant key gestures
        //        return keyGesture.ToDictionary(x => x.Key, x => (KeyBinding)x.Value); // TODO
        //    }
        //    set { }
        //}


        private IEventAggregator Events;
        private IMatchManager MatchManager;
       
        public LiveViewModel LiveView { get; private set; }

        public LiveScoutingViewModel (IEventAggregator ev, IMatchManager man, IMediaPosition mp, LiveViewModel live)
        {
            Events = ev;
            MatchManager = man;
            LiveView = live;
            

        }


        #region View Methods
        public void SetRallyLength(int length)
        {
            LiveView.SetRallyLength(length);

        }

        public void RallyWon(int player)
        {
            LiveView.RallyWon(player);
        }

        public void StartRally()
        {
            LiveView.StartRally();
        }
        public void SetNewStart()
        {
            LiveView.SetNewStart();
        }
        public void SkipForward()
        {
            LiveView.MediaPlayer.SkipForward();
        }
        public void SkipBackwards()
        {
            LiveView.MediaPlayer.SkipBackwards();
        }
        public void Previous()
        {
            LiveView.CurrentScreen = LiveView.ChoiceOfServiceReceive;
            LiveView.ChangeTransitioningContent();
        }

        #endregion
    }
}
