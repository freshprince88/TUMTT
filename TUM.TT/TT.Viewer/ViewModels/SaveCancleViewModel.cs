using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TT.Lib.Managers;
using TT.Lib.Views;

namespace TT.Viewer.ViewModels
{
    public class SaveCancleViewModel : Conductor<IScreen>.Collection.AllActive
    {
        public IScreen MainView { get; set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        private ISaveCancle parent;

        public SaveCancleViewModel(IEventAggregator eventAggregator, IMatchManager man, ISaveCancle parent, IScreen mainView)
        {
            this.events = eventAggregator;
            this.Manager = man;
            this.parent = parent;

            MainView = mainView;
            this.ActivateItem(mainView);
        }

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        public void Save()
        {
            parent.Save();
        }

        public void Cancle()
        {
            parent.Cancle();
        }
    }
}
