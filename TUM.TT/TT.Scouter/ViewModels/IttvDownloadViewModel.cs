using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TT.Lib;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Lib.Results;
using TT.Models;

namespace TT.Scouter.ViewModels
{
    public class IttvDownloadViewModel : Conductor<IScreen>.Collection.OneActive, IShell, INotifyPropertyChangedEx, IHandle<WebBrowserEvent>
    {
        public IEventAggregator events { get; private set; }
        public IMatchManager MatchManager { get; set; }
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;
        public Match Match { get { return MatchManager.Match; } }
        private string _url;
        public string currentUrl
        { get
            {
                return _url;
            }
            set
            {
                _url = value;
                NotifyOfPropertyChange(() => currentUrl);
            }
        }
        private bool _isClosing;
        public bool IsClosing
        {
            get { return _isClosing; }
            set
            {
                _isClosing = value;
                NotifyOfPropertyChange(() => IsClosing);
            }
        }




        public IttvDownloadViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IMatchManager man, IDialogCoordinator coordinator)
        {
            this.DisplayName = "Competition Details";
            this.events = eventAggregator;
            MatchManager = man;
            _windowManager = windowmanager;
            DialogCoordinator = coordinator;
            currentUrl = "";

        }
        /// <summary>
        /// Set MatchModified=true, if match informations are modified
        /// </summary>

        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnActivate()
        {

            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);

        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
        }

        protected override async void OnDeactivate(bool close)
        {

            //if (MatchManager.MatchModified)
            //{
            //    var mySettings = new MetroDialogSettings()
            //    {
            //        AffirmativeButtonText = "Save and Close",
            //        NegativeButtonText = "Cancel",
            //        FirstAuxiliaryButtonText = "Close Without Saving",
            //        AnimateShow = true,
            //        AnimateHide = false
            //    };

            //    var result = await DialogCoordinator.ShowMessageAsync(this, "Close Window?",
            //        "You didn't save your changes?",
            //        MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

            //    bool _shutdown = result == MessageDialogResult.Affirmative;

            //    if (_shutdown)
            //    {
            //        Coroutine.BeginExecute(MatchManager.SaveMatch().GetEnumerator(), new CoroutineExecutionContext() { View = this.GetView() });
            //        Application.Current.Shutdown();
            //    }
            //}
            IsClosing = close;
            events.Unsubscribe(this);
        }

        ///// <summary>
        ///// Determines whether the view model can be closed.
        ///// </summary>
        ///// <param name="callback">Called to perform the closing</param>
        //public override void CanClose(System.Action<bool> callback)
        //{
        //    var context = new CoroutineExecutionContext()
        //    {
        //        Target = this,
        //        View = this.GetView() as DependencyObject,
        //    };

        //    Coroutine.BeginExecute(
        //        this.PrepareClose().GetEnumerator(),
        //        context,
        //        (sender, args) =>
        //        {
        //            callback(args.WasCancelled != true);
        //        });
        //}

        ///// <summary>
        ///// Prepare the closing of this view model.
        ///// </summary>
        ///// <returns>The actions to execute before closing</returns>
        //private IEnumerable<IResult> PrepareClose()
        //{

        //    if (MatchManager.MatchModified)
        //    {



        //        var question = new YesNoCloseQuestionResult()
        //        {
        //            Title = "Save the Changes?",
        //            Question = "The Player Informations are modified. Save changes?",
        //            AllowCancel = true,

        //        };
        //        yield return question;

        //        var playlist = MatchManager.Match.Playlists.Where(p => p.Name == "Alle").FirstOrDefault();
        //        var lastRally = playlist.Rallies.LastOrDefault();
        //        //TODO
        //        if (playlist.Rallies.Any())
        //        {
        //            if (lastRally.Winner == MatchPlayer.None)
        //                playlist.Rallies.Remove(lastRally);
        //        }

        //        if (question.Result)
        //        {
        //            foreach (var action in MatchManager.SaveMatch())
        //            {
        //                yield return action;
        //            }
        //        }
        //    }
        //}

        #endregion
        #region View Methods




        #endregion

        #region Events

        public void Handle(WebBrowserEvent message)
        {
            currentUrl = message.CurrentUrl;
        }


        #endregion
        #region Helper Methods
        /// <summary>
        /// Gets a value indicating whether Match can be saved.
        /// </summary>

        public bool CanSaveMatch
        {
            get
            {
                return MatchManager.Match != null && MatchManager.Match.DefaultPlaylist.FinishedRallies.Any();
            }
        }

        public IEnumerable<IResult> SaveMatch()
        {
            if (MatchManager.MatchModified)
            {
                foreach (var action in MatchManager.SaveMatch())
                {
                    yield return action;
                }
            }
        }

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name) ? System.Windows.Application.Current.Windows.OfType<T>().Any() : System.Windows.Application.Current.Windows.OfType<T>().Any(wde => wde.Name.Equals(name));
        }

        public void Test()
        {
            //System.Diagnostics.Process process = new System.Diagnostics.Process();

            //process.StartInfo.WorkingDirectory = @"C:\Users\Michael Fuchs\Desktop\rtmpdump - 2.3";
            //process.StartInfo.FileName = "rtmpdump.exe";
            //process.StartInfo.Arguments = arg;
            //process.Start();




            Process.Start("CMD.exe");




            //string arg = "rtmpdump -r \"rtmp://cp77194.edgefcs.net/ondemand/mp4:CHANNEL1-Seniors/2016/wttc_kuala_lumpur/t1/160306_t1_chn_jpn_men_match3\" -o \"C:\\Users\\Michael Fuchs\\Desktop\\160306_OSHIMA_Yuya_JPN_ZHANG_Jike_CHN_WTTTC_2016_MT_Final.flv\" -W http://cdn.laola1.tv/ittf/iframe/ittfplayer_v10.swf \" ";
            //Process p = new Process();
            //p.StartInfo.FileName = "cmd.exe";
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.RedirectStandardInput = true;
            //p.Start();
            //p.StandardInput.WriteLine("cd C:\\Users\\Michael Fuchs\\Desktop\\rtmpdump-2.3");
            //p.StandardInput.WriteLine(arg);
        }




        #endregion

    }
}
