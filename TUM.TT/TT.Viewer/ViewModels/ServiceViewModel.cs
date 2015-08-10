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
    public class ServiceViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<MatchOpenedEvent>,
        IHandle<TableViewSelectionChangedEvent>,
        IHandle<SpinControlSelectionChangedEvent>
    {
        public enum EFilterMode
        {
            AND,
            OR
        }

        public EFilterMode FilterMode { get; set; }

        public SpinControlViewModel SpinControl { get; private set; }
        public TableViewModel TableView { get; private set; }
        public List<MatchRally> SelectedRallies { get; private set; }

        public Match Match { get; private set; }
        public List<SpinControlViewModel.Spins> SelectedSpins { get; private set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public ServiceViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedRallies = new List<MatchRally>();
            SelectedSpins = new List<SpinControlViewModel.Spins>();
            Match = new Match();

            SpinControl = new SpinControlViewModel(events);
            TableView = new TableViewModel(events);

            this.FilterMode = EFilterMode.OR;
        }

        public void SwitchTable(bool check)
        {
            if (check)
            {
                TableView.Mode = TableViewModel.ViewMode.Top;
            }
            else
            {
                TableView.Mode = TableViewModel.ViewMode.Bottom;
            }
        }

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

            this.ActivateItem(SpinControl);
            this.ActivateItem(TableView);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            this.DeactivateItem(SpinControl, close);
            this.DeactivateItem(TableView, close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }

        #endregion

        #region Event Handlers

        public void Handle(MatchOpenedEvent message)
        {
            this.Match = message.Match;
            SelectedRallies = this.Match.Rallies.Where(r => r.Schlag.Length > 0).ToList();
            this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
        }

        public void Handle(TableViewSelectionChangedEvent message)
        {
            throw new NotImplementedException();
        }

        public void Handle(SpinControlSelectionChangedEvent message)
        {
            SelectedSpins = message.Selected;
            UpdateSelection();
        }

        #endregion

        #region Helper Methods

        private void UpdateSelection()
        {
            if (this.Match.Rallies != null)
            {
                SelectedRallies = this.Match.Rallies.Where(r => r.Schlag.Length > 0 && HasSpins(r, this.FilterMode)).ToList();
                this.events.PublishOnUIThread(new FilterSelectionChangedEvent(SelectedRallies));
            }
        }

        private bool HasSpins(MatchRally r, EFilterMode mode)
        {
            List<bool> results = new List<bool>();
            MatchRallySchlag service = r.Schlag.Where(s => s.Nummer == "1").FirstOrDefault();

            foreach (var spin in SelectedSpins)
            {
                switch (spin)
                {
                    case SpinControlViewModel.Spins.ÜS:
                        results.Add(service.Spin.ÜS == "1");
                        break;
                    case SpinControlViewModel.Spins.SR:
                        results.Add(service.Spin.SR == "1");
                        break;
                    case SpinControlViewModel.Spins.No:
                        results.Add(service.Spin.No == "1");
                        break;
                    case SpinControlViewModel.Spins.SL:
                        results.Add(service.Spin.SL == "1");
                        break;
                    case SpinControlViewModel.Spins.US:
                        results.Add(service.Spin.US == "1");
                        break;
                    default:
                        break;
                }
            }

            switch (mode)
            {
                case EFilterMode.AND:
                    return results.Count == 0 ? false : results.Aggregate(true, (a, b) => a && b);
                case EFilterMode.OR:
                    return results.Count == 0 ? false : results.Aggregate(false, (a, b) => a || b);
                default:
                    return false;
            }
        }

        #endregion
    }
}
