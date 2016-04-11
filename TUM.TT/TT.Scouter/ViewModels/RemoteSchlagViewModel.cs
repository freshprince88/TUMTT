using Caliburn.Micro;
using System.Collections.ObjectModel;
using TT.Lib.Models;

namespace TT.Scouter.ViewModels
{
    public class RemoteSchlagViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public ObservableCollection<Schlag> Strokes { get; set; }


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

        public RemoteSchlagViewModel(ObservableCollection<Schlag> schläge)
        {
            Strokes = schläge;
            Strokes.CollectionChanged += Strokes_CollectionChanged;
        }

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
    }
}
