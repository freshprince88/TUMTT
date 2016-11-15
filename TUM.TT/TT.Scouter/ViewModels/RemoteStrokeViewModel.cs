using Caliburn.Micro;
using System.Collections.ObjectModel;
using TT.Lib.Managers;
using TT.Models;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TT.Lib.Events;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using TT.Lib.Util;
using System.Collections.Generic;

namespace TT.Scouter.ViewModels
{
    public class RemoteStrokeViewModel : Conductor<IScreen>.Collection.OneActive
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
        private IMatchManager MatchManager;
        private ObservableCollection<Stroke> _strokes;
        public ObservableCollection<Stroke> Strokes
        {
            get { return _strokes; }
            set
            {
                if (_strokes != value)
                {
                    _strokes = value;
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange("Strokes");
                    NotifyOfPropertyChange("CurrentRally");
                    NotifyOfPropertyChange("CurrentStroke");

                }
            }
        }
        public IEventAggregator Events { get; set; }

        public Screen SchlagDetail { get; set; }

        private Stroke _stroke;
        public Stroke CurrentStroke
        {
            get { return _stroke; }
            set
            {
                if (_stroke != value)
                {
                    _stroke = value;
                    NotifyOfPropertyChange("CurrentStroke");

                    if (_stroke == null || _stroke.Number == 1)
                    {
                        SchlagDetail = new ServiceDetailViewModel(CurrentStroke, MatchManager, CurrentRally);
                        NotifyOfPropertyChange("SchlagDetail");
                    }
                    else
                    {
                        SchlagDetail = new StrokeDetailViewModel(CurrentStroke, MatchManager, CurrentRally);
                        NotifyOfPropertyChange("SchlagDetail");
                    }

                }
            }
        }

        private Rally _rally;
        public Rally CurrentRally
        {
            get { return _rally; }
            set
            {
                if (_rally != value)
                {
                    _rally = value;
                    Strokes = _rally.Strokes != null ? new ObservableCollection<Stroke>(_rally.Strokes) : new ObservableCollection<Stroke>();
                    CurrentStroke = Strokes.Count > 0 ? Strokes[0] : null;
                    NotifyOfPropertyChange("CurrentRally");
                }
            }
        }
        #endregion

        public RemoteStrokeViewModel(IMatchManager man, Rally r)
        {
            Events = IoC.Get<IEventAggregator>();
            MatchManager = man;
            CurrentRally = r;
            Strokes.CollectionChanged += Strokes_CollectionChanged;

        }

        private void Strokes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CurrentStroke = Strokes.Count > 0 ? Strokes[0] : null;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Events.Subscribe(this);
            ActivateItem(SchlagDetail);
        }
        #region View Methods
        public void NextStroke()
        {
            if (CurrentStroke.Number < CurrentRally.Length)
            {
                CurrentStroke = Strokes[CurrentStroke.Number];
            }
        }

        public void PreviousStroke()
        {
            if (CurrentStroke.Number != 1)
            {
                var idx = CurrentStroke.Number - 1;
                CurrentStroke = Strokes[idx - 1];
            }
        }

        public void FirstStroke()
        {
            CurrentStroke = Strokes[0];
        }

        public void LastStroke()
        {
            if (CurrentRally.Winner == Strokes[Strokes.Count - 1].Player)
            {
                CurrentStroke = Strokes[Strokes.Count - 1];
            }
            else
            {
                CurrentStroke = Strokes[Strokes.Count - 2];
            }
        }
        #endregion
        #region Helper Methods
        public void SetCourses()
        {

        }

        #endregion
    }
}
