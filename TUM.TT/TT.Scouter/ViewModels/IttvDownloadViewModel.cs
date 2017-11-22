using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TT.Lib;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Lib.Results;
using TT.Lib.Util;
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
        private string _header;
        public string Header
        {
            get
            {
                return _header;
            }
            set
            {
                if (value != _header)
                    _header = value;
                NotifyOfPropertyChange(() => Header);
            }
        }
        private Visibility _secretLabel;
        public Visibility secretLabel
        {
            get
            {
                return _secretLabel;
            }
            set
            {
                _secretLabel = value;
                NotifyOfPropertyChange(() => secretLabel);
            }
        }
        private Visibility _secretTextbox;
        public Visibility secretTextbox
        {
            get
            {
                return _secretTextbox;
            }
            set
            {
                _secretTextbox = value;
                NotifyOfPropertyChange(() => secretTextbox);
            }
        }
        private Visibility _secretDownload;
        public Visibility secretDownload
        {
            get
            {
                return _secretDownload;
            }
            set
            {
                _secretDownload = value;
                NotifyOfPropertyChange(() => secretDownload);
            }
        }
        private Visibility _errorMessageVisible;
        public Visibility errorMessageVisible
        {
            get
            {
                return _errorMessageVisible;
            }
            set
            {
                _errorMessageVisible = value;
                NotifyOfPropertyChange(() => errorMessageVisible);
            }
        }
        private Visibility _headerVisible;
        public Visibility headerVisible
        {
            get
            {
                return _headerVisible;
            }
            set
            {
                _headerVisible = value;
                NotifyOfPropertyChange(() => headerVisible);
            }
        }


        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (value != _password)
                    _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        private string _outputString;
        public string outputString
        {
            get
            {
                return _outputString;
            }
            set
            {
                if (value != _outputString)
                {
                    _outputString = value;
                    NotifyOfPropertyChange(() => outputString);
                }
            }
        }
        private string _inputString;
        public string inputString
        {
            get
            {
                return _inputString;
            }
            set
            {
                if (value != _inputString)
                {
                    _inputString = value;
                    NotifyOfPropertyChange(() => inputString);
                }
            }
        }
        private string _argumentString;
        public string argumentString
        {
            get
            {
                return _argumentString;
            }
            set
            {
                if (value != _argumentString)
                {
                    _argumentString = value;
                    NotifyOfPropertyChange(() => argumentString);
                }
            }
        }

        private string _url;
        public string currentUrl
        {
            get
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

        private string _tour;
        public string Tournament
        {
            get
            {
                return _tour;
            }
            set
            {
                if (value != _tour)
                    _tour = value;
                NotifyOfPropertyChange(() => Tournament);
            }
        }
        private string _year;
        public string Year
        {
            get
            {
                return _year;
            }
            set
            {
                if (value != _year)
                    _year = value;
                NotifyOfPropertyChange(() => Year);
            }
        }


        private MatchRound round = MatchRound.Round;
        public MatchRound Round
        {
            get { return this.round; }
            set
            {
                if (value != round)
                {
                    round = value;
                    NotifyOfPropertyChange(() => Round);
                }
            }
        }
        private MatchCategory competition = MatchCategory.Category;
        public MatchCategory Competition
        {
            get { return this.competition; }
            set
            {
                if (value != competition)
                {
                    competition = value;
                    NotifyOfPropertyChange(() => Competition);
                }
            }
        }




        public StringBuilder sortOutput { get; set; }
        public event DataReceivedEventHandler Completed = delegate { };

        public IttvDownloadViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IMatchManager man, IDialogCoordinator coordinator)
        {
            this.DisplayName = "Competition Details";
            this.events = eventAggregator;
            MatchManager = man;
            _windowManager = windowmanager;
            DialogCoordinator = coordinator;
            Header = "Choose a Video to watch!";
            currentUrl = "";
            Tournament = "Tournament";
            Year = "Year";
            sortOutput = new StringBuilder("");
            secretLabel = Visibility.Visible;
            secretTextbox = Visibility.Collapsed;
            secretDownload = Visibility.Collapsed;
            errorMessageVisible = Visibility.Collapsed;
            headerVisible = Visibility.Visible;
            Password = "";
            

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

        protected override void OnDeactivate(bool close)
        {

            IsClosing = close;
            events.Unsubscribe(this);
        }

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
                return MatchManager.Match != null && MatchManager.Match.FinishedRallies.Any();
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

        public void UnlockTextbox()
        {
            if (secretLabel == Visibility.Visible && secretTextbox==Visibility.Collapsed) { 
            secretLabel = Visibility.Collapsed;
            secretTextbox = Visibility.Visible;
            }
            else
            {
                secretLabel = Visibility.Visible;
                secretTextbox = Visibility.Collapsed;
            }
        }
        public void EnterPassword()
        {
            if (Password == "Waldner")
            {
                secretDownload = Visibility.Visible;
                Header = "You have unlocked the secret Download";
            }
            Password = "";
            UnlockTextbox();

        }

        public bool isCorrectMatchSelected()
        {
            if (currentUrl.Contains(@"http://cdn.laola1.tv/ittf/iframe/player.html?pfad=mp4:CHANNEL1-Seniors"))
            {
                return true;
            }
            else
                return false;
        }

        public string InputString()
        {
            string tempInput = currentUrl;
            // http:// cdn.laola1.tv/ittf/iframe/player.html?pfad=mp4:CHANNEL1-Seniors/2016/ittf_lagos/160521_t1_HANFFOU_Sarah_CMR_OSHONAIKE_Olufunke_NGR_GARNOVA_Tatiana_RUS_ROSSIKHINA_Anna_RUS
            // rtmp://cp77194.edgefcs.net/ondemand/mp4:CHANNEL1-Seniors/2016/wttc_kuala_lumpur/t1/160306_t1_chn_jpn_men_match3\

            tempInput = tempInput.Replace("http://cdn.laola1.tv/ittf/iframe/player.html?pfad=", "rtmp://cp77194.edgefcs.net/ondemand/");

            return tempInput;

        }
        public string OutputStringVideoName()
        {
            string tempOutput = currentUrl;
            tempOutput = tempOutput.Split('/').Last();
            if (Tournament != "Tournament")
            {
                tempOutput = tempOutput + "_" + Tournament;
            }
            if (Year != "Year")
            {
                tempOutput = tempOutput + "_" + Year;
            }
            if (Competition != MatchCategory.Category)
            {
                tempOutput = tempOutput + "_" + Competition.ToString();
            }
            if (Round != MatchRound.Round)
            {
                tempOutput = tempOutput + "_" + Round.ToString();
            }


            // Speicherplatz auswählen lassen
            tempOutput = tempOutput + ".flv";
            return tempOutput;

        }

        public IEnumerable<IResult> DownloadMatch()
        {
            if (isCorrectMatchSelected())
            {
                errorMessageVisible = Visibility.Collapsed;
                headerVisible = Visibility.Visible;
                var dialog = new SaveFileDialogResult()
                {
                    Title = string.Format("Download match..."),
                    Filter = string.Format("{0}|{1}", "Flash Video", "*.flv"),
                    DefaultFileName = OutputStringVideoName(),
                };
                yield return dialog;
                outputString = dialog.Result;

                inputString = InputString();
                argumentString = "/c rtmpdump -r \"" + @inputString + "\" -o \"" + @outputString + "\" -W http://cdn.laola1.tv/ittf/iframe/ittfplayer_v10.swf";

                //Create a Process

                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = argumentString;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                //process.WaitForExit();
            }
            else
            {
                errorMessageVisible = Visibility.Visible;
                headerVisible = Visibility.Collapsed;
                // Webbrowser allways on Top....no Dialog possible

                //var errorDialog = new ErrorMessageResult()
                //{
                //    Title = "Keine Video ausgewählt!",
                //    Message = "Bitte wählen sie ein korrektes Match aus!",
                //    Dialogs = DialogCoordinator
                //};
                //yield return errorDialog;
            }
        }



        #endregion

    }
}
