using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using Xceed.Wpf.Toolkit;
using TT.Lib.Events;
using TT.Models.Util.Enums;

namespace TT.Viewer.ViewModels
{
    public class SpinControlViewModel : Screen
    {
        private IEventAggregator events;

        

        private HashSet<Stroke.Spin> _spins;
        public HashSet<Stroke.Spin> Selected
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
            Selected = new HashSet<Stroke.Spin>();
        }


        public void SelectTop(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Stroke.Spin.ÜS))
                    Selected.Add(Stroke.Spin.ÜS);
            }
            else
            {
                if (Selected.Contains(Stroke.Spin.ÜS))
                    Selected.Remove(Stroke.Spin.ÜS);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectLeft(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Stroke.Spin.SL))
                    Selected.Add(Stroke.Spin.SL);
            }
            else
            {
                if (Selected.Contains(Stroke.Spin.SL))
                    Selected.Remove(Stroke.Spin.SL);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectMid(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Stroke.Spin.No))
                    Selected.Add(Stroke.Spin.No);
            }
            else
            {
                if (Selected.Contains(Stroke.Spin.No))
                    Selected.Remove(Stroke.Spin.No);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectRight(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Stroke.Spin.SR))
                    Selected.Add(Stroke.Spin.SR);
            }
            else
            {
                if (Selected.Contains(Stroke.Spin.SR))
                    Selected.Remove(Stroke.Spin.SR);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectBot(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Stroke.Spin.US))
                    Selected.Add(Stroke.Spin.US);
            }
            else
            {
                if (Selected.Contains(Stroke.Spin.US))
                    Selected.Remove(Stroke.Spin.US);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectBotRight(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Stroke.Spin.USSR))
                    Selected.Add(Stroke.Spin.USSR);
            }
            else
            {
                if (Selected.Contains(Stroke.Spin.USSR))
                    Selected.Remove(Stroke.Spin.USSR);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectBotLeft(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Stroke.Spin.USSL))
                    Selected.Add(Stroke.Spin.USSL);
            }
            else
            {
                if (Selected.Contains(Stroke.Spin.USSL))
                    Selected.Remove(Stroke.Spin.USSL);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectTopLeft(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Stroke.Spin.ÜSSL))
                    Selected.Add(Stroke.Spin.ÜSSL);
            }
            else
            {
                if (Selected.Contains(Stroke.Spin.ÜSSL))
                    Selected.Remove(Stroke.Spin.ÜSSL);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectTopRight(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Stroke.Spin.ÜSSR))
                    Selected.Add(Stroke.Spin.ÜSSR);
            }
            else
            {
                if (Selected.Contains(Stroke.Spin.ÜSSR))
                    Selected.Remove(Stroke.Spin.ÜSSR);
            }
            this.events.PublishOnUIThread(new SpinControlSelectionChangedEvent(this.Selected.ToList()));
        }

        public void SelectHidden(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                if (!Selected.Contains(Stroke.Spin.Hidden))
                    Selected.Add(Stroke.Spin.Hidden);
            }
            else
            {
                if (Selected.Contains(Stroke.Spin.Hidden))
                    Selected.Remove(Stroke.Spin.Hidden);
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
