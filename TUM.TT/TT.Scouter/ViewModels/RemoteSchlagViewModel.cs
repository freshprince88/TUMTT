using Caliburn.Micro;
using System.Collections.ObjectModel;
using TT.Lib.Events;
using TT.Models;
using System;
using TT.Scouter.Util.Model;

namespace TT.Scouter.ViewModels
{
    public class RemoteSchlagViewModel : Conductor<IScreen>.Collection.OneActive
    {

        private ObservableCollection<Schlag> _strokes;
        public ObservableCollection<Schlag> Strokes
        {
            get
            {
                return _strokes;
            }
            set
            {
                _strokes = value;
                Strokes_CollectionChanged();
            }
        }


        private Schlag _stroke;
        public Schlag CurrentStroke
        {
            get { return _stroke; }
            set
            {
                if (_stroke != value)
                {
                    _stroke = value;
                    NotifyOfPropertyChange("CurrentStroke");

                    if(_stroke == null)
                    {
                        this.DeactivateItem(this.ActiveItem, true);
                    }
                    else
                    {
                        if (_stroke.Nummer == 1)
                        {
                            this.ActivateItem(new ServiceDetailViewModel(CurrentStroke));
                        }
                        else
                        {
                            this.ActivateItem(new SchlagDetailViewModel(CurrentStroke));
                        }
                    }
                }
            }
        }

        public RemoteSchlagViewModel(ObservableCollection<Schlag> schläge, Calibration cal)
        {
            Strokes = schläge;
            cal.StrokePositionCalculated += OnStrokePositionCalculated;
        }

        private void Strokes_CollectionChanged()
        {
            CurrentStroke = Strokes.Count > 0 ? Strokes[0] : null;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        public void NextStroke()
        {
            if (CurrentStroke.Nummer < Strokes.Count)
                CurrentStroke = Strokes[CurrentStroke.Nummer];
        }

        public void PreviousStroke()
        {
            var idx = CurrentStroke.Nummer - 1;
            CurrentStroke = Strokes[idx - 1];
        }

        public void OnStrokePositionCalculated(object sender, StrokePositionCalculatedEventArgs args)
        {
            Platzierung newPosition = new Platzierung();
            newPosition.WX = args.Position.X;
            newPosition.WY = args.Position.Y;
            CurrentStroke.Platzierung = newPosition;
            Console.WriteLine(args.Position.X);
            Console.WriteLine(args.Position.Y);
            NextStroke();
        }
    }
}
