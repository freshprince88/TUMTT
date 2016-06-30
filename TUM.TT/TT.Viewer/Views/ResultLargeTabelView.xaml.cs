using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;
using TT.Lib.Events;
using System;
using TT.Models;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ResultLargeTableView.xaml
    /// </summary>
    public partial class ResultLargeTableView : UserControl,
        IHandle<ResultListControlEvent>,
        IHandle<FullscreenEvent>,
        IHandle<StrokesPaintEvent>
    {
        private double halfFieldWidth;
        private double halfFieldHeight;
        private double tableMarginFieldLeft;
        private double tableMarginFieldBottom;

        private const double STROKE_THICKNESS = 1.75;
        private const double STROKE_THICKNESS_HOVER = 2.5;
        private const double SPIN_STROKE_THICKNESS = 1;
        private const double SPIN_STROKE_THICKNESS_HOVER = 2;
        private const double STROKE_THICKNESS_SMASH = 2;
        private const double STROKE_THICKNESS_SMASH_HOVER = 3;

        private const string TAG_SPIN_ARROW = "spinarrow";
        private const string TAG_ARROW_TIP = "arrowtip";

        private const string STROKE_ATTR_SIDE_FOREHAND = "Forehand";
        private const string STROKE_ATTR_SIDE_BACKHAND = "Backhand";
        private const string STROKE_ATTR_HAS_SPIN = "has_spin";
        private const string STROKE_ATTR_TECHNIQUE_PUSH = "Push";
        private const string STROKE_ATTR_TECHNIQUE_FLIP = "Flip";
        private const string STROKE_ATTR_TECHNIQUE_OPTION_BANANA = "Banana";
        private const string STROKE_ATTR_SMASH = "Smash";
        private const string STROKE_ATTR_COUNTER = "Counter";
        private const string STROKE_ATTR_CHOP = "Chop";
        private const string STROKE_ATTR_POC_OVER = "over";
        private const string STROKE_ATTR_POC_BEHIND = "behind";

        private Dictionary<Stroke, List<Shape>> strokeShapes;
        private int strokeNumber;

        public IEventAggregator Events { get; private set; }

        public ResultLargeTableView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);

            strokeShapes = new Dictionary<Stroke, List<Shape>>();
        }


        public void Handle(ResultListControlEvent msg)
        {
            
        }
        public void Handle(FullscreenEvent message)
        {
            switch (message.Fullscreen)
            {
                case true:
                case false:
                    break;
                default:
                    break;
            }
        }

        private void Items_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void CheckSpin_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
                AddServiceStrokesSpinArrows(new List<Stroke>(strokeShapes.Keys));
            else
                RemoveServiceStrokesSpinArrows();
        }

        private void CheckDirection_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
                AddStrokesDirectionLines(new List<Stroke>(strokeShapes.Keys), strokeNumber == 1);
            else
                RemoveStrokesDirectionLines();
        }

        private void Stroke_MouseEnter(Object sender, MouseEventArgs e)
        {
            Debug.WriteLine("mouse enter on stroke of rally {0}", ((sender as Shape).DataContext as Stroke).Rally.Number);
            Shape shape = sender as Shape;
            Stroke stroke = shape.DataContext as Stroke;

            foreach (var strokeShape in strokeShapes)
            {
                foreach (var s in strokeShape.Value)
                {
                    if (strokeShape.Key.Equals(stroke))
                    {
                        if (TAG_SPIN_ARROW.Equals(s.Tag))
                        {
                            s.StrokeThickness = SPIN_STROKE_THICKNESS_HOVER;
                        }
                        else
                        {
                            if ("Smash".Equals(stroke.Stroketechnique))
                            {
                                s.StrokeThickness = STROKE_THICKNESS_SMASH_HOVER;
                            }
                            else
                                s.StrokeThickness = STROKE_THICKNESS_HOVER;
                        }
                    }
                    else
                    {
                        s.Opacity = 0.1;
                    }
                }
            }
        }
        
        private void Stroke_MouseLeave(Object sender, MouseEventArgs e)
        {
            Debug.WriteLine("mouse leave on stroke of rally {0}", ((sender as Shape).DataContext as Stroke).Rally.Number);
            Shape shape = sender as Shape;
            Stroke stroke = shape.DataContext as Stroke;

            foreach (var strokeShape in strokeShapes)
            {
                foreach (var s in strokeShape.Value)
                {
                    if (strokeShape.Key.Equals(stroke))
                    {
                        if (TAG_SPIN_ARROW.Equals(s.Tag))
                        {
                            s.StrokeThickness = SPIN_STROKE_THICKNESS;
                        }
                        else
                        {
                            if ("Smash".Equals(stroke.Stroketechnique))
                            {
                                s.StrokeThickness = STROKE_THICKNESS_SMASH;
                            }
                            else
                                s.StrokeThickness = STROKE_THICKNESS;
                        }
                    }
                    else
                    {
                        s.Opacity = 1;
                    }
                }
            }
        }

        public void Handle(StrokesPaintEvent message)
        {
            InnerFieldGrid.Children.Clear();
            InnerFieldSpinGrid.Children.Clear();
            strokeShapes.Clear();

            if (message.Strokes == null)
                return;

            strokeNumber = message.StrokeNumber;
            message.Strokes.ForEach(s => { strokeShapes[s] = new List<Shape>(); });

            halfFieldWidth = InnerFieldGrid.ActualWidth / 2;
            halfFieldHeight = InnerFieldGrid.ActualHeight / 2;
            tableMarginFieldLeft = TableBorder.Margin.Left - InnerFieldGrid.Margin.Left;
            tableMarginFieldBottom = TableBorder.Margin.Bottom - InnerFieldGrid.Margin.Bottom;

            switch (strokeNumber)
            {
                case 1:                    
                    AddStrokesArrowtips(message.Strokes, true);
                    if (CheckDirection.IsChecked.Value)
                        AddStrokesDirectionLines(message.Strokes, true);
                    if (CheckSpin.IsChecked.Value)
                        AddServiceStrokesSpinArrows(message.Strokes);
                    break;
                default:
                    AddStrokesArrowtips(message.Strokes, false);
                    if (CheckDirection.IsChecked.Value)
                        AddStrokesDirectionLines(message.Strokes, false);
                    break;
            }
        }

        private void AddStrokesDirectionLines(List<Stroke> strokes, bool isServiceStroke)
        {
            Debug.WriteLine("Strokes to paint: {0}", strokes.Count);
            foreach (var stroke in strokes)
            {
                if (PlacementValuesValid(stroke.Placement))
                {
                    Stroke precedingStroke = null;
                    Line strokeLine = new Line();

                    double X1, X2, Y1, Y2;

                    if (isServiceStroke)
                    {
                        X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : GetAdjustedX(stroke, stroke.Playerposition);
                        Y1 = InnerFieldGrid.ActualHeight; // start lines at the bottom
                    }
                    else
                    {
                        precedingStroke = stroke.Rally.Strokes[stroke.Number - 2];
                        X1 = GetAdjustedX(stroke, precedingStroke.Placement.WX);
                        Y1 = GetAdjustedY(stroke, precedingStroke.Placement.WY);
                    }

                    X2 = GetAdjustedX(stroke, stroke.Placement.WX);
                    Y2 = GetAdjustedY(stroke, stroke.Placement.WY);

                    strokeLine.X1 = X1; strokeLine.Y1 = Y1; strokeLine.X2 = X2; strokeLine.Y2 = Y2;

                    if (!isServiceStroke && STROKE_ATTR_POC_BEHIND.Equals(stroke.PointOfContact))
                    {
                        Line interceptBehindLine = new Line();
                        interceptBehindLine.X1 = X2 - (X2 - X1) * ((InnerFieldGrid.ActualHeight - 10 - Y2) / (Y1 - Y2));
                        interceptBehindLine.Y1 = InnerFieldGrid.ActualHeight - 10;

                        interceptBehindLine.X2 = X1;
                        interceptBehindLine.Y2 = Y1;

                        interceptBehindLine.Stroke = Brushes.Black;
                        interceptBehindLine.Fill = Brushes.Black;
                        interceptBehindLine.StrokeThickness = STROKE_THICKNESS;

                        DoubleCollection dashes = new DoubleCollection();
                        dashes.Add(0.5);
                        interceptBehindLine.StrokeDashArray = dashes;

                        interceptBehindLine.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
                        interceptBehindLine.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
                        interceptBehindLine.DataContext = stroke;
                        Message.SetAttach(interceptBehindLine, "StrokeSelected($DataContext)");

                        if (!strokeShapes[stroke].Contains(interceptBehindLine))
                            strokeShapes[stroke].Add(interceptBehindLine);

                        InnerFieldGrid.Children.Add(interceptBehindLine);
                    }

                    Debug.WriteLine("stroke rally={4} x1={0} y1={1} x2={2} y2={3}", strokeLine.X1, strokeLine.Y1, strokeLine.X2, strokeLine.Y2, stroke.Rally.Number);

                    strokeLine.StrokeThickness = STROKE_THICKNESS;
                    strokeLine.Stroke = Brushes.Black;
                    strokeLine.Fill = Brushes.Black;

                    if (isServiceStroke)
                    {
                        if (STROKE_ATTR_SIDE_BACKHAND.Equals(stroke.Side))
                            ApplyStyle(strokeLine, STROKE_ATTR_SIDE_BACKHAND);

                        if (stroke.Spin != null && !"1".Equals(stroke.Spin.No))
                        {
                            ApplyStyle(strokeLine, STROKE_ATTR_HAS_SPIN);
                        }
                    }
                    else
                    {
                        if (STROKE_ATTR_TECHNIQUE_PUSH.Equals(stroke.Stroketechnique.Type) || STROKE_ATTR_CHOP.Equals(stroke.Stroketechnique.Type))
                        {
                            ApplyStyle(strokeLine, STROKE_ATTR_TECHNIQUE_PUSH);
                        }
                        else if (STROKE_ATTR_TECHNIQUE_FLIP.Equals(stroke.Stroketechnique.Type) || STROKE_ATTR_TECHNIQUE_OPTION_BANANA.Equals(stroke.Stroketechnique.Option))
                        {
                            ApplyStyle(strokeLine, STROKE_ATTR_TECHNIQUE_FLIP);
                        }
                        else if (STROKE_ATTR_SMASH.Equals(stroke.Stroketechnique.Type))
                        {
                            ApplyStyle(strokeLine, STROKE_ATTR_SMASH);
                        }
                    }

                    strokeLine.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
                    strokeLine.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
                    strokeLine.DataContext = stroke;
                    Message.SetAttach(strokeLine, "StrokeSelected($DataContext)");

                    if (!strokeShapes[stroke].Contains(strokeLine))
                        strokeShapes[stroke].Add(strokeLine);

                    InnerFieldGrid.Children.Add(strokeLine);
                }
                else
                {
                    Debug.WriteLine("invalid Placement of stroke {0}: x={1} y={2}", stroke, stroke.Placement.WX, stroke.Placement.WY);
                }
            }
        }

        private void AddStrokesArrowtips(List<Stroke> strokes, bool isServiceStroke)
        {
            foreach (var stroke in strokes)
            {
                if (PlacementValuesValid(stroke.Placement))
                {
                    double X1, X2, Y1, Y2;

                    if (isServiceStroke)
                    {
                        X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : GetAdjustedX(stroke, stroke.Playerposition);
                        Y1 = InnerFieldGrid.ActualHeight; // start lines at the bottom
                    }
                    else
                    {
                        var precedingStroke = stroke.Rally.Strokes[stroke.Number - 2];                        
                        X1 = GetAdjustedX(stroke, precedingStroke.Placement.WX);
                        Y1 = GetAdjustedY(stroke, precedingStroke.Placement.WY);
                    }

                    X2 = GetAdjustedX(stroke, stroke.Placement.WX);
                    Y2 = GetAdjustedY(stroke, stroke.Placement.WY);

                    PathGeometry arrowTipGeometry = new PathGeometry();

                    PathFigure pathFigure = new PathFigure();
                    pathFigure.IsClosed = true;
                    pathFigure.StartPoint = new Point(X2 - 3, Y2);

                    LineSegment ltt = new LineSegment(new Point(X2, Y2 + 3), true);
                    pathFigure.Segments.Add(ltt);

                    LineSegment ttr = new LineSegment(new Point(X2 + 3, Y2), true);
                    pathFigure.Segments.Add(ttr);

                    double theta = Math.Atan2((Y2 - Y1), (X2 - X1)) * 180 / Math.PI;
                    RotateTransform transform = new RotateTransform();
                    transform.Angle = theta - 90;
                    transform.CenterX = X2;
                    transform.CenterY = Y2;
                    arrowTipGeometry.Transform = transform;

                    arrowTipGeometry.Figures.Add(pathFigure);

                    Path strokeArrowTip = new Path();
                    strokeArrowTip.Data = arrowTipGeometry;
                    strokeArrowTip.Tag = TAG_ARROW_TIP;
                    strokeArrowTip.Stroke = Brushes.Black;
                    strokeArrowTip.Fill = Brushes.Black;
                    strokeArrowTip.StrokeThickness = STROKE_THICKNESS;

                    if (isServiceStroke)
                    {
                        if (stroke.Spin != null && !"1".Equals(stroke.Spin.No))
                        {
                            ApplyStyle(strokeArrowTip, STROKE_ATTR_HAS_SPIN);
                        }
                    }
                    else
                    {
                        if (STROKE_ATTR_TECHNIQUE_PUSH.Equals(stroke.Stroketechnique.Type) || STROKE_ATTR_CHOP.Equals(stroke.Stroketechnique.Type))
                        {
                            ApplyStyle(strokeArrowTip, STROKE_ATTR_TECHNIQUE_PUSH);                        
                        }
                        else if (STROKE_ATTR_TECHNIQUE_FLIP.Equals(stroke.Stroketechnique.Type) || STROKE_ATTR_TECHNIQUE_OPTION_BANANA.Equals(stroke.Stroketechnique.Option))
                        {
                            ApplyStyle(strokeArrowTip, STROKE_ATTR_TECHNIQUE_FLIP);
                        }
                        else if (STROKE_ATTR_SMASH.Equals(stroke.Stroketechnique.Type))
                        {
                            ApplyStyle(strokeArrowTip, STROKE_ATTR_SMASH);
                        }
                    }

                    strokeArrowTip.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
                    strokeArrowTip.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
                    strokeArrowTip.DataContext = stroke;
                    Message.SetAttach(strokeArrowTip, "StrokeSelected($DataContext)");

                    if (!strokeShapes[stroke].Contains(strokeArrowTip))
                        strokeShapes[stroke].Add(strokeArrowTip);

                    InnerFieldGrid.Children.Add(strokeArrowTip);
                }
                else
                {
                    Debug.WriteLine("invalid Placement of stroke {0}: x={1} y={2}", stroke, stroke.Placement.WX, stroke.Placement.WY);
                }
            }
        }

        private void AddServiceStrokesSpinArrows(List<Stroke> strokes)
        {
            Random rnd = new Random();
            foreach (var stroke in strokes)
            {
                if (PlacementValuesValid(stroke.Placement))
                {
                    if (stroke.Spin == null || "1".Equals(stroke.Spin.No))
                        continue;

                    double X1, Y1;

                    X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : GetAdjustedX(stroke, stroke.Playerposition);
                    Y1 = InnerFieldSpinGrid.ActualHeight - 1;

                    PathGeometry arrowTipGeometry = new PathGeometry();

                    RotateTransform transform = new RotateTransform();
                    transform.Angle = GetRotationAngleForSpin(stroke.Spin);
                    transform.CenterX = X1;
                    transform.CenterY = Y1 - 4;
                    arrowTipGeometry.Transform = transform;

                    PathFigure pathFigure = new PathFigure();
                    pathFigure.StartPoint = new Point(X1 - 3, Y1 - 5);

                    LineSegment btt = new LineSegment(new Point(X1, Y1 - 8), true);
                    pathFigure.Segments.Add(btt);

                    LineSegment ttl = new LineSegment(new Point(X1 + 3, Y1 - 5), true);
                    pathFigure.Segments.Add(ttl);

                    arrowTipGeometry.Figures.Add(pathFigure);

                    pathFigure = new PathFigure();
                    pathFigure.StartPoint = new Point(X1, Y1);

                    LineSegment ttr = new LineSegment(new Point(X1, Y1 - 8), true);
                    pathFigure.Segments.Add(ttr);

                    arrowTipGeometry.Figures.Add(pathFigure);

                    Path spinArrow = new Path();
                    spinArrow.Data = arrowTipGeometry;
                    spinArrow.Stroke = Brushes.Blue;
                    spinArrow.StrokeThickness = SPIN_STROKE_THICKNESS;

                    spinArrow.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
                    spinArrow.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
                    spinArrow.DataContext = stroke;
                    spinArrow.Tag = TAG_SPIN_ARROW;
                    Message.SetAttach(spinArrow, "StrokeSelected($DataContext)");

                    if (!strokeShapes[stroke].Contains(spinArrow))
                        strokeShapes[stroke].Add(spinArrow);

                    InnerFieldSpinGrid.Children.Add(spinArrow);
                }
                else
                {
                    Debug.WriteLine("invalid Placement of stroke {0}: x={1} y={2}", stroke, stroke.Placement.WX, stroke.Placement.WY);
                }
            }
        }
        
        private double GetAdjustedX(Stroke stroke, double oldX)
        {
            if (stroke.Placement.WY < 137)
            {
                // stroke in the upper half of table
                return oldX + tableMarginFieldLeft;                
            }
            else
            {
                // stroke in the lower half of table => flip x
                return halfFieldWidth - ((oldX + tableMarginFieldLeft) - halfFieldWidth);
            }
        }

        private double GetAdjustedY(Stroke stroke, double oldY)
        {
            if (stroke.Placement.WY < 137)
            {
                // stroke in the upper half of table
                return oldY + tableMarginFieldBottom;
            }
            else
            {
                // stroke in the lower half of table => flip y
                return InnerFieldGrid.ActualHeight - tableMarginFieldBottom - oldY;
            }
        }

        private void RemoveStrokesDirectionLines()
        {
            foreach (var s in strokeShapes.Values)
            {
                for (int i = 0; i < s.Count; i++)
                {
                    if (s[i] is Line)
                    {
                        InnerFieldGrid.Children.Remove(s[i]);
                        s.RemoveAt(i);
                        --i;
                    }
                }                
            }
        }

        private void RemoveServiceStrokesSpinArrows()
        {
            foreach (var s in strokeShapes.Values)
            {
                for (int i = 0; i < s.Count; i++)
                {
                    if (s[i] is Path && TAG_SPIN_ARROW.Equals((s[i] as Path).Tag))
                    {
                        InnerFieldSpinGrid.Children.Remove(s[i]);
                        s.RemoveAt(i);
                        --i;
                    }
                }
            }
        }

        private void ApplyStyle(Shape shape, string style)
        {
            switch (style)
            {
                case STROKE_ATTR_SIDE_BACKHAND:
                    DoubleCollection dashes = new DoubleCollection();
                    dashes.Add(2);
                    shape.StrokeDashArray = dashes;
                    break;
                case STROKE_ATTR_TECHNIQUE_PUSH:
                case STROKE_ATTR_CHOP:
                    shape.Stroke = Brushes.Red;
                    shape.Fill = Brushes.Red;
                    break;
                case STROKE_ATTR_TECHNIQUE_FLIP:
                case STROKE_ATTR_TECHNIQUE_OPTION_BANANA:
                    shape.Stroke = Brushes.Yellow;
                    shape.Fill = Brushes.Yellow;
                    break;
                case STROKE_ATTR_SMASH:
                    shape.Stroke = Brushes.Blue;
                    shape.Fill = Brushes.Blue;
                    shape.StrokeThickness = STROKE_THICKNESS_SMASH;
                    break;
                case STROKE_ATTR_HAS_SPIN:
                    shape.Stroke = Brushes.SaddleBrown;
                    shape.Fill = Brushes.SaddleBrown;
                    break;
            }
        }

        private bool PlacementValuesValid(Placement placement)
        {
            return placement.WX != double.NaN && placement.WX > 0 && placement.WY != double.NaN && placement.WY > 0;
        }

        private double GetRotationAngleForSpin(Spin spin)
        {
            if ("1".Equals(spin.SL) && "1".Equals(spin.TS))
                return -45;
            else if ("1".Equals(spin.SL) && "1".Equals(spin.US))
                return -135;
            else if ("1".Equals(spin.SL))
                return -90;
            else if ("1".Equals(spin.SR) && "1".Equals(spin.TS))
                return 45;
            else if ("1".Equals(spin.SR) && "1".Equals(spin.US))
                return 135;
            else if ("1".Equals(spin.SR))
                return 90;
            else if ("1".Equals(spin.TS))
                return 0;
            else if ("1".Equals(spin.US))
                return 180;
            else
                return 0;
        }
    }
}
