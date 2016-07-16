using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.Linq;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class TableLegendViewModel : Screen
    {
        public ObservableCollection<Stroke> Strokes { get; set; }

        public TableLegendViewModel()
        {
            Strokes = new ObservableCollection<Stroke>();

            Rally rally;

            rally = new Rally();
            rally.Number = -1;
            rally.Strokes = CreateSpinlessServiceStrokeList(rally);
            Strokes.Add(rally.Strokes.First());

            rally = new Rally();
            rally.Number = -2;
            rally.Strokes = CreateSpinServiceStrokeList(rally);
            Strokes.Add(rally.Strokes.First());

            rally = new Rally();
            rally.Number = -3;
            rally.Strokes = CreateStrokeListForStroketechnique(Models.Util.Enums.Stroke.Technique.Push.ToString(), rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count-2]);
            
            rally = new Rally();
            rally.Number = -4;
            rally.Strokes = CreateStrokeListForStroketechnique(Models.Util.Enums.Stroke.Technique.Chop.ToString(), rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);
            
            rally = new Rally();
            rally.Number = -5;
            rally.Strokes = CreateStrokeListForStroketechnique(Models.Util.Enums.Stroke.Technique.Lob.ToString(), rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);

            rally = new Rally();
            rally.Number = -6;
            rally.Strokes = CreateStrokeListForStroketechnique(Models.Util.Enums.Stroke.Technique.Topspin.ToString(), rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);

            rally = new Rally();
            rally.Number = -7;
            rally.Strokes = CreateStrokeListForStroketechnique(Models.Util.Enums.Stroke.Technique.Flip.ToString(), rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);

            rally = new Rally();
            rally.Number = -8;
            rally.Strokes = CreateStrokeListForStroketechnique(Models.Util.Enums.Stroke.Technique.Block.ToString(), rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);

            rally = new Rally();
            rally.Number = -9;
            rally.Strokes = CreateStrokeListForStroketechnique(Models.Util.Enums.Stroke.Technique.Smash.ToString(), rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);

            rally = new Rally();
            rally.Number = -10;
            rally.Strokes = CreateStrokeListForStroketechnique(Models.Util.Enums.Stroke.Technique.Miscellaneous.ToString(), rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);

            rally = new Rally();
            rally.Number = -11;
            rally.Strokes = CreateForehandStrokeList(rally);
            Strokes.Add(rally.Strokes.First());

            rally = new Rally();
            rally.Number = -12;
            rally.Strokes = CreateBackhandStrokeList(rally);
            Strokes.Add(rally.Strokes.First());
        }

        private Stroke CreateStroke(int number, double x, double y, string poc, string st, Rally rally)
        {
            Stroke stroke = new Stroke();
            stroke.Number = number;
            stroke.Placement = new Placement();
            stroke.Placement.WX = x;
            stroke.Placement.WY = y;
            stroke.PointOfContact = Models.Util.Enums.Stroke.PointOfContact.Over.ToString();
            Stroketechnique stroketechnique = new Stroketechnique();
            stroketechnique.Type = st;
            stroke.Stroketechnique = stroketechnique;
            stroke.Rally = rally;
            return stroke;
        }

        private ObservableCollection<Stroke> CreateSpinlessServiceStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // service without spin
            stroke = CreateStroke(1, 120, 25, Models.Util.Enums.Stroke.PointOfContact.Behind.ToString(), "", rally);
            stroke.Playerposition = double.MinValue;
            strokesTmp.Add(stroke);

            stroke = CreateStroke(2, 0, 25, Models.Util.Enums.Stroke.PointOfContact.Behind.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateSpinServiceStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // service with spin
            stroke = CreateStroke(1, 120, 25, Models.Util.Enums.Stroke.PointOfContact.Behind.ToString(), "", rally);
            stroke.Playerposition = double.MinValue;
            Spin spin = new Spin();
            spin.TS = "1";
            spin.SR = "1";
            stroke.Spin = spin;
            strokesTmp.Add(stroke);

            stroke = CreateStroke(2, 0, 25, Models.Util.Enums.Stroke.PointOfContact.Behind.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateStrokeListForStroketechnique(string stroketechnique, Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // first stroke
            stroke = CreateStroke(1, 120, 25, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // second stroke
            stroke = CreateStroke(2, 0, 25, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // third stroke (actual stroke that will be drawn)
            stroke = CreateStroke(3, 120, 25, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(),
                stroketechnique, rally);
            strokesTmp.Add(stroke);

            // fourth stroke (for intercept arrow)
            stroke = CreateStroke(4, 0, 25, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateForehandStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // forehand service spin
            stroke = CreateStroke(1, 80, 8, "", "", rally);
            stroke.Side = Models.Util.Enums.Stroke.Hand.Forehand.ToString();
            stroke.Playerposition = double.MinValue;
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateBackhandStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // backhand service
            stroke = CreateStroke(1, 80, 8, "", "", rally);
            stroke.Side = Models.Util.Enums.Stroke.Hand.Backhand.ToString();
            stroke.Playerposition = double.MinValue;
            strokesTmp.Add(stroke);            

            return strokesTmp;
        }

        public void MouseLeft()
        {
            TryClose();
        }
    }
}
