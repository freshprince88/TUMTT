using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class TableLegendViewModel : Screen
    {
        private const int MiddleX = 25;
        private const int StrokeLength = 120;

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
            rally.Strokes = CreatePushStrokeList(rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count-2]);
            
            rally = new Rally();
            rally.Number = -4;
            rally.Strokes = CreateChopStrokeList(rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);
            
            rally = new Rally();
            rally.Number = -5;
            rally.Strokes = CreateLobStrokeList(rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);

            rally = new Rally();
            rally.Number = -6;
            rally.Strokes = CreateTopspinStrokeList(rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);

            rally = new Rally();
            rally.Number = -7;
            rally.Strokes = CreateBananaStrokeList(rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);

            rally = new Rally();
            rally.Number = -8;
            rally.Strokes = CreateBlockStrokeList(rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);

            rally = new Rally();
            rally.Number = -9;
            rally.Strokes = CreateSmashStrokeList(rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);

            rally = new Rally();
            rally.Number = -10;
            rally.Strokes = CreateMiscStrokeList(rally);
            Strokes.Add(rally.Strokes[rally.Strokes.Count - 2]);
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
            stroke = CreateStroke(1, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Behind.ToString(), "", rally);
            stroke.Playerposition = double.MinValue;
            strokesTmp.Add(stroke);

            stroke = CreateStroke(2, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Behind.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateSpinServiceStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // service without spin
            stroke = CreateStroke(1, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Behind.ToString(), "", rally);
            stroke.Playerposition = double.MinValue;
            Spin spin = new Spin();
            spin.TS = "1";
            spin.SR = "1";
            stroke.Spin = spin;
            strokesTmp.Add(stroke);

            stroke = CreateStroke(2, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Behind.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreatePushStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // first stroke
            stroke = CreateStroke(1, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // second stroke
            stroke = CreateStroke(2, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // third stroke (push)
            stroke = CreateStroke(3, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(),
                Models.Util.Enums.Stroke.Technique.Push.ToString(), rally);
            strokesTmp.Add(stroke);

            // fourth stroke (for intercept arrow)
            stroke = CreateStroke(4, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateChopStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // first stroke
            stroke = CreateStroke(1, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // second stroke
            stroke = CreateStroke(2, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // third stroke (push)
            stroke = CreateStroke(3, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(),
                Models.Util.Enums.Stroke.Technique.Chop.ToString(), rally);
            strokesTmp.Add(stroke);

            // fourth stroke (for intercept arrow)
            stroke = CreateStroke(4, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateLobStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // first stroke
            stroke = CreateStroke(1, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // second stroke
            stroke = CreateStroke(2, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // third stroke (push)
            stroke = CreateStroke(3, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(),
                Models.Util.Enums.Stroke.Technique.Lob.ToString(), rally);
            strokesTmp.Add(stroke);

            // fourth stroke (for intercept arrow)
            stroke = CreateStroke(4, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateTopspinStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // first stroke
            stroke = CreateStroke(1, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // second stroke
            stroke = CreateStroke(2, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // third stroke (push)
            stroke = CreateStroke(3, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(),
                Models.Util.Enums.Stroke.Technique.Topspin.ToString(), rally);
            strokesTmp.Add(stroke);

            // fourth stroke (for intercept arrow)
            stroke = CreateStroke(4, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateBananaStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // first stroke
            stroke = CreateStroke(1, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // second stroke
            stroke = CreateStroke(2, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // third stroke (banana)
            stroke = CreateStroke(3, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(),
                Models.Util.Enums.Stroke.Technique.Banana.ToString(), rally);
            strokesTmp.Add(stroke);

            // fourth stroke (for intercept arrow)
            stroke = CreateStroke(4, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateBlockStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // first stroke
            stroke = CreateStroke(1, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // second stroke
            stroke = CreateStroke(2, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // third stroke (block)
            stroke = CreateStroke(3, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(),
                Models.Util.Enums.Stroke.Technique.Block.ToString(), rally);
            strokesTmp.Add(stroke);

            // fourth stroke (for intercept arrow)
            stroke = CreateStroke(4, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateSmashStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // first stroke
            stroke = CreateStroke(1, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // second stroke
            stroke = CreateStroke(2, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // third stroke (push)
            stroke = CreateStroke(3, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(),
                Models.Util.Enums.Stroke.Technique.Smash.ToString(), rally);
            strokesTmp.Add(stroke);

            // fourth stroke (for intercept arrow)
            stroke = CreateStroke(4, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        private ObservableCollection<Stroke> CreateMiscStrokeList(Rally rally)
        {
            ObservableCollection<Stroke> strokesTmp = new ObservableCollection<Stroke>();
            Stroke stroke;

            // first stroke
            stroke = CreateStroke(1, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // second stroke
            stroke = CreateStroke(2, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            // third stroke (push)
            stroke = CreateStroke(3, StrokeLength, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(),
                Models.Util.Enums.Stroke.Technique.Miscellaneous.ToString(), rally);
            strokesTmp.Add(stroke);

            // fourth stroke (for intercept arrow)
            stroke = CreateStroke(4, 0, MiddleX, Models.Util.Enums.Stroke.PointOfContact.Over.ToString(), "", rally);
            strokesTmp.Add(stroke);

            return strokesTmp;
        }

        public void MouseLeft()
        {
            TryClose();
        }
    }
}
