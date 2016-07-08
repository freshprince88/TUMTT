﻿using Caliburn.Micro;
using System.Collections.ObjectModel;
using TT.Lib.Managers;
using TT.Lib.Events;
using TT.Models;
using System;
using TT.Scouter.Util.Model;

namespace TT.Scouter.ViewModels
{
    public class RemoteStrokeViewModel : Conductor<IScreen>.Collection.OneActive
    {
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
                    Strokes_CollectionChanged();

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

        public RemoteStrokeViewModel(IMatchManager man, Rally r, Calibration cal)
        {
            Events = IoC.Get<IEventAggregator>();
            MatchManager = man;
            CurrentRally = r;

            Strokes = r.Strokes;
            cal.StrokePositionCalculated += OnStrokePositionCalculated;
        }

        private void Strokes_CollectionChanged()
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
            if (CurrentStroke.Number < Strokes.Count)
                CurrentStroke = Strokes[CurrentStroke.Number];
        }

        public void PreviousStroke()
        {
            var idx = CurrentStroke.Number - 1;
            CurrentStroke = Strokes[idx - 1];
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
