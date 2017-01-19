using Caliburn.Micro;
using System.Collections.ObjectModel;
using TT.Lib.Managers;
using TT.Lib.Events;
using TT.Models;
using System;
using TT.Scouter.Util.Model;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private RemoteViewModel remoteViewModel;
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


        public Stroke CurrentStroke
        {
            get { return remoteViewModel.CurrentStroke; }
            set
            {
                remoteViewModel.CurrentStroke = value;
                NotifyOfPropertyChange("CurrentStroke");

                if (remoteViewModel.CurrentStroke == null || remoteViewModel.CurrentStroke.Number == 1)
                {
                    SchlagDetail = new ServiceDetailViewModel(value, MatchManager, CurrentRally);                       
                    NotifyOfPropertyChange("SchlagDetail");
                }
                else
                {
                    SchlagDetail = new StrokeDetailViewModel(value, MatchManager, CurrentRally);
                    NotifyOfPropertyChange("SchlagDetail");
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
                    NotifyOfPropertyChange("CurrentRally");
                }
            }
        }
        #endregion

        public RemoteStrokeViewModel(RemoteViewModel remoteViewModel, IMatchManager man, Rally r, Calibration cal)
        {
            this.remoteViewModel = remoteViewModel;
            Events = IoC.Get<IEventAggregator>();
            MatchManager = man;
            CurrentRally = r;

            Strokes = r.Strokes;
            cal.StrokePositionCalculated += OnStrokePositionCalculated;
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
            if (CurrentStroke.Number < Strokes.Count)
                CurrentStroke = Strokes[CurrentStroke.Number];
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

        public void OnStrokePositionCalculated(object sender, StrokePositionCalculatedEventArgs args)
        {
            if (!CurrentStroke.Course.Equals("Net/Out"))
            {
                Placement newPosition = new Placement();
                newPosition.WX = args.Position.X;
                newPosition.WY = args.Position.Y;
                CurrentStroke.Placement = newPosition;
                Console.WriteLine(args.Position.X);
                Console.WriteLine(args.Position.Y);
                NextStroke();
            }
        }
    }
}
