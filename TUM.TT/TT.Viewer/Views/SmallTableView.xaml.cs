
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TT.Models;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaction logic for SmallTableView.xaml
    /// </summary>
    public partial class SmallTableView : UserControl
    {
        #region Constants

        private const double STROKE_THICKNESS = 1.75;
        private const double STROKE_THICKNESS_HOVER = 2.5;
        private const double SPIN_ARROW_STROKE_THICKNESS = 1;
        private const double SPIN_ARROW_STROKE_THICKNESS_HOVER = 2;
        private const double STROKE_THICKNESS_SMASH = 3.5;
        private const double STROKE_THICKNESS_SMASH_HOVER = 5;

        private const string TAG_SPIN_ARROW = "spinarrow";
        private const string TAG_ARROW_TIP = "arrowtip";
        private const string TAG_DIRECTION = "direction";

        private const string STROKE_ATTR_SIDE_FOREHAND = "Forehand";
        private const string STROKE_ATTR_SIDE_BACKHAND = "Backhand";

        private const string STROKE_ATTR_HAS_SPIN = "has_spin";

        private const string STROKE_ATTR_TECHNIQUE_PUSH = "Push";
        private const string STROKE_ATTR_TECHNIQUE_FLIP = "Flip";
        private const string STROKE_ATTR_TECHNIQUE_OPTION_BANANA = "Banana";
        private const string STROKE_ATTR_TECHNIQUE_TOPSPIN = "Topspin";
        private const string STROKE_ATTR_TECHNIQUE_BLOCK = "Block";
        private const string STROKE_ATTR_TECHNIQUE_SMASH = "Smash";
        private const string STROKE_ATTR_TECHNIQUE_COUNTER = "Counter";
        private const string STROKE_ATTR_TECHNIQUE_CHOP = "Chop";
        private const string STROKE_ATTR_TECHNIQUE_LOB = "Lob";
        private const string STROKE_ATTR_TECHNIQUE_MISCELLANEOUS = "Miscellaneous";

        private const string STROKE_ATTR_POC_OVER = "over";
        private const string STROKE_ATTR_POC_HALFDISTANCE = "half-distance";
        private const string STROKE_ATTR_POC_BEHIND = "behind";

        #endregion

        private Dictionary<Stroke, List<Shape>> strokeShapes;

        public ICollection<Stroke> Strokes
        {
            get { return (ICollection<Stroke>)GetValue(StrokesProperty); }
            set { SetValue(StrokesProperty, value); }
        }

        public static DependencyProperty StrokesProperty = DependencyProperty.Register(
        "Strokes", typeof(ICollection<Stroke>), typeof(SmallTableView), new FrameworkPropertyMetadata(default(ICollection<Stroke>), new PropertyChangedCallback(OnStrokesPropertyChanged)));

        public SmallTableView()
        {
            InitializeComponent();
            strokeShapes = new Dictionary<Stroke, List<Shape>>();
        }

        private void Table_MouseEnter(object sender, MouseEventArgs e)
        {
            SmallTableViewBorder.BorderThickness = new Thickness(2);
        }

        private void Table_MouseLeave(object sender, MouseEventArgs e)
        {
            SmallTableViewBorder.BorderThickness = new Thickness(1);
        }

        private void Handle(List<Stroke> strokes)
        {
            for (int i = 0; i < strokes.Count && i < 4; i++)
            {
                Stroke stroke = strokes[i];
                if (!PlacementValuesValid(stroke.Placement))
                    continue;

                foreach (UIElement p in TableGrid.Children)
                {
                    if (p is Grid)
                        (p as Grid).Children.Clear();
                }
                strokeShapes.Clear();

                strokes.ForEach(s => { strokeShapes[s] = new List<Shape>(); });

                switch (stroke.Number)
                {
                    case 1:
                        AddStrokesArrowtips(strokes, true);
                        //if (checkdirection.ischecked.value)
                            AddStrokesDirectionLines(strokes, true);
                        //if (checkspin.ischecked.value)
                            AddServiceStrokesSpinArrows(strokes);
                        break;
                    default:
                        //if (checkdirection.ischecked.value)
                            AddStrokesDirectionLines(strokes, false);
                        AddStrokesArrowtips(strokes, false);
                        break;
                }
            }
        }

        private static void OnStrokesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("OnStrokesPropertyChanged sender={0}, sender.Tag={3} e.ov={1}, e.nv={2}", sender, e.OldValue, e.NewValue, ((SmallTableView)sender).Tag);

            if ((string)((SmallTableView)sender).Tag == "SmallTable")
                ((SmallTableView)sender).SmallTableViewBorder.Visibility = Visibility.Visible;

            Grid innerFieldGrid = ((SmallTableView)sender).InnerFieldGrid;
            if (e.NewValue is ICollection<Stroke>)
            {
                List<Stroke> strokes = new List<Stroke>((ICollection<Stroke>) e.NewValue);
                ((SmallTableView)sender).Handle(strokes);
                
                    

                    //Line l = new Line();
                    //l.X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : stroke.Playerposition;
                    //l.Y1 = innerFieldGrid.ActualHeight;
                    //l.X2 = stroke.Placement.WX.Equals(double.NaN) ? 0 : stroke.Placement.WX;
                    //l.Y2 = stroke.Placement.WY.Equals(double.NaN) ? 0 : stroke.Placement.WY;

                    //l.Stroke = Brushes.Black;
                    //l.StrokeThickness = 6;

                    //innerFieldGrid.Children.Add(l);
            }
        }

        private static bool PlacementValuesValid(Placement placement)
        {
            return placement.WX != double.NaN && placement.WX > 0 && placement.WY != double.NaN && placement.WY > 0;
        }



        #region ResultLargeTableView.xaml.cs contents

        private void Stroke_MouseEnter(Object sender, MouseEventArgs e)
        {
            Shape shape = sender as Shape;
            Stroke stroke = shape.DataContext as Stroke;

            //Debug.WriteLine("mouse enter on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);

            foreach (var strokeShape in strokeShapes)
            {
                foreach (var s in strokeShape.Value)
                {
                    if (strokeShape.Key.Equals(stroke))
                    {
                        if ((string)s.Tag == TAG_SPIN_ARROW)
                        {
                            s.StrokeThickness = SPIN_ARROW_STROKE_THICKNESS_HOVER;
                        }
                        else
                        {
                            if (stroke.Stroketechnique != null && stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_SMASH)
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
            Shape shape = sender as Shape;
            Stroke stroke = shape.DataContext as Stroke;

            //Debug.WriteLine("mouse leave on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);

            foreach (var strokeShape in strokeShapes)
            {
                foreach (var s in strokeShape.Value)
                {
                    if (strokeShape.Key.Equals(stroke))
                    {
                        if ((string)s.Tag == TAG_SPIN_ARROW)
                        {
                            s.StrokeThickness = SPIN_ARROW_STROKE_THICKNESS;
                        }
                        else
                        {
                            if (stroke.Stroketechnique != null && stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_SMASH)
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

        private void AddStrokesDirectionLines(List<Stroke> strokes, bool isServiceStroke)
        {
            Debug.WriteLine("Strokes to paint: {0}", strokes.Count);
            foreach (var stroke in strokes)
            {
                isServiceStroke = stroke.Number == 1;
                if (PlacementValuesValid(stroke.Placement))
                {
                    double X1, X2, Y1, Y2;

                    if (isServiceStroke)
                    {
                        X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : GetAdjustedX(stroke, stroke.Playerposition);
                        Y1 = InnerFieldBehindGrid.ActualHeight; // service strokes start at the bottom
                    }
                    else
                    {
                        var precedingStroke = stroke.Rally.Strokes[stroke.Number - 2];

                        X1 = GetAdjustedX(stroke, precedingStroke.Placement.WX);
                        Y1 = GetAdjustedY(stroke, precedingStroke.Placement.WY);
                    }

                    X2 = GetAdjustedX(stroke, stroke.Placement.WX);
                    Y2 = GetAdjustedY(stroke, stroke.Placement.WY);

                    Shape shape;
                    if (!isServiceStroke)
                        if (stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_FLIP || stroke.Stroketechnique.Option == STROKE_ATTR_TECHNIQUE_OPTION_BANANA)
                            shape = GetBananaShape(stroke, X1, Y1, X2, Y2);
                        else if (stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_TOPSPIN)
                            shape = GetTopSpinShape(stroke, X1, Y1, X2, Y2);
                        else if (stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_CHOP)
                            shape = GetChopShape(stroke, X1, Y1, X2, Y2);
                        else if (stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_LOB)
                            shape = GetLobShape(stroke, X1, Y1, X2, Y2);
                        else
                            shape = GetLineShape(stroke, X1, Y1, X2, Y2);
                    else
                        shape = GetLineShape(stroke, X1, Y1, X2, Y2);

                    shape.StrokeThickness = STROKE_THICKNESS;
                    shape.Stroke = Brushes.Black;

                    shape.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
                    shape.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
                    shape.DataContext = stroke;
                    shape.Tag = TAG_DIRECTION;
                    Message.SetAttach(shape, "StrokeSelected($DataContext)");

                    if (!strokeShapes[stroke].Contains(shape))
                        strokeShapes[stroke].Add(shape);

                    ApplyStyle(stroke, shape);

                    if (isServiceStroke)
                        InnerFieldBehindGrid.Children.Add(shape);
                    else
                    {
                        if (stroke.PointOfContact == STROKE_ATTR_POC_BEHIND)
                            InnerFieldBehindGrid.Children.Add(shape);
                        else if (stroke.PointOfContact == STROKE_ATTR_POC_HALFDISTANCE)
                            InnerFieldHalfDistanceGrid.Children.Add(shape);
                        else
                            InnerFieldGrid.Children.Add(shape);
                    }

                    shape = strokeShapes[stroke].Find(s => s is Path && (string)s.Tag == TAG_DIRECTION);
                    if (shape != null)
                    {
                        Geometry geom = ((Path)shape).Data;
                        if (geom is PathGeometry)
                        {
                            PathSegment seg = ((PathGeometry)geom).Figures[0].Segments[0];
                            if (seg is QuadraticBezierSegment)
                            {
                                X2 = ((QuadraticBezierSegment)seg).Point1.X;
                                Y2 = ((QuadraticBezierSegment)seg).Point1.Y;
                            }
                        }
                    }

                    if (!isServiceStroke && stroke.PointOfContact == STROKE_ATTR_POC_BEHIND)
                        AddBehindLine(stroke, X1, Y1, X2, Y2);
                    else if (!isServiceStroke && stroke.PointOfContact == STROKE_ATTR_POC_HALFDISTANCE)
                        AddHalfDistanceLine(stroke, X1, Y1, X2, Y2);
                }
                else
                {
                    Debug.WriteLine("invalid Placement of stroke {0} in rally {3}: x={1} y={2}", stroke.Number, stroke.Placement.WX, stroke.Placement.WY, stroke.Rally.Number);
                }
            }
        }

        private void AddStrokesArrowtips(List<Stroke> strokes, bool isServiceStroke)
        {
            foreach (var stroke in strokes)
            {
                isServiceStroke = stroke.Number == 1;
                if (PlacementValuesValid(stroke.Placement))
                {
                    double X1, X2, Y1, Y2;
                    X1 = Y1 = int.MinValue;

                    if (isServiceStroke)
                    {
                        X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : GetAdjustedX(stroke, stroke.Playerposition);
                        Y1 = InnerFieldBehindGrid.ActualHeight; // service strokes start at the bottom
                    }
                    else
                    {
                        Shape shape = strokeShapes[stroke].Find(s => s is Path && (string)s.Tag == TAG_DIRECTION);
                        if (shape != null)
                        {
                            Geometry geom = ((Path)shape).Data;
                            if (geom is PathGeometry)
                            {
                                PathSegment seg = ((PathGeometry)geom).Figures[0].Segments[0];
                                if (seg is QuadraticBezierSegment)
                                {
                                    X1 = ((QuadraticBezierSegment)seg).Point1.X;
                                    Y1 = ((QuadraticBezierSegment)seg).Point1.Y;
                                }
                            }
                        }
                        if (X1 == int.MinValue || Y1 == int.MinValue)
                        {
                            var precedingStroke = stroke.Rally.Strokes[stroke.Number - 2];
                            X1 = GetAdjustedX(stroke, precedingStroke.Placement.WX);
                            Y1 = GetAdjustedY(stroke, precedingStroke.Placement.WY);
                        }
                    }

                    X2 = GetAdjustedX(stroke, stroke.Placement.WX);
                    Y2 = GetAdjustedY(stroke, stroke.Placement.WY);

                    PathGeometry arrowTipGeometry = new PathGeometry();

                    PathFigure pathFigure = new PathFigure();
                    pathFigure.IsClosed = true;
                    pathFigure.StartPoint = new Point(X2 - 3, Y2 - 3);

                    LineSegment ltt = new LineSegment(new Point(X2, Y2), true);
                    pathFigure.Segments.Add(ltt);

                    LineSegment ttr = new LineSegment(new Point(X2 + 3, Y2 - 3), true);
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
                    strokeArrowTip.StrokeThickness = STROKE_THICKNESS;

                    ApplyStyle(stroke, strokeArrowTip);

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
                    Debug.WriteLine("invalid Placement of stroke {0} in rally {3}: x={1} y={2}", stroke.Number, stroke.Placement.WX, stroke.Placement.WY, stroke.Rally.Number);
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
                    if (stroke.Spin == null || stroke.Spin.No == "1")
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
                    spinArrow.StrokeThickness = SPIN_ARROW_STROKE_THICKNESS;

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
                    Debug.WriteLine("invalid Placement of stroke {0} in rally {3}: x={1} y={2}", stroke.Number, stroke.Placement.WX, stroke.Placement.WY, stroke.Rally.Number);
                }
            }
        }

        private void AddBehindLine(Stroke stroke, double x1, double y1, double x2, double y2)
        {
            Line behindLine = new Line();
            behindLine.X1 = x2 - (x2 - x1) * ((InnerFieldBehindGrid.ActualHeight - y2) / (y1 - y2));
            behindLine.Y1 = InnerFieldBehindGrid.ActualHeight;

            behindLine.X2 = x1;
            behindLine.Y2 = y1;

            behindLine.Stroke = Brushes.Black;
            behindLine.Fill = Brushes.Black;
            behindLine.StrokeThickness = STROKE_THICKNESS;

            DoubleCollection dashes = new DoubleCollection();
            dashes.Add(0.5);
            behindLine.StrokeDashArray = dashes;

            ApplyStyle(stroke, behindLine);

            behindLine.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
            behindLine.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
            behindLine.DataContext = stroke;
            behindLine.Tag = TAG_DIRECTION;
            Message.SetAttach(behindLine, "StrokeSelected($DataContext)");

            if (!strokeShapes[stroke].Contains(behindLine))
                strokeShapes[stroke].Add(behindLine);

            InnerFieldBehindGrid.Children.Add(behindLine);
        }

        private void AddHalfDistanceLine(Stroke stroke, double x1, double y1, double x2, double y2)
        {
            Line halfDistanceLine = new Line();
            halfDistanceLine.X1 = x2 - (x2 - x1) * ((InnerFieldHalfDistanceGrid.ActualHeight - y2) / (y1 - y2));
            halfDistanceLine.Y1 = InnerFieldHalfDistanceGrid.ActualHeight;

            halfDistanceLine.X2 = x1;
            halfDistanceLine.Y2 = y1;

            halfDistanceLine.Stroke = Brushes.Black;
            halfDistanceLine.Fill = Brushes.Black;
            halfDistanceLine.StrokeThickness = STROKE_THICKNESS;

            DoubleCollection dashes = new DoubleCollection();
            dashes.Add(1);
            halfDistanceLine.StrokeDashArray = dashes;

            ApplyStyle(stroke, halfDistanceLine);

            halfDistanceLine.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
            halfDistanceLine.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
            halfDistanceLine.DataContext = stroke;
            halfDistanceLine.Tag = TAG_DIRECTION;
            Message.SetAttach(halfDistanceLine, "StrokeSelected($DataContext)");

            if (!strokeShapes[stroke].Contains(halfDistanceLine))
                strokeShapes[stroke].Add(halfDistanceLine);

            InnerFieldHalfDistanceGrid.Children.Add(halfDistanceLine);
        }

        private Shape GetLobShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineTwoThirdsX, lineTwoThirdsY;

            lineTwoThirdsY = Y1 - (2d / 3d) * (Y1 - Y2);
            if (X1 <= X2)
            {
                m = (X2 - X1) / (Y1 - Y2);
                lineTwoThirdsX = X1 + (2d / 3d) * (X2 - X1);
                cpx = lineTwoThirdsX - 0.7 * Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y1 - Y2, 2));
                cpy = lineTwoThirdsY - m * (lineTwoThirdsX - cpx);
            }
            else
            {
                m = (X1 - X2) / (Y1 - Y2);
                lineTwoThirdsX = X1 - (2d / 3d) * (X1 - X2);
                cpx = lineTwoThirdsX + 0.7 * Math.Sqrt(Math.Pow(X1 - X2, 2) + Math.Pow(Y1 - Y2, 2));
                cpy = lineTwoThirdsY - m * (cpx - lineTwoThirdsX);
            }

            //Debug.WriteLine("chop {7} of rally {6}: x1={0} y1={1} -> x2={2} y2={3} (cp: x={4} y={5})", X1, Y1, X2, Y2, cpx, cpy, stroke.Rally.Number, stroke.Number);

            PathGeometry chopGeometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure();

            Point startPoint = new Point(X1, Y1);
            Point endPoint = new Point(X2, Y2);
            Point controlPoint = new Point(cpx, cpy);

            pathFigure.StartPoint = startPoint;

            QuadraticBezierSegment segment = new QuadraticBezierSegment(controlPoint, endPoint, true);
            pathFigure.Segments.Add(segment);

            chopGeometry.Figures.Add(pathFigure);

            Path chopPath = new Path();
            chopPath.Data = chopGeometry;
            return chopPath;
        }

        private Shape GetChopShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineNineteenTwentiethsX, lineNineteenTwentiethsY;

            lineNineteenTwentiethsY = Y1 - (19d / 20d) * (Y1 - Y2);
            if (X1 <= X2)
            {
                m = (X2 - X1) / (Y1 - Y2);
                lineNineteenTwentiethsX = X1 + (19d / 20d) * (X2 - X1);
                cpx = lineNineteenTwentiethsX - 0.2 * Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y1 - Y2, 2));
                cpy = lineNineteenTwentiethsY - m * (lineNineteenTwentiethsX - cpx);
            }
            else
            {
                m = (X1 - X2) / (Y1 - Y2);
                lineNineteenTwentiethsX = X1 - (19d / 20d) * (X1 - X2);
                cpx = lineNineteenTwentiethsX + 0.2 * Math.Sqrt(Math.Pow(X1 - X2, 2) + Math.Pow(Y1 - Y2, 2));
                cpy = lineNineteenTwentiethsY - m * (cpx - lineNineteenTwentiethsX);
            }

            //Debug.WriteLine("chop {7} of rally {6}: x1={0} y1={1} -> x2={2} y2={3} (cp: x={4} y={5})", X1, Y1, X2, Y2, cpx, cpy, stroke.Rally.Number, stroke.Number);

            PathGeometry chopGeometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure();

            Point startPoint = new Point(X1, Y1);
            Point endPoint = new Point(X2, Y2);
            Point controlPoint = new Point(cpx, cpy);

            pathFigure.StartPoint = startPoint;

            QuadraticBezierSegment segment = new QuadraticBezierSegment(controlPoint, endPoint, true);
            pathFigure.Segments.Add(segment);

            chopGeometry.Figures.Add(pathFigure);

            Path chopPath = new Path();
            chopPath.Data = chopGeometry;
            return chopPath;
        }

        private Shape GetTopSpinShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineFourFifthsX, lineFourFifthsY;

            lineFourFifthsY = Y1 - (4d / 5d) * (Y1 - Y2);
            if (X1 <= X2)
            {
                m = (X2 - X1) / (Y1 - Y2);
                lineFourFifthsX = X1 + (4d / 5d) * (X2 - X1);
                cpx = lineFourFifthsX - 0.25 * Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y1 - Y2, 2));
                cpy = lineFourFifthsY - m * (lineFourFifthsX - cpx);
            }
            else
            {
                m = (X1 - X2) / (Y1 - Y2);
                lineFourFifthsX = X1 - (4d / 5d) * (X1 - X2);
                cpx = lineFourFifthsX + 0.25 * Math.Sqrt(Math.Pow(X1 - X2, 2) + Math.Pow(Y1 - Y2, 2));
                cpy = lineFourFifthsY - m * (cpx - lineFourFifthsX);
            }

            //Debug.WriteLine("topspin {7} of rally {6}: x1={0} y1={1} -> x2={2} y2={3} (cp: x={4} y={5})", X1, Y1, X2, Y2, cpx, cpy, stroke.Rally.Number, stroke.Number);

            PathGeometry topSpinGeometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure();

            Point startPoint = new Point(X1, Y1);
            Point endPoint = new Point(X2, Y2);
            Point controlPoint = new Point(cpx, cpy);

            pathFigure.StartPoint = startPoint;

            QuadraticBezierSegment segment = new QuadraticBezierSegment(controlPoint, endPoint, true);
            pathFigure.Segments.Add(segment);

            topSpinGeometry.Figures.Add(pathFigure);

            Path topSpinPath = new Path();
            topSpinPath.Data = topSpinGeometry;
            return topSpinPath;
        }

        private Shape GetBananaShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineMiddleX;

            if (X1 <= X2)
            {
                m = (X2 - X1) / (Y1 - Y2);
                lineMiddleX = (X2 + X1) / 2;
                cpx = lineMiddleX - 0.25 * Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y1 - Y2, 2));
                cpy = (Y1 + Y2) / 2 - m * (lineMiddleX - cpx);
            }
            else
            {
                m = (X1 - X2) / (Y1 - Y2);
                lineMiddleX = (X1 + X2) / 2;
                cpx = lineMiddleX + 0.25 * Math.Sqrt(Math.Pow(X1 - X2, 2) + Math.Pow(Y1 - Y2, 2));
                cpy = (Y1 + Y2) / 2 - m * (cpx - lineMiddleX);
            }

            //Debug.WriteLine("banana/flip {7} of rally {6}: x1={0} y1={1} -> x2={2} y2={3} (cp: x={4} y={5})", X1, Y1, X2, Y2, cpx, cpy, stroke.Rally.Number, stroke.Number);

            PathGeometry bananaGeometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure();

            Point startPoint = new Point(X1, Y1);
            Point endPoint = new Point(X2, Y2);
            Point controlPoint = new Point(cpx, cpy);

            pathFigure.StartPoint = startPoint;

            QuadraticBezierSegment segment = new QuadraticBezierSegment(controlPoint, endPoint, true);
            pathFigure.Segments.Add(segment);

            bananaGeometry.Figures.Add(pathFigure);

            Path bananaPath = new Path();
            bananaPath.Data = bananaGeometry;
            return bananaPath;
        }

        private Shape GetLineShape(Stroke stroke, double x1, double y1, double x2, double y2)
        {
            Line line = new Line();
            line.X1 = x1; line.Y1 = y1; line.X2 = x2; line.Y2 = y2;

            //Debug.WriteLine("stroke {5} of rally {4}: x1={0} y1={1} -> x2={2} y2={3}", x1, y1, x2, y2, stroke.Rally.Number, stroke.Number);

            return line;
        }

        private void ApplyStyle(Stroke stroke, Shape shape)
        {
            if (stroke.Number == 1)
            {
                if (stroke.Side == STROKE_ATTR_SIDE_BACKHAND)
                    ApplyStyle(shape, STROKE_ATTR_SIDE_BACKHAND);
                if (stroke.Spin != null && stroke.Spin.No != "1")
                    ApplyStyle(shape, STROKE_ATTR_HAS_SPIN);
            }
            else
            {
                if (stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_PUSH || stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_CHOP)
                    ApplyStyle(shape, STROKE_ATTR_TECHNIQUE_PUSH);
                else if (stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_FLIP || stroke.Stroketechnique.Option == STROKE_ATTR_TECHNIQUE_OPTION_BANANA)
                    ApplyStyle(shape, STROKE_ATTR_TECHNIQUE_FLIP);
                else if (stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_SMASH)
                    ApplyStyle(shape, STROKE_ATTR_TECHNIQUE_SMASH);
                else if (stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_BLOCK || stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_COUNTER)
                    ApplyStyle(shape, STROKE_ATTR_TECHNIQUE_BLOCK);
                else if (stroke.Stroketechnique.Type == STROKE_ATTR_TECHNIQUE_MISCELLANEOUS)
                    ApplyStyle(shape, STROKE_ATTR_TECHNIQUE_MISCELLANEOUS);
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
                case STROKE_ATTR_TECHNIQUE_CHOP:
                    shape.Stroke = Brushes.Red;
                    break;
                case STROKE_ATTR_TECHNIQUE_FLIP:
                case STROKE_ATTR_TECHNIQUE_OPTION_BANANA:
                    shape.Stroke = Brushes.Yellow;
                    break;
                case STROKE_ATTR_TECHNIQUE_SMASH:
                    shape.Stroke = Brushes.Blue;
                    shape.Fill = Brushes.Blue;
                    shape.StrokeThickness = STROKE_THICKNESS_SMASH;
                    break;
                case STROKE_ATTR_HAS_SPIN:
                    shape.Stroke = Brushes.SaddleBrown;
                    shape.Fill = Brushes.SaddleBrown;
                    break;
                case STROKE_ATTR_TECHNIQUE_BLOCK:
                case STROKE_ATTR_TECHNIQUE_COUNTER:
                    shape.Stroke = Brushes.Blue;
                    shape.Fill = Brushes.Blue;
                    break;
                case STROKE_ATTR_TECHNIQUE_MISCELLANEOUS:
                    shape.Stroke = Brushes.Green;
                    break;
            }
        }

        private double GetAdjustedX(Stroke stroke, double oldX)
        {
            Grid grid = GetGridForStroke(stroke);
            if (stroke.Placement.WY < 137)
            {
                // stroke in the upper half of table
                return oldX + (TableBorder.Margin.Left - grid.Margin.Left);
            }
            else
            {
                // stroke in the lower half of table => flip x
                return grid.ActualWidth - oldX - (TableBorder.Margin.Left - grid.Margin.Left);
            }
        }

        private double GetAdjustedY(Stroke stroke, double oldY)
        {
            Grid grid = GetGridForStroke(stroke);
            if (stroke.Placement.WY < 137)
            {
                // stroke in the upper half of table
                return oldY + (TableBorder.Margin.Top - grid.Margin.Top);
            }
            else
            {
                // stroke in the lower half of table => flip y
                return grid.ActualHeight - oldY - (TableBorder.Margin.Bottom - grid.Margin.Bottom);
            }
        }

        private Grid GetGridForStroke(Stroke stroke)
        {
            if (stroke.Number == 1)
                return InnerFieldBehindGrid;
            else if (stroke.PointOfContact == STROKE_ATTR_POC_OVER)
                return InnerFieldGrid;
            else if (stroke.PointOfContact == STROKE_ATTR_POC_BEHIND)
                return InnerFieldBehindGrid;
            else if (stroke.PointOfContact == STROKE_ATTR_POC_HALFDISTANCE)
                return InnerFieldHalfDistanceGrid;
            return null;
        }


        private double GetRotationAngleForSpin(Spin spin)
        {
            if (spin.SL == "1" && spin.TS == "1")
                return -45;
            else if (spin.SL == "1" && spin.US == "1")
                return -135;
            else if (spin.SL == "1")
                return -90;
            else if (spin.SR == "1" && spin.TS == "1")
                return 45;
            else if (spin.SR == "1" && spin.US == "1")
                return 135;
            else if (spin.SR == "1")
                return 90;
            else if (spin.TS == "1")
                return 0;
            else if (spin.US == "1")
                return 180;
            else
                return 0;
        }

        #endregion
    }
}
