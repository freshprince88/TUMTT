using Caliburn.Micro;
using System.Windows;
using System.Reflection;

namespace TT.Viewer.ViewModels {

    public class ShellViewModel : Conductor<object>.Collection.AllActive, IShell {

        public Screen FilterView { get; private set; }
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            Events = eventAggregator;            
        }

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Subscribe ourself to the event bus
            //this.Events.Subscribe(this);

            FilterView = new FilterViewModel(Events);
            ActivateItem(FilterView);
        }
        
            
        protected override void OnViewLoaded(object view)
        {            
            base.OnViewLoaded(view);
        }

        protected override void OnActivate()
        {            
            base.OnActivate();
        }
    }
}