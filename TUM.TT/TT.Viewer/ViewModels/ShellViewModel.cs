using Caliburn.Micro;
using System.Windows;
using System.Reflection;


namespace TT.Viewer.ViewModels {
    
    public class ShellViewModel : Conductor<object>.Collection.AllActive, IShell {

        public Screen FilterView { get; private set; }
        public Screen MediaView { get; set; }
        public IEventAggregator Events { get; private set; }

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            Events = eventAggregator;
            this.DisplayName = "TUM.TT";
            FilterView = new FilterViewModel();
            MediaView = new MediaViewModel(Events);
        }

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
            this.ActivateItem(MediaView);
            this.ActivateItem(FilterView);
        }
    }
}
