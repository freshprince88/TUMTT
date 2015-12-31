using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Lib.Models;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class BasicFilterStatisticsViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<FilterSwitchedEvent>,
        IHandle<MatchOpenedEvent>
    {
        #region Properties

        public string FilterPointPlayer1Button { get; set; }
        public string FilterPointPlayer2Button { get; set; }

       
        public List<Rally> SelectedRallies { get; private set; }

        public Playlist RallyList { get; private set; }

      
        public EPlayer Player { get; private set; }

        public ECrunch Crunch { get; private set; }

        public HashSet<int> SelectedSets { get; private set; }
        
        public int MinRallyLength { get; set; }
        public bool LastStroke { get; set; }
        public int StrokeNumber { get; set; }

       



        #endregion

        #region Enums



       

        public enum EPlayer

        {
            Player1,
            Player2,
            None,
            Both
        }



        public enum ECrunch
        {
            CrunchTime,
            Not
        }



        #endregion

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public BasicFilterStatisticsViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedRallies = new List<Rally>();
            RallyList = new Playlist();
            Player = EPlayer.None;
            Crunch = ECrunch.Not;
            SelectedSets = new HashSet<int>();
           
            FilterPointPlayer1Button = "Spieler 1";
            FilterPointPlayer2Button = "Spieler 2";
            MinRallyLength = 0;
            
            LastStroke = false;
            StrokeNumber = 0;


        }

        #region View Methods


        public void SetFilter(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("setallbutton"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(1);
                    SelectedSets.Add(2);
                    SelectedSets.Add(3);
                    SelectedSets.Add(4);
                    SelectedSets.Add(5);
                    SelectedSets.Add(6);
                    SelectedSets.Add(7);
                }
                else
                {
                    SelectedSets.Remove(1);
                    SelectedSets.Remove(2);
                    SelectedSets.Remove(3);
                    SelectedSets.Remove(4);
                    SelectedSets.Remove(5);
                    SelectedSets.Remove(6);
                    SelectedSets.Remove(7);
                }
            }
            else if (source.Name.ToLower().Contains("set1button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(1);
                }
                else
                {
                    SelectedSets.Remove(1);
                }
            }
            else if (source.Name.ToLower().Contains("set2button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(2);
                }
                else
                {
                    SelectedSets.Remove(2);
                }
            }
            else if (source.Name.ToLower().Contains("set3button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(3);
                }
                else
                {
                    SelectedSets.Remove(3);
                }
            }
            else if (source.Name.ToLower().Contains("set4button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(4);
                }
                else
                {
                    SelectedSets.Remove(4);
                }
            }
            else if (source.Name.ToLower().Contains("set5button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(5);
                }
                else
                {
                    SelectedSets.Remove(5);
                }
            }
            else if (source.Name.ToLower().Contains("set6button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(6);
                }
                else
                {
                    SelectedSets.Remove(6);
                }
            }
            else if (source.Name.ToLower().Contains("set7button"))
            {
                if (source.IsChecked.Value)
                {
                    SelectedSets.Add(7);
                }
                else
                {
                    SelectedSets.Remove(7);
                }
            }
            UpdateSelection();
        }


        public void CrunchOrNot(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("crunchtime"))
            {
                if (source.IsChecked.Value)
                {
                    Crunch = ECrunch.CrunchTime;
                }
                else
                {
                    Crunch = ECrunch.Not;
                }
            }
            UpdateSelection();
        }


      

        public void P1P2(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("player1"))
            {
                if (source.IsChecked.Value)
                {
                    if (Player == EPlayer.None)
                        Player = EPlayer.Player1;
                    else if (Player == EPlayer.Player2)
                        Player = EPlayer.Both;
                }
                else
                {
                    if (Player == EPlayer.Player1)
                        Player = EPlayer.None;
                    else if (Player == EPlayer.Both)
                        Player = EPlayer.Player2;
                }
            }
            else if (source.Name.ToLower().Contains("player2"))
            {
                if (source.IsChecked.Value)
                {
                    if (Player == EPlayer.None)
                        Player = EPlayer.Player2;
                    else if (Player == EPlayer.Player1)
                        Player = EPlayer.Both;
                }
                else
                {
                    if (Player == EPlayer.Player2)
                        Player = EPlayer.None;
                    else if (Player == EPlayer.Both)
                        Player = EPlayer.Player1;
                }
            }
            UpdateSelection();
        }

        #endregion

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

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }

        #endregion

        #region Event Handlers

        public void Handle(FilterSwitchedEvent message)
        {
            this.RallyList = message.Playlist;

            UpdateSelection();
        }

        public void Handle(MatchOpenedEvent message)
        {
            if (message.Match.FirstPlayer != null && message.Match.SecondPlayer != null)
            {
                FilterPointPlayer1Button = message.Match.FirstPlayer.Name.Split(' ')[0];
                FilterPointPlayer2Button = message.Match.SecondPlayer.Name.Split(' ')[0];
            }
        }

        #endregion

        #region Helper Methods

        public void UpdateSelection()
        {
            if (this.RallyList.Rallies != null)
            {
                SelectedRallies = this.RallyList.Rallies.Where(r => Convert.ToInt32(r.Length) > MinRallyLength && HasSet(r) && HasCrunchTime(r) && HasPlayer(r)).ToList();
                this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
            }
        }


       

        private bool HasPlayer(Rally r)
        {
            switch (this.Player)
            {
                case EPlayer.Player1:
                    return r.Schlag[StrokeNumber].Spieler == "First";  //TODO Name der Spieler dynamisch???? && Letzter Schlag funktioniert so nicht...
                case EPlayer.Player2:
                    return r.Schlag[StrokeNumber].Spieler == "Second"; //TODO Name der Spieler dynamisch???? && Letzter Schlag funktioniert so nicht...
                case EPlayer.None:
                    return true;
                case EPlayer.Both:
                    return true;
                default:
                    return false;
            }
        }


        private bool HasSet(Rally r)
        {
            List<bool> ORresults = new List<bool>();

            foreach (var set in SelectedSets)
            {
                int setTotal = Convert.ToInt32(r.CurrentSetScore.First) + Convert.ToInt32(r.CurrentSetScore.Second) + 1;
                ORresults.Add(setTotal == set);
            }
            return ORresults.Count == 0 ? true : ORresults.Aggregate(false, (a, b) => a || b);
        }

      

        private bool HasCrunchTime(Rally r)
        {
            switch (this.Crunch)
            {
                case ECrunch.CrunchTime:
                    return (Convert.ToInt32(r.CurrentRallyScore.First) + Convert.ToInt32(r.CurrentRallyScore.Second)) >= 16;
                case ECrunch.Not:
                    return true;
                default:
                    return false;
            }
        }

        #endregion
    }
}
