using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TT.Models;

namespace TT.Viewer.Views
{
    public abstract class TableView : UserControl
    {
        protected Dictionary<Stroke, List<Shape>> StrokeShapes { get; private set; }

        public abstract Grid View_InnerFieldGrid { get; }
        public abstract Grid View_TableGrid { get; }
        public abstract Grid View_InnerFieldBehindGrid { get; }
        public abstract Grid View_InnerFieldHalfDistanceGrid { get; }
        public abstract Grid View_InnerFieldSpinGrid { get; }
        public abstract Border View_TableBorder { get; }

        public TableView()
        {
            StrokeShapes = new Dictionary<Stroke, List<Shape>>();
        }

        #region Constants

        protected const double STROKE_THICKNESS = 1.75;
        protected const double STROKE_THICKNESS_HOVER = 2.5;
        protected const double STROKE_THICKNESS_SPIN_ARROW = 1;
        protected const double STROKE_THICKNESS_SPIN_ARROW_HOVER = 2;
        protected const double STROKE_THICKNESS_SMASH = 3.5;
        protected const double STROKE_THICKNESS_SMASH_HOVER = 5;
        protected const double STROKE_THICKNESS_DEBUG_PRECEDING = 0.5;
        protected const double STROKE_THICKNESS_DEBUG_PRECEDING_HOVER = 0.7;
        protected const double STROKE_THICKNESS_INTERCEPT = 1.0;
        protected const double STROKE_THICKNESS_INTERCEPT_HOVER = 1.7;

        protected const string TAG_SPIN_ARROW = "spinarrow";
        protected const string TAG_ARROW_TIP = "arrowtip";
        protected const string TAG_DIRECTION = "direction";
        protected const string TAG_INTERCEPT = "intercept";
        protected const string TAG_DEBUG_PRECEDING = "debug_preceding";
        protected const string TAG_SMALL_TABLE = "SmallTable";

        protected const string STROKE_ATTR_SIDE_FOREHAND = "Forehand";
        protected const string STROKE_ATTR_SIDE_BACKHAND = "Backhand";

        protected const string STROKE_ATTR_HAS_SPIN = "has_spin";

        protected const string STROKE_ATTR_TECHNIQUE_PUSH = "Push";
        protected const string STROKE_ATTR_TECHNIQUE_FLIP = "Flip";
        protected const string STROKE_ATTR_TECHNIQUE_OPTION_BANANA = "Banana";
        protected const string STROKE_ATTR_TECHNIQUE_TOPSPIN = "Topspin";
        protected const string STROKE_ATTR_TECHNIQUE_BLOCK = "Block";
        protected const string STROKE_ATTR_TECHNIQUE_SMASH = "Smash";
        protected const string STROKE_ATTR_TECHNIQUE_COUNTER = "Counter";
        protected const string STROKE_ATTR_TECHNIQUE_CHOP = "Chop";
        protected const string STROKE_ATTR_TECHNIQUE_LOB = "Lob";
        protected const string STROKE_ATTR_TECHNIQUE_MISCELLANEOUS = "Miscellaneous";

        protected const string STROKE_ATTR_POC_OVER = "over";
        protected const string STROKE_ATTR_POC_HALFDISTANCE = "half-distance";
        protected const string STROKE_ATTR_POC_BEHIND = "behind";

        #endregion

        #region Dependency Properties

        public ICollection<Stroke> Strokes
        {
            get { return (ICollection<Stroke>)GetValue(StrokesProperty); }
            set { SetValue(StrokesProperty, value); }
        }

        public bool ShowDebug
        {
            get { return (bool)GetValue(ShowDebugProperty); }
            set { SetValue(ShowDebugProperty, value); }
        }

        public bool ShowDirection
        {
            get { return (bool)GetValue(ShowDirectionProperty); }
            set { SetValue(ShowDirectionProperty, value); }
        }

        public bool ShowSpin
        {
            get { return (bool)GetValue(ShowSpinProperty); }
            set { SetValue(ShowSpinProperty, value); }
        }

