using Caliburn.Micro;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using TT.Lib.Util;
using System.Collections.Generic;

namespace TT.Scouter.ViewModels
{
    public class ServiceDetailViewModel : Conductor<IScreen>.Collection.AllActive
    {
        #region Properties
        /// <summary>
        /// Sets key bindings for ControlWithBindableKeyGestures
        /// </summary>
        public Dictionary<string, KeyGesture> KeyBindings
        {
            get
            {
                //get all method names of this class
                var methodNames = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Select(info => info.Name);

                //get all existing key gestures that match the method names
                var keyGesture = ShortcutFactory.Instance.KeyGestures.Where(pair => methodNames.Contains(pair.Key));

                //return relevant key gestures
                return keyGesture.ToDictionary(x => x.Key, x => (KeyGesture)x.Value); // TODO
            }
            set { }
        }

        private Stroke _stroke;
        public Stroke Stroke {
            get
            {
                return _stroke;
            }
            set
            {
                TableControl.Stroke = value;
                _stroke = value;
            }
        }
        public IEventAggregator Events { get; set; }
        private IMatchManager MatchManager;
        private Rally _rally;
        public Rally CurrentRally
        {
            get { return _rally; }
            set
            {
                if (_rally != value)
                {
                    _rally = value;
                    NotifyOfPropertyChange("CurrentRally");
                }
            }
        }


        public ServicePositionTableViewModel TableControl { get; set; }
        public SpinRadioViewModel SpinControl { get; set; }
        public string PlayerName { get { return GetNameFromStrokePlayer(); } }
        #endregion

        public ServiceDetailViewModel(Stroke s, IMatchManager man, Rally cr)
        {
            Events = IoC.Get<IEventAggregator>();
            MatchManager = man;
            _stroke = s;
            TableControl = new ServicePositionTableViewModel(s, MatchManager);
            SpinControl = new SpinRadioViewModel(MatchManager, this);
            CurrentRally = cr;
            SetCourse();
            if (Stroke.Spin == null)
            {
                Stroke.Spin = new Spin();
                Stroke.Spin.TS = "0";
                Stroke.Spin.SL = "0";
                Stroke.Spin.SR = "0";
                Stroke.Spin.US = "0";
                Stroke.Spin.No = "0";
                Events.PublishOnUIThread(new RalliesStrokesAddedEvent());
            }

            TableControl = new ServicePositionTableViewModel(s, man);
            SpinControl = new SpinRadioViewModel(man, this);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Events.Subscribe(this);
        }


        #region View Methods

