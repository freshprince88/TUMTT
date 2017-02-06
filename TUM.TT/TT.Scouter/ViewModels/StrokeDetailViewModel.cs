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
    public class StrokeDetailViewModel : Conductor<IScreen>.Collection.AllActive
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
        public Stroke Stroke
        {
            get { return _stroke; }
            set
            {
                if (_stroke != value)
                {
                    _stroke = value;
                    NotifyOfPropertyChange("Stroke");
                }
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


        public StrokePositionTableViewModel TableControl { get; set; }

        public string Title { get { return GetTitleFromStroke(); } }
        public string PlayerName { get { return GetNameFromStrokePlayer(); } }



        #endregion

        public StrokeDetailViewModel(Stroke s, IMatchManager man, Rally cr)
        {
            Events = IoC.Get<IEventAggregator>();
            MatchManager = man;
            Stroke = s;
            TableControl = new StrokePositionTableViewModel(s, man);
            CurrentRally = cr;
            SetCourse();
            if (Stroke.Stroketechnique == null)
            {
                Stroke.Stroketechnique = new Stroketechnique();
                Stroke.Stroketechnique.Type = "";
                Stroke.Stroketechnique.Option = "";
                Events.PublishOnUIThread(new RalliesStrokesAddedEvent());
            }

        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Events.Subscribe(this);
        }
        #region View Methods
        public void SelectForehand()
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
        public void SelectStroketechnique(ToggleButton source)
        {
            if (source.Name.ToLower().Equals("push"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Type = "Push";
                    Stroke.Stroketechnique.Option = "";
                }
                else
                {
                    Stroke.Stroketechnique.Type = "";
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("pushaggressive"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Option = "aggressive";
                }
                else
                {
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("flip"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Type = "Flip";
                    Stroke.Stroketechnique.Option = "";
                }
                else
                {
                    Stroke.Stroketechnique.Type = "";
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("banana"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Option = "Banana";
                }
                else
                {
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("topspin"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Type = "Topspin";
                    Stroke.Stroketechnique.Option = "";
                }
                else
                {
                    Stroke.Stroketechnique.Type = "";
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("topspinspin"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Option = "Spin";
                }
                else
                {
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("topspintempo"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Option = "Tempo";
                }
                else
                {
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("block"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Type = "Block";
                    Stroke.Stroketechnique.Option = "";
                }
                else
                {
                    Stroke.Stroketechnique.Type = "";
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("blockchop"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Option = "Chop";
                }
                else
                {
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("blocktempo"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Option = "Tempo";
                    Stroke.Stroketechnique.Option = "";
                }
                else
                {
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("chop"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Type = "Chop";
                    Stroke.Stroketechnique.Option = "";
                }
                else
                {
                    Stroke.Stroketechnique.Type = "";
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("lob"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Type = "Lob";
                    Stroke.Stroketechnique.Option = "";
                }
                else
                {
                    Stroke.Stroketechnique.Type = "";
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("smash"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Type = "Smash";
                    Stroke.Stroketechnique.Option = "";
                }
                else
                {
                    Stroke.Stroketechnique.Type = "";
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("counter"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Type = "Counter";
                    Stroke.Stroketechnique.Option = "";
                }
                else
                {
                    Stroke.Stroketechnique.Type = "";
                    Stroke.Stroketechnique.Option = "";
                }
            }
            else if (source.Name.ToLower().Equals("special"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Stroketechnique.Type = "Special";
                    Stroke.Stroketechnique.Option = "";
                }
                else
                {
                    Stroke.Stroketechnique.Type = "";
                    Stroke.Stroketechnique.Option = "";
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
        public void SelectAggressiveness(ToggleButton source)
        {
            if (Stroke == null)
            {
                Stroke.Aggressiveness = "";
                return;
            }

            if (source.Name.ToLower().Contains("aggressive"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Aggressiveness = "aggressive";
                }
                else
                {
                    Stroke.Aggressiveness = "";
                }
            }
            else if (source.Name.ToLower().Contains("passive"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Aggressiveness = "passive";
                }
                else
                {
                    Stroke.Aggressiveness = "";
                }
            }
            else if (source.Name.ToLower().Contains("control"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Aggressiveness = "Control";
                }
                else
                {
                    Stroke.Aggressiveness = "";
                }
            }

        }
        public void SelectSpecials(ToggleButton source)
        {
            if (Stroke == null)
            {
                Stroke.Specials = "";
                return;
            }

            if (source.Name.ToLower().Contains("edgetable"))
            {
                if (source.IsChecked.Value)
                {
                    if (Stroke.Specials == "EdgeNet")
                    {
                        Stroke.Specials = "EdgeNetTable";
                    }
                    else
                    {
                        Stroke.Specials = "EdgeTable";
                    }
                }
                else
                {
                    if (Stroke.Specials == "EdgeTable")
                        Stroke.Specials = "";
                    else if (Stroke.Specials == "EdgeNetTable")
                        Stroke.Specials = "EdgeNet";
                }
            }
            else if (source.Name.ToLower().Contains("edgenet"))
            {
                if (source.IsChecked.Value)
                {


                    if (Stroke.Specials == "EdgeTable")
                    {
                        Stroke.Specials = "EdgeNetTable";
                    }
                    else
                        Stroke.Specials = "EdgeNet";
                }
                else
                {
                    if (Stroke.Specials == "EdgeNet")
                        Stroke.Specials = "";
                    else if (Stroke.Specials == "EdgeNetTable")
                        Stroke.Specials = "EdgeTable";
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

        private string GetTitleFromStroke()
        {
            if (Stroke == null)
                return "";

            switch (Stroke.Number)
            {
                case 2:
                    return "Receive";
                default:
                    return Stroke.Number + ". Stroke";
            }
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