        public bool ShowIntercept
        {
            get { return (bool)GetValue(ShowInterceptProperty); }
            set { SetValue(ShowInterceptProperty, value); }
        }

        public static DependencyProperty StrokesProperty = DependencyProperty.Register(
            "Strokes", typeof(ICollection<Stroke>), typeof(TableView), new PropertyMetadata(default(ICollection<Stroke>), new PropertyChangedCallback(OnStrokesPropertyChanged)));

        public static DependencyProperty ShowDebugProperty = DependencyProperty.Register(
            "ShowDebug", typeof(bool), typeof(TableView), new PropertyMetadata(true, new PropertyChangedCallback(OnDisplayTypePropertyChanged)));

        public static DependencyProperty ShowDirectionProperty = DependencyProperty.Register(
            "ShowDirection", typeof(bool), typeof(TableView), new PropertyMetadata(true, new PropertyChangedCallback(OnDisplayTypePropertyChanged)));

        public static DependencyProperty ShowSpinProperty = DependencyProperty.Register(
            "ShowSpin", typeof(bool), typeof(TableView), new PropertyMetadata(true, new PropertyChangedCallback(OnDisplayTypePropertyChanged)));

        public static DependencyProperty ShowInterceptProperty = DependencyProperty.Register(
            "ShowIntercept", typeof(bool), typeof(TableView), new PropertyMetadata(true, new PropertyChangedCallback(OnDisplayTypePropertyChanged)));

        #endregion

        #region Event handlers
        
        private static void OnStrokesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Debug.WriteLine("OnStrokesPropertyChanged sender={0}, sender.Tag={3} e.ov={1}, e.nv={2}", sender, e.OldValue, e.NewValue, ((TableView)sender).Tag);

            TableView view = (TableView)sender;
            view.ProcessStrokes(new List<Stroke>((ICollection<Stroke>)e.NewValue));
        }

        protected abstract void ProcessStrokes(List<Stroke> strokes);

        private static void OnDisplayTypePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Debug.WriteLine("OnDisplayTypePropertyChanged sender={0}, sender.Tag={3} e.ov={1}, e.nv={2}", sender, e.OldValue, e.NewValue, ((TableView)sender).Tag);

