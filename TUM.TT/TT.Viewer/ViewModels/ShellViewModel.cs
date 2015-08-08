using Caliburn.Micro;
using System.Windows;
using System.Reflection;

namespace TT.Viewer.ViewModels {

    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell {

        public FilterViewModel FilterView { get; private set; }
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            Events = eventAggregator;
            FilterView = new FilterViewModel(Events);
        }

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Subscribe ourself to the event bus
            //this.Events.Subscribe(this);        
        }
        
            
        protected override void OnViewLoaded(object view)
        {            
            base.OnViewLoaded(view);
        }

        protected override void OnActivate()
        {            
            base.OnActivate();
            this.ActivateItem(FilterView);
        }
    }
}