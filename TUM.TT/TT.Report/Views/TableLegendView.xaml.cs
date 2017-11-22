using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TT.Models;

namespace TT.Report.Views
{
    /// <summary>
    /// Interaction logic for TableLegendView.xaml
    /// </summary>
    public partial class TableLegendView : TableView
    {
        public TableLegendView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e) when (e is InvalidOperationException)
            {
                Debug.WriteLine("TableLegendView: InitializeComponent() failed ({0} - {1})", e.GetType().Name, e.Message);
            }
        }

        private Grid currentInnerFieldGrid;
        private Grid currentInnerFieldBehindGrid;
        private Grid currentInnerFieldSpinGrid;

        public override Grid View_InnerFieldBehindGrid
        {
            get
            {
                return currentInnerFieldBehindGrid;
            }
        }

        public override Grid View_InnerFieldGrid
        {
            get
            {
                return currentInnerFieldGrid;
            }
        }

        public override Grid View_InnerFieldSpinGrid
        {
            get
            {
                return currentInnerFieldSpinGrid != null ? currentInnerFieldSpinGrid : IFSG1;
            }
        }

        protected override double GetAdjustedX(Stroke stroke, double oldX)
        {
            return oldX;
        }

        protected override double GetAdjustedY(Stroke stroke, double oldY)
        {
            return oldY;
        }

        protected override Shape GetTopSpinShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            return GetBezierPathShape(X1, Y1, X2, Y2, TopspinCurveRation * X2, 10);
        }

        protected override Shape GetBananaShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            return GetBezierPathShape(X1, Y1, X2, Y2, BananaCurveRation * X2, 10);
        }

        protected override Shape GetLobShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            return GetBezierPathShape(X1, Y1, X2, Y2, LobCurveRation * X2, 0);
        }

        protected override Shape GetChopShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            return GetBezierPathShape(X1, Y1, X2, Y2, ChopCurveRation * X2, 10);
        }

        private Shape GetBezierPathShape(double X1, double Y1, double X2, double Y2, double controlPointX, double controlPointY)
        {
            PathGeometry topSpinGeometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure();

            Point startPoint = new Point(X1, Y1);
            Point endPoint = new Point(X2, Y2);
            Point controlPoint = new Point(controlPointX, controlPointY);

            pathFigure.StartPoint = startPoint;

            QuadraticBezierSegment segment = new QuadraticBezierSegment(controlPoint, endPoint, true);
            pathFigure.Segments.Add(segment);

            topSpinGeometry.Figures.Add(pathFigure);

            Path topSpinPath = new Path();
            topSpinPath.Data = topSpinGeometry;

            return topSpinPath;
        }

        protected override void ProcessStrokes(List<Stroke> strokes)
        {
            //Debug.WriteLine("TableLegendView: {0} strokes to process", strokes.Count);

            foreach (Stroke s in strokes)
            {
                // For the legend, strokes are drawn in separate InnerFieldGrids
                // The model creates rallies with decreasing rally numbers, going from -1, -2, -3, etc.
                // Each rally corresponds to one painted stroke in the legend
                // Before processing each stroke, set the currentInnerField(Behind)Grid so TableView can get it while adding the stroke shapes
                currentInnerFieldGrid = (Grid)FindName("IFG" + (-1) * s.Rally.Number);
                currentInnerFieldBehindGrid = (Grid)FindName("IFBG" + (-1) * s.Rally.Number);
                currentInnerFieldSpinGrid = (Grid)FindName("IFSG" + (-1) * s.Rally.Number);

                StrokeShapes[s] = new List<Shape>();
                AddStrokesDirectionShapes(s);
                AddStrokesArrowtips(s);
                AddInterceptArrows(s);
                AddServiceStrokesSpinShapes(s);
            }
        }

        protected override void AttachEventHandlerToShape(Shape shape, Stroke stroke) { /* not needed for legend */ }
        protected override double GetSecondStrokePrecedingStartY() { /* not needed for legend */ throw new NotImplementedException(); }

        protected override bool ShowDirectionForStroke(Stroke stroke)
        {
            return true;
        }

        protected override bool ShowSpinForStroke(Stroke stroke)
        {
            return true;
        }

        protected override bool ShowInterceptForStroke(Stroke stroke)
        {
            return true;
        }

        public override Grid View_InnerFieldHalfDistanceGrid { get { /* not needed for legend */ throw new NotImplementedException(); }}

    }
}
