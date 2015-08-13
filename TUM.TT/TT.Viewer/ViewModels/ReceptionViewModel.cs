using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Models;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class ReceptionViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<TableViewSelectionChangedEvent>,
        IHandle<FilterSwitchedEvent>
    {
     
        public TableServiceViewModel TableView { get; set; }
        public List<MatchRally> SelectedRallies { get; private set; }
        public Match Match { get; private set; }
        public EHand Hand { get; private set; }
        public HashSet<int> SelectedSets { get; private set; }

        public enum EHand
        {
            Fore,
            Back,
            None,
            Both
        }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public ReceptionViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedRallies = new List<MatchRally>();
            Match = new Match();
            Hand = EHand.None;
            SelectedSets = new HashSet<int>();
            TableView = new TableServiceViewModel(this.events);
        }


        #region View Methods
        public void SwitchTable(bool check)
        {
            if (check)
            {
                TableView.Mode = TableServiceViewModel.ViewMode.Top;
            }
            else
            {
                TableView.Mode = TableServiceViewModel.ViewMode.Bottom;
            }
        }
        public void SetFilter(int set, bool isChecked)
        {
            if (set == 0)
            {
                if (!isChecked)
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
            else if (set == 1)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(1);
                }
                else
                {
                    SelectedSets.Remove(1);
                }
            }
            else if (set == 2)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(2);
                }
                else
                {
                    SelectedSets.Remove(2);
                }
            }
            else if (set == 3)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(3);
                }
                else
                {
                    SelectedSets.Remove(3);
                }
            }
            else if (set == 4)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(4);
                }
                else
                {
                    SelectedSets.Remove(4);
                }
            }
            else if (set == 5)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(5);
                }
                else
                {
                    SelectedSets.Remove(5);
                }
            }
            else if (set == 6)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(6);
                }
                else
                {
                    SelectedSets.Remove(6);
                }
            }
            else if (set == 7)
            {
                if (!isChecked)
                {
                    SelectedSets.Add(7);
                }
                else
                {
                    SelectedSets.Remove(7);
                }
            }
        }

        public void ForBackHand(string hand, bool isChecked)
        {
            if (hand == "fore")
            {
                if (!isChecked)
                {
                    if (Hand == EHand.None)
                        Hand = EHand.Fore;
                    else if (Hand == EHand.Back)
                        Hand = EHand.Both;
                }
                else
                {
                    if (Hand == EHand.Fore)
                        Hand = EHand.None;
                    else if (Hand == EHand.Both)
                        Hand = EHand.Back;
                }
            }
            else if (hand == "back")
            {
                if (!isChecked)
                {
                    if (Hand == EHand.None)
                        Hand = EHand.Back;
                    else if (Hand == EHand.Fore)
                        Hand = EHand.Both;
                }
                else
                {
                    if (Hand == EHand.Back)
                        Hand = EHand.None;
                    else if (Hand == EHand.Both)
                        Hand = EHand.Fore;
                }
            }
        }



        #endregion


        #region Caliburn Hooks
        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Subscribe ourself to the event bus
            //this.events.Subscribe(this);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            this.ActivateItem(TableView);
        }
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);            
            this.DeactivateItem(TableView, close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }
        #endregion

        #region Event Handlers
        public void Handle(FilterSwitchedEvent message)
        {
            this.Match = message.Match;
            SelectedRallies = this.Match.Rallies.Where(r => r.Schlag.Length > 0).ToList();
            this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
        }

        public void Handle(TableViewSelectionChangedEvent message)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helper Methods

        private void UpdateSelection()
        {
            if (this.Match.Rallies != null)
            {
                SelectedRallies = this.Match.Rallies.Where(r => Convert.ToInt32(r.Length) > 0 && HasHand(r) && HasSet(r)).ToList();
                this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
            }
        }

        private bool HasHand(MatchRally r)
        {
            switch (this.Hand)
            {
                case EHand.Fore:
                    return r.Schlag[0].Schlägerseite == "Vorhand";
                case EHand.Back:
                    return r.Schlag[0].Schlägerseite == "Rückhand";
                case EHand.None:
                    return true;
                case EHand.Both:
                    return true;
                default:
                    return false;
            }
        }

        private bool HasSet(MatchRally r)
        {
            List<bool> ORresults = new List<bool>();

            foreach (var set in SelectedSets)
            {
                int setTotal = Convert.ToInt32(r.CurrentSetScore.First) + Convert.ToInt32(r.CurrentSetScore.Second) + 1;
                ORresults.Add(setTotal == set);
            }
            return ORresults.Count == 0 ? false : ORresults.Aggregate(false, (a, b) => a || b);
        }

        #endregion
    }
}
