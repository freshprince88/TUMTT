using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using Xceed.Wpf.Toolkit;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class SpinControlViewModel : Screen
    {
        private IEventAggregator events;

        public enum Spins
        {
            ÜS,
            SR,
            No,
            SL,
            US
        }

        private HashSet<Spins> _spins;
        public HashSet<Spins> Selected
        {
            get
            {
                return _spins;
            }
            private set 
            {
                _spins = value;
            }
        }

        public SpinControlViewModel(IEventAggregator e)
        {
            events = e;
            Selected = new HashSet<Spins>();
        }


        public void SelectTop(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Spins.ÜS))
                    Selected.Add(Spins.ÜS);
            }
            else
            {
                if (Selected.Contains(Spins.ÜS))
                    Selected.Remove(Spins.ÜS);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectLeft(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Spins.SL))
                    Selected.Add(Spins.SL);
            }
            else
            {
                if (Selected.Contains(Spins.SL))
                    Selected.Remove(Spins.SL);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectMid(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Spins.No))
                    Selected.Add(Spins.No);
            }
            else
            {
                if (Selected.Contains(Spins.No))
                    Selected.Remove(Spins.No);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectRight(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Spins.SR))
                    Selected.Add(Spins.SR);
            }
            else
            {
                if (Selected.Contains(Spins.SR))
                    Selected.Remove(Spins.SR);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectBot(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Spins.US))
                    Selected.Add(Spins.US);
            }
            else
            {
                if (Selected.Contains(Spins.US))
                    Selected.Remove(Spins.US);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

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
            this.events.Subscribe(this);
        }

        /// <summary>
        /// Handles deactivation of this view model.
        /// </summary>
        /// <param name="close">Whether the view model is closed</param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            this.events.Unsubscribe(this);
        }
    }
}