            if (sender is TableView)
            {
                TableView view = (TableView)sender;

                if (e.Property == ShowDirectionProperty)
                {
                    if ((bool)e.NewValue)
                        foreach (Stroke s in view.StrokeShapes.Keys) view.AddStrokesDirectionLines(s);
                    else
                        view.HideShapesByTag(TAG_DIRECTION);
                }
                else if (e.Property == ShowSpinProperty)
                {
                    if ((bool)e.NewValue)
                        foreach (Stroke s in view.StrokeShapes.Keys) view.AddServiceStrokesSpinArrows(s);
                    else
                        view.HideShapesByTag(TAG_SPIN_ARROW);
                }
                else if (e.Property == ShowDebugProperty)
                {
                    // you'll have to reload the view to display debug lines again :(
                    if (!(bool)e.NewValue)
                        view.HideShapesByTag(TAG_DEBUG_PRECEDING);
                }
                else if (e.Property == ShowInterceptProperty)
                {
                    if ((bool)e.NewValue)
                        foreach (Stroke s in view.StrokeShapes.Keys) view.AddInterceptArrows(s);
                    else
                        view.HideShapesByTag(TAG_INTERCEPT);
                }
            }
        }
        
        #endregion

        #region Shape addition & removal

        protected void AddStrokesDirectionLines(Stroke stroke)
        {
            Shape shape = StrokeShapes[stroke].Find(s => (string)s.Tag == TAG_DIRECTION);
            if (shape != null)
                shape.Visibility = Visibility.Visible;

            else
            {
                bool isServiceStroke = stroke.Number == 1;
                if (PlacementValuesValid(stroke.Placement))
                {
                    double X1, X2, Y1, Y2;

                    if (isServiceStroke)
                    {
                        X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : GetAdjustedX(stroke, stroke.Playerposition);
                        Y1 = View_InnerFieldBehindGrid.ActualHeight; // service strokes start at the bottom
                    }
                    else
                    {
                        var precedingStroke = stroke.Rally.Strokes[stroke.Number - 2];
                        double precedingStartX, precedingStartY, precedingEndX, precedingEndY;

                        if (precedingStroke.Number == 1)
                        {
                            precedingStartX = GetAdjustedX(stroke, precedingStroke.Playerposition);
                            precedingStartY = GetSecondStrokePrecedingStartY(); // magic
                        }
                        else
                        {
                            precedingStartX = GetAdjustedX(stroke, stroke.Rally.Strokes[stroke.Number - 3].Placement.WX);
                            precedingStartY = GetAdjustedY(stroke, stroke.Rally.Strokes[stroke.Number - 3].Placement.WY);
                        }

                        precedingEndX = GetAdjustedX(stroke, precedingStroke.Placement.WX);
                        precedingEndY = GetAdjustedY(stroke, precedingStroke.Placement.WY);


                        if (ShowDebug)
                            AddDebugLine(stroke, precedingStartX, precedingStartY, precedingEndX, precedingEndY, false);

                        if (precedingStartY > precedingEndY)    // bottom -> top
                            Y1 = stroke.PointOfContact == STROKE_ATTR_POC_OVER ? precedingEndY - 30 : 0;
                        else
                            Y1 = stroke.PointOfContact == STROKE_ATTR_POC_OVER ? precedingEndY + 30 : GetGridForStroke(stroke).ActualHeight;

                        X1 = GetLinearContinuationX(precedingStartX, precedingStartY, precedingEndX, precedingEndY, Y1);

                        if (ShowDebug)
                            AddDebugLine(stroke, precedingEndX, precedingEndY, X1, Y1, true);
                    }

                    X2 = GetAdjustedX(stroke, stroke.Placement.WX);
                    Y2 = GetAdjustedY(stroke, stroke.Placement.WY);

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

                    shape.Tag = TAG_DIRECTION;
                    shape.StrokeThickness = STROKE_THICKNESS;
                    shape.Stroke = Brushes.Black;

                    AttachEventHandlerToShape(shape, stroke);

                    StrokeShapes[stroke].Add(shape);

                    ApplyStyle(stroke, shape);

                    if (isServiceStroke)
                        View_InnerFieldBehindGrid.Children.Add(shape);
                    else
                    {
                        if (stroke.PointOfContact == STROKE_ATTR_POC_BEHIND)
                            View_InnerFieldBehindGrid.Children.Add(shape);
                        else if (stroke.PointOfContact == STROKE_ATTR_POC_HALFDISTANCE)
                            View_InnerFieldHalfDistanceGrid.Children.Add(shape);
                        else
                            View_InnerFieldGrid.Children.Add(shape);
                    }

                    if (!ShowDirection)
                        shape.Visibility = Visibility.Hidden;
                }
                else
                {
                    Debug.WriteLine("invalid Placement of stroke {0} in rally {3}: x={1} y={2}", stroke.Number, stroke.Placement.WX, stroke.Placement.WY, stroke.Rally.Number);
                }
            }
        }

        protected void AddInterceptArrows(Stroke stroke)
        {
            List<Shape> interceptShapes = StrokeShapes[stroke].FindAll(s => (string)s.Tag == TAG_INTERCEPT);
            if (interceptShapes != null && interceptShapes.Count != 0)
                foreach (Shape interceptShape in interceptShapes)
                    interceptShape.Visibility = Visibility.Visible;

            else
            {
                bool isServiceStroke = stroke.Number == 1;
                if (PlacementValuesValid(stroke.Placement))
                {
                    foreach (Shape shape in StrokeShapes[stroke])
                    {
                        if ((string)shape.Tag == TAG_DIRECTION)
                        {
                            if (stroke.Number >= stroke.Rally.Strokes.Count)
                                return;

                            Stroke followingStroke = stroke.Rally.Strokes[stroke.Number];
                            Grid followingStrokeGrid = GetGridForStroke(followingStroke);

                            double x1, y1, x2, y2;
                            GetPointForShapeRelativeToGrid(shape, PointType.Start, followingStrokeGrid, out x1, out y1);
                            GetPointForShapeRelativeToGrid(shape, PointType.End, followingStrokeGrid, out x2, out y2);

                            double xE, yE;

                            if (y1 > y2)
                                yE = followingStroke.PointOfContact == STROKE_ATTR_POC_OVER ? y2 - 30 : 0;
                            else
                                yE = followingStroke.PointOfContact == STROKE_ATTR_POC_OVER ? y2 + 30 : followingStrokeGrid.ActualHeight;
                            xE = GetLinearContinuationX(x1, y1, x2, y2, yE);

                            if (xE.Equals(double.NaN) || yE.Equals(double.NaN))
                            {
                                Console.Out.WriteLine("NaN extrapolated values (xE={2} yE={3}) for stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number, xE, yE);
                                return;
                            }

                            // line
                            Line interceptLine = new Line();
                            interceptLine.X1 = x2;
                            interceptLine.Y1 = y2;
                            interceptLine.X2 = xE;
                            interceptLine.Y2 = yE;

                            interceptLine.Tag = TAG_INTERCEPT;
                            interceptLine.Stroke = Brushes.Black;
                            interceptLine.Fill = Brushes.Black;
                            interceptLine.StrokeThickness = STROKE_THICKNESS_INTERCEPT;

                            DoubleCollection dashes = new DoubleCollection();
                            dashes.Add(1);
                            interceptLine.StrokeDashArray = dashes;

                            ApplyStyle(stroke, interceptLine);

                            AttachEventHandlerToShape(interceptLine, stroke);

                            StrokeShapes[stroke].Add(interceptLine);

                            if (!ShowIntercept)
                                interceptLine.Visibility = Visibility.Hidden;

                            followingStrokeGrid.Children.Add(interceptLine);
                            // ---

                            // arrow tip
                            PathGeometry arrowTipGeometry = new PathGeometry();

                            PathFigure pathFigure = new PathFigure();
                            pathFigure.IsClosed = true;
                            pathFigure.StartPoint = new Point(xE - 1.5, yE - 3.5);

                            LineSegment ltt = new LineSegment(new Point(xE, yE), true);
                            pathFigure.Segments.Add(ltt);

                            LineSegment ttr = new LineSegment(new Point(xE + 1.5, yE - 3.5), true);
                            pathFigure.Segments.Add(ttr);

                            double theta = Math.Atan2((yE - y2), (xE - x2)) * 180 / Math.PI;
                            RotateTransform transform = new RotateTransform();
                            transform.Angle = theta - 90;
                            transform.CenterX = xE;
                            transform.CenterY = yE;
                            arrowTipGeometry.Transform = transform;

                            arrowTipGeometry.Figures.Add(pathFigure);

                            Path interceptArrowTip = new Path();
                            interceptArrowTip.Data = arrowTipGeometry;
                            interceptArrowTip.Tag = TAG_INTERCEPT;
                            interceptArrowTip.Stroke = Brushes.Black;
                            interceptArrowTip.Fill = Brushes.Black;
                            interceptArrowTip.StrokeThickness = STROKE_THICKNESS_INTERCEPT;

                            ApplyStyle(stroke, interceptArrowTip);

                            AttachEventHandlerToShape(interceptArrowTip, stroke);                            

                            StrokeShapes[stroke].Add(interceptArrowTip);

                            if (!ShowIntercept)
                                interceptArrowTip.Visibility = Visibility.Hidden;

                            followingStrokeGrid.Children.Add(interceptArrowTip);
                            // ---

                            return;
                        }
                    }
                }
            }
        }

        protected void AddDebugLine(Stroke stroke, double precedingStartX, double precedingStartY, double precedingEndX, double precedingEndY, bool dashed)
        {
            if (!precedingStartX.Equals(double.NaN) && !precedingStartY.Equals(double.NaN) && !precedingEndX.Equals(double.NaN) && !precedingEndY.Equals(double.NaN))
            {
                Line l = new Line();
                l.X1 = precedingStartX; l.Y1 = precedingStartY;
                l.X2 = precedingEndX; l.Y2 = precedingEndY;
                l.Stroke = Brushes.Black;
                l.StrokeThickness = STROKE_THICKNESS_DEBUG_PRECEDING;
                l.Tag = TAG_DEBUG_PRECEDING;
                if (dashed)
                {
                    DoubleCollection dashes = new DoubleCollection();
                    dashes.Add(2);
                    l.StrokeDashArray = dashes;
                }
                GetGridForStroke(stroke).Children.Add(l);
                StrokeShapes[stroke].Add(l);
            }
        }

        protected void AddStrokesArrowtips(Stroke stroke)
        {
            Shape strokeArrowTip = StrokeShapes[stroke].Find(s => (string)s.Tag == TAG_ARROW_TIP);
            if (strokeArrowTip != null)
                strokeArrowTip.Visibility = Visibility.Visible;

            else
            {
                bool isServiceStroke = stroke.Number == 1;
                if (PlacementValuesValid(stroke.Placement))
                {
                    double X1, X2, Y1, Y2;
                    X1 = Y1 = int.MinValue;

                    Grid gridOfStroke = GetGridForStroke(stroke);
                    if (isServiceStroke)
                    {
                        X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : GetAdjustedX(stroke, stroke.Playerposition);
                        Y1 = View_InnerFieldBehindGrid.ActualHeight; // service strokes start at the bottom
                    }
                    else
                    {
                        Shape shape = StrokeShapes[stroke].Find(s => (string)s.Tag == TAG_DIRECTION);
                        GetPointForShapeRelativeToGrid(shape, PointType.Middle, gridOfStroke, out X1, out Y1);
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

                    strokeArrowTip = new Path();
                    ((Path)strokeArrowTip).Data = arrowTipGeometry;
                    strokeArrowTip.Tag = TAG_ARROW_TIP;
                    strokeArrowTip.Stroke = Brushes.Black;
                    strokeArrowTip.Fill = Brushes.Black;
                    strokeArrowTip.StrokeThickness = STROKE_THICKNESS;

                    ApplyStyle(stroke, strokeArrowTip);

                    AttachEventHandlerToShape(strokeArrowTip, stroke);

                    StrokeShapes[stroke].Add(strokeArrowTip);

                    gridOfStroke.Children.Add(strokeArrowTip);
                }
                else
                {
                    Debug.WriteLine("invalid Placement of stroke {0} in rally {3}: x={1} y={2}", stroke.Number, stroke.Placement.WX, stroke.Placement.WY, stroke.Rally.Number);
                }
            }
        }

        protected void AddServiceStrokesSpinArrows(Stroke stroke)
        {
            Shape spinArrow = StrokeShapes[stroke].Find(s => (string)s.Tag == TAG_SPIN_ARROW);
            if (spinArrow != null)
                spinArrow.Visibility = Visibility.Visible;

            else
            {
                if (PlacementValuesValid(stroke.Placement))
                {
                    if (stroke.Spin == null || stroke.Spin.No == "1")
                        return;

                    double X1, Y1;

                    X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : GetAdjustedX(stroke, stroke.Playerposition);
                    Y1 = View_InnerFieldSpinGrid.ActualHeight - 1;

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

                    spinArrow = new Path();
                    ((Path)spinArrow).Data = arrowTipGeometry;
                    spinArrow.Tag = TAG_SPIN_ARROW;
                    spinArrow.Stroke = Brushes.Blue;
                    spinArrow.StrokeThickness = STROKE_THICKNESS_SPIN_ARROW;

                    AttachEventHandlerToShape(spinArrow, stroke);

                    StrokeShapes[stroke].Add(spinArrow);

                    if (!ShowSpin)
                        spinArrow.Visibility = Visibility.Hidden;

                    View_InnerFieldSpinGrid.Children.Add(spinArrow);
                }
                else
                {
                    Debug.WriteLine("invalid Placement of stroke {0} in rally {3}: x={1} y={2}", stroke.Number, stroke.Placement.WX, stroke.Placement.WY, stroke.Rally.Number);
                }
            }
        }

        private void HideShapesByTag(string tag)
        {
            foreach (List<Shape> shapes in StrokeShapes.Values)
            {
                foreach (Shape shape in shapes)
                {
                    if ((string)shape.Tag == tag)
                    {
                        shape.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        #endregion

        #region Shape creation

        private Shape GetLobShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineTwoThirdsX, lineTwoThirdsY;

            lineTwoThirdsY = Y1 - (2d / 3d) * (Y1 - Y2);
            if (stroke.Side == STROKE_ATTR_SIDE_BACKHAND)
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

            PathGeometry lobGeometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure();

            Point startPoint = new Point(X1, Y1);
            Point endPoint = new Point(X2, Y2);
            Point controlPoint = new Point(cpx, cpy);

            pathFigure.StartPoint = startPoint;

            QuadraticBezierSegment segment = new QuadraticBezierSegment(controlPoint, endPoint, true);
            pathFigure.Segments.Add(segment);

            lobGeometry.Figures.Add(pathFigure);

            Path lobPath = new Path();
            lobPath.Data = lobGeometry;

            if (stroke.Side == STROKE_ATTR_SIDE_BACKHAND)
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                lobPath.StrokeDashArray = dashes;
            }
            return lobPath;
        }

        private Shape GetChopShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineNineteenTwentiethsX, lineNineteenTwentiethsY;

            lineNineteenTwentiethsY = Y1 - (19d / 20d) * (Y1 - Y2);
            if (stroke.Side == STROKE_ATTR_SIDE_BACKHAND)
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

            if (stroke.Side == STROKE_ATTR_SIDE_BACKHAND)
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                chopPath.StrokeDashArray = dashes;
            }
            return chopPath;
        }

        private Shape GetTopSpinShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineFourFifthsX, lineFourFifthsY;

            lineFourFifthsY = Y1 - (4d / 5d) * (Y1 - Y2);
            if (stroke.Side == STROKE_ATTR_SIDE_BACKHAND)
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

            if (stroke.Side == STROKE_ATTR_SIDE_BACKHAND)
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                topSpinPath.StrokeDashArray = dashes;
            }
            return topSpinPath;
        }

        private Shape GetBananaShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineMiddleX;

            // banana is always backhand -> left curvature
            m = (X2 - X1) / (Y1 - Y2);
            lineMiddleX = (X2 + X1) / 2;
            cpx = lineMiddleX - 0.25 * Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y1 - Y2, 2));
            cpy = (Y1 + Y2) / 2 - m * (lineMiddleX - cpx);

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

            if (stroke.Side == STROKE_ATTR_SIDE_BACKHAND)
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                bananaPath.StrokeDashArray = dashes;
            }
            return bananaPath;
        }

        private Shape GetLineShape(Stroke stroke, double x1, double y1, double x2, double y2)
        {
            Line line = new Line();
            line.X1 = x1; line.Y1 = y1; line.X2 = x2; line.Y2 = y2;

            //Debug.WriteLine("stroke {5} of rally {4}: x1={0} y1={1} -> x2={2} y2={3}", x1, y1, x2, y2, stroke.Rally.Number, stroke.Number);

            if (stroke.Side == STROKE_ATTR_SIDE_BACKHAND)
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                line.StrokeDashArray = dashes;
            }
            return line;
        }

        #endregion

        #region Style

        private void ApplyStyle(Stroke stroke, Shape shape)
        {
            if (stroke.Number == 1)
            {
                if (stroke.Spin != null && stroke.Spin.No != "1")
                    ApplyStyle(shape, STROKE_ATTR_HAS_SPIN);
            }
            else
            {
                ApplyStyle(shape, stroke.Stroketechnique.Type);
            }

            if (stroke.Side == STROKE_ATTR_SIDE_BACKHAND && (string)shape.Tag != TAG_ARROW_TIP && (string)shape.Tag != TAG_INTERCEPT)
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                shape.StrokeDashArray = dashes;
            }
        }

        private void ApplyStyle(Shape shape, string style)
        {
            switch (style)
            {                
                case STROKE_ATTR_TECHNIQUE_PUSH:
                case STROKE_ATTR_TECHNIQUE_CHOP:
                    shape.Stroke = Brushes.Red;
                    if (shape is Path && (string)shape.Tag != TAG_DIRECTION)
                        shape.Fill = Brushes.Red;
                    break;
                case STROKE_ATTR_TECHNIQUE_FLIP:
                case STROKE_ATTR_TECHNIQUE_OPTION_BANANA:
                    shape.Stroke = Brushes.Yellow;
                    if (shape is Path && (string)shape.Tag != TAG_DIRECTION)
                        shape.Fill = Brushes.Yellow;
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
                    if (shape is Path && (string)shape.Tag != TAG_DIRECTION)
                        shape.Fill = Brushes.Green;
                    break;
            }
        }

        #endregion Style

        #region Helper methods

        protected abstract void AttachEventHandlerToShape(Shape interceptArrowTip, Stroke stroke);

        protected abstract double GetAdjustedX(Stroke stroke, double oldX);

        protected abstract double GetAdjustedY(Stroke stroke, double oldY);

        /// <summary>
        /// Not 100% sure why, but we need this method - small and large table need exact opposite values,
        /// but only for second stroke
        /// </summary>
        protected abstract double GetSecondStrokePrecedingStartY();

        protected Grid GetGridForStroke(Stroke stroke)
        {
            if (stroke.Number == 1)
                return View_InnerFieldBehindGrid;
            else if (stroke.PointOfContact == STROKE_ATTR_POC_OVER)
                return View_InnerFieldGrid;
            else if (stroke.PointOfContact == STROKE_ATTR_POC_BEHIND)
                return View_InnerFieldBehindGrid;
            else if (stroke.PointOfContact == STROKE_ATTR_POC_HALFDISTANCE)
                return View_InnerFieldHalfDistanceGrid;
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

        private void GetPointForShapeRelativeToGrid(Shape shape, PointType which, Grid grid, out double x, out double y)
        {
            if (shape is Path)
            {
                Geometry geom = ((Path)shape).Data;
                if (geom is PathGeometry)
                {
                    PathFigure fig = ((PathGeometry)geom).Figures[0];
                    switch (which)
                    {
                        case PointType.Middle:
                        case PointType.End:
                            PathSegment seg = fig.Segments[0];
                            if (seg is QuadraticBezierSegment)
                            {
                                QuadraticBezierSegment quadSeg = (QuadraticBezierSegment)seg;
                                if (which == PointType.Middle)
                                {
                                    x = quadSeg.Point1.X;
                                    y = quadSeg.Point1.Y;
                                }
                                else
                                {
                                    x = quadSeg.Point2.X;
                                    y = quadSeg.Point2.Y;
                                }
                                break;
                            }
                            else
                            {
                                throw new ArgumentException("point for shape not defined! first segment of first figure of " + shape + " is not a QuadraticBezierSegment.");
                            }
                        default:
                        case PointType.Start:
                            x = fig.StartPoint.X;
                            y = fig.StartPoint.Y;
                            break;
                    }
                }
                else
                    throw new ArgumentException("point for shape not defined! data of " + shape + " is not a PathGeometry.");
            }
            else if (shape is Line)
            {
                Line line = (Line)shape;
                switch (which)
                {
                    case PointType.End:
                        x = line.X2;
                        y = line.Y2;
                        break;
                    default:
                    case PointType.Start:
                        x = line.X1;
                        y = line.Y1;
                        break;
                }
            }
            else
            {
                throw new ArgumentException("point for shape not defined! shape " + shape + " is neither Path nor Line.");
            }

            Grid parent = (Grid)shape.Parent;
            if (parent.Name != grid.Name)
            {
                y -= grid.Margin.Top - parent.Margin.Top;
            }
        }

        private double GetLinearContinuationX(double x1, double y1, double x2, double y2, double targetY)
        {
            return ((targetY - y1) / (y2 - y1)) * (x2 - x1) + x1;
        }

        protected static bool PlacementValuesValid(Placement placement)
        {
            return placement.WX != double.NaN && placement.WX > 0 && placement.WY != double.NaN && placement.WY > 0;
        }

        #endregion

        private enum PointType { Start, Middle, End }

    }
}
