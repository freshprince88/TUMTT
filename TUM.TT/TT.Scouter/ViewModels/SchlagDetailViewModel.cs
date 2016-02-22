using Caliburn.Micro;
using TT.Lib.Models;

namespace TT.Scouter.ViewModels
{
    public class SchlagDetailViewModel : Conductor<IScreen>.Collection.AllActive 
    {
        public Schlag Stroke { get; set; }

        public string Title { get { return GetTitleFromStroke(); } }

        public SchlagDetailViewModel(Schlag s)
        {
            Stroke = s;
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
