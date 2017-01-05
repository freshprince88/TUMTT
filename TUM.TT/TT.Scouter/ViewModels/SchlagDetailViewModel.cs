using Caliburn.Micro;
using TT.Models;

namespace TT.Scouter.ViewModels
{
    public class SchlagDetailViewModel : Conductor<IScreen>.Collection.AllActive 
    {
        private Schlag _stroke;
        public Schlag Stroke
        {
            get
            {
                return _stroke;
            }
            set
            {
                _stroke = value;
                TableControl.Stroke = value;
            }
        }

        public StrokePositionTableViewModel TableControl { get; set; }

        public string Title { get { return GetTitleFromStroke(); } }

        public SchlagDetailViewModel(Schlag s)
        {
            _stroke = s;
            TableControl = new StrokePositionTableViewModel(s);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }


        private string GetTitleFromStroke()
        {
            switch (Stroke.Nummer)
            {
                case 2:
                    return "Rückschlag";
                default:
                    return Stroke.Nummer + ". Schlag";
            }
        }
    }
}
