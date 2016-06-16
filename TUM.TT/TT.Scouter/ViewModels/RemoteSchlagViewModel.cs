using Caliburn.Micro;
using System.Collections.ObjectModel;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Scouter.ViewModels
{
    public class RemoteSchlagViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private IMatchManager MatchManager;
        private ObservableCollection<Schlag> _strokes;
        public ObservableCollection<Schlag> Strokes
        {
            get { return _strokes; }
            set
            {
                if(_strokes != value)
                {
                    _strokes = value;
                    CurrentStroke = Strokes.Count > 0 ? Strokes[0] : null;
                    NotifyOfPropertyChange();
                }
            }
        }
        public IEventAggregator Events { get; set; }


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
                            this.ActivateItem(new ServiceDetailViewModel(CurrentStroke, MatchManager));
                        }
                        else
                        {
                            this.ActivateItem(new SchlagDetailViewModel(CurrentStroke, MatchManager));
                        }
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
                    NotifyOfPropertyChange("CurrentRally");
                }
            }
        }

        public RemoteSchlagViewModel(ObservableCollection<Schlag> schläge, IMatchManager man, Rally r)
        {
            
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            MatchManager = man;
            Strokes = schläge;
            Strokes.CollectionChanged += Strokes_CollectionChanged;
            CurrentStroke = Strokes.Count > 0 ? Strokes[0] : null;
            CurrentRally = r;
        }


        //TODO wird nie aufgerufen!!
        private void Strokes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CurrentStroke = Strokes.Count > 0 ? Strokes[0] : null;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        public void NextStroke()
        {
            CurrentStroke = Strokes[CurrentStroke.Nummer];
        }

        public void PreviousStroke()
        {
            var idx = CurrentStroke.Nummer - 1;
            CurrentStroke = Strokes[idx - 1];
        }
        public void FirstStroke()
        {
            CurrentStroke = Strokes[0];
        }
        public void LastStroke()
        {
            if(CurrentRally.Winner == Strokes[Strokes.Count - 1].Spieler) { 
            CurrentStroke = Strokes[Strokes.Count-1];
            }
            else
            {
                CurrentStroke = Strokes[Strokes.Count - 2];
            }
        }
    }
}