        public void SelectForehand ()
        {
            if (Stroke == null)
            {
                Stroke.Side = "";
                return;
            }
            else if (Stroke.Side == "Forehand")
            {
                Stroke.Side = "";
            }
            else 
            {
                Stroke.Side = "Forehand";
            }
        }
        public void SelectBackhand()
        {
            if (Stroke == null)
            {
                Stroke.Side = "";
                return;
            }
            else if (Stroke.Side == "Backhand")
            {
                Stroke.Side = "";
            }
            else
            {
                Stroke.Side = "Backhand";
            }
        }
        public void SelectSide(ToggleButton source)
        {
            if (Stroke == null)
            {
                Stroke.Side = "";
                return;
            }

            if (source.Name.ToLower().Contains("forehand"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Side = "Forehand";
                }
                else
                {
                    Stroke.Side = "";
                }
            }
            else if (source.Name.ToLower().Contains("backhand"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Side = "Backhand";
                }
                else
                {
                    Stroke.Side = "";
                }
            }

        }
        public void SelectSpin(ToggleButton source)
        {

            if (source.Name.ToLower().Equals("tssl"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Spin.TS = "1";
                    Stroke.Spin.SL = "1";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";

                }
                else
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";
                }
            }
            else if (source.Name.ToLower().Equals("ts"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Spin.TS = "1";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";

                }
                else
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";
                }
            }
            else if (source.Name.ToLower().Equals("tssr"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Spin.TS = "1";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "1";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";

                }
                else
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";
                }
            }
            else if (source.Name.ToLower().Equals("sl"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "1";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";

                }
                else
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";
                }
            }
            else if (source.Name.ToLower().Equals("sr"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "1";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";

                }
                else
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";
                }
            }
            else if (source.Name.ToLower().Equals("ussl"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "1";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "1";
                    Stroke.Spin.No = "0";

                }
                else
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";
                }
            }
            else if (source.Name.ToLower().Equals("ussr"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "1";
                    Stroke.Spin.US = "1";
                    Stroke.Spin.No = "0";

                }
                else
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";
                }
            }
            else if (source.Name.ToLower().Equals("us"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "1";
                    Stroke.Spin.No = "0";

                }
                else
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";
                }
            }
            else if (source.Name.ToLower().Equals("no"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "1";

                }
                else
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";
                }
            }
            else if (source.Name.ToLower().Equals("hidden"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Spin.TS = "";
                    Stroke.Spin.SL = "";
                    Stroke.Spin.SR = "";
                    Stroke.Spin.US = "";
                    Stroke.Spin.No = "";

                }
                else
                {
                    Stroke.Spin.TS = "0";
                    Stroke.Spin.SL = "0";
                    Stroke.Spin.SR = "0";
                    Stroke.Spin.US = "0";
                    Stroke.Spin.No = "0";
                }
            }
        }
        public void SelectQuality(ToggleButton source)
        {
            if (Stroke == null)
            {
                Stroke.Quality = "";
                return;
            }

            if (source.Name.ToLower().Contains("good"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Quality = "good";
                }
                else
                {
                    Stroke.Quality = "";
                }
            }
            else if (source.Name.ToLower().Contains("normal"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Quality = "normal";
                }
                else
                {
                    Stroke.Quality = "";
                }
            }
            else if (source.Name.ToLower().Contains("bad"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Quality = "bad";
                }
                else
                {
                    Stroke.Quality = "";
                }
            }

        }


        public void SelectService(ToggleButton source)
        {
            if (Stroke == null)
            {
                Stroke.Servicetechnique = "";
                return;
            }

            if (source.Name.ToLower().Contains("pendulum"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Servicetechnique = "Pendulum";
                }
                else
                {
                    Stroke.Servicetechnique = "";
                }
            }
            else if (source.Name.ToLower().Contains("reverse"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Servicetechnique = "Reverse";
                }
                else
                {
                    Stroke.Servicetechnique = "";
                }
            }
            else if (source.Name.ToLower().Contains("tomahawk"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Servicetechnique = "Tomahawk";
                }
                else
                {
                    Stroke.Servicetechnique = "";
                }
            }
            else if (source.Name.ToLower().Contains("special"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Servicetechnique = "Special";
                }
                else
                {
                    Stroke.Servicetechnique = "";
                }
            }

        }

        public void SelectCourse(ToggleButton source)
        {
            if (Stroke == null)
            {
                Stroke.Course = "";
                return;
            }

            if (source.Name.ToLower().Contains("netout"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Course = "Net/Out";
                }
                else
                {
                    Stroke.Course = "";
                }
            }
            else if (source.Name.ToLower().Contains("continue"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Course = "continue";
                }
                else
                {
                    Stroke.Course = "";
                }
            }
            else if (source.Name.ToLower().Contains("winner"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Course = "Winner";
                }
                else
                {
                    Stroke.Course = "";
                }
            }


        }
        #endregion
        #region Helper Methods
        public void MutualExclusiveToggleButtonClick(Grid parent, ToggleButton tb)
        {
            foreach (ToggleButton btn in parent.FindChildren<ToggleButton>())
            {
                if (btn.Name != tb.Name)
                    btn.IsChecked = false;
            }
        }

        public void SetCourse()
        {
            if (Stroke.Number < CurrentRally.Length)
            {
                Stroke.Course = "continue";
            }
            else if (Stroke.Number == CurrentRally.Length)
            {
                if (Stroke.Player == CurrentRally.Winner)
                {
                    Stroke.Course = "Winner";
                }
                else
                    Stroke.Course = "Net/Out";
            }
            else
                Stroke.Course = "";
        }
        private string GetNameFromStrokePlayer()
        {
            if (Stroke == null)
                return "";

            switch (Stroke.Player)
            {
                case MatchPlayer.First:
                    return MatchManager.Match.FirstPlayer.Name.Split(' ')[0];
                case MatchPlayer.Second:
                    return MatchManager.Match.SecondPlayer.Name.Split(' ')[0];
                case MatchPlayer.None:
                    return "";
                default:
                    return "";
            }
        }
        #endregion
    }
}
