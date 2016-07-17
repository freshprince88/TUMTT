using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TT.Models;

namespace TT.Viewer.Views
{
    public abstract class TableView : UserControl
    {
        #region Constants

        protected const double StrokeThickness = 1.75;
        protected const double StrokeThicknessHover = 2.5;
        protected const double StrokeThicknessSpinArrow = 1;
        protected const double StrokeThicknessSpinArrowHover = 2;
        protected const double StrokeThicknessSmash = 3.5;
        protected const double StrokeThicknessSmashHover = 5;
        protected const double StrokeThicknessPreceding_Debug = 0.5;
        protected const double StrokeThicknessPrecedingHover_Debug = 0.7;
        protected const double StrokeThicknessIntercept = 1.0;
        protected const double StrokeThicknessInterceptHover = 1.7;

        protected const double BananaCurveRation = 0.5;
        protected const double TopspinCurveRation = 0.8;
        protected const double ChopCurveRation = 0.95;
        protected const double LobCurveRation = 2d / 3d;

        protected const string StrokeAttrTechniqueOptionBanana = "Banana";  // what is 'option'? TODO: convert to enum!        

        #endregion

        protected Dictionary<Stroke, List<Shape>> StrokeShapes { get; private set; }
        protected Dictionary<Stroke, List<FrameworkElement>> StrokeElements { get; private set; }

        public abstract Grid View_InnerFieldGrid { get; }
        public abstract Grid View_InnerFieldBehindGrid { get; }
        public abstract Grid View_InnerFieldHalfDistanceGrid { get; }
        public abstract Grid View_InnerFieldSpinGrid { get; }

        public TableView()
        {
            StrokeShapes = new Dictionary<Stroke, List<Shape>>();
            StrokeElements = new Dictionary<Stroke, List<FrameworkElement>>();
        }

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

        public bool ShowNumbers
        {
            get { return (bool)GetValue(ShowNumbersProperty); }
            set { SetValue(ShowNumbersProperty, value); }
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

        public static DependencyProperty ShowNumbersProperty = DependencyProperty.Register(
            "ShowNumbers", typeof(bool), typeof(TableView), new PropertyMetadata(true, new PropertyChangedCallback(OnDisplayTypePropertyChanged)));

        #endregion

        #region Event handlers

        private static void OnStrokesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Debug.WriteLine("OnStrokesPropertyChanged sender={0}, sender.Tag={3} e.ov={1}, e.nv={2}", sender, e.OldValue, e.NewValue, ((TableView)sender).Tag);
            
             ((TableView)sender).ProcessStrokes(new List<Stroke>((ICollection<Stroke>)e.NewValue));
        }

        private static void OnDisplayTypePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Debug.WriteLine("OnDisplayTypePropertyChanged sender={0}, sender.Tag={3} e.ov={1}, e.nv={2}", sender, e.OldValue, e.NewValue, ((TableView)sender).Tag);

            if (sender is TableView)
            {
                TableView view = (TableView)sender;

                if (e.Property == ShowDirectionProperty)
                {
                    if ((bool)e.NewValue)
                        foreach (Stroke s in view.StrokeShapes.Keys) view.AddStrokesDirectionShapes(s);
                    else
                        view.HideShapesByTag(ShapeType.Direction);
                }
                else if (e.Property == ShowSpinProperty)
                {
                    if ((bool)e.NewValue)
                        foreach (Stroke s in view.StrokeShapes.Keys) view.AddServiceStrokesSpinShapes(s);
                    else
                        view.HideShapesByTag(ShapeType.SpinShape);
                }
                else if (e.Property == ShowDebugProperty)
                {
                    // you'll have to reload the view to display debug lines again :(
                    if (!(bool)e.NewValue)
                        view.HideShapesByTag(ShapeType.Debug_preceding);
                }
                else if (e.Property == ShowInterceptProperty)
                {
                    if ((bool)e.NewValue)
                        foreach (Stroke s in view.StrokeShapes.Keys) view.AddInterceptArrows(s);
                    else
                        view.HideShapesByTag(ShapeType.Intercept);
                }
                else if (e.Property == ShowNumbersProperty)
                {
                    if ((bool)e.NewValue)
                        foreach (Stroke s in view.StrokeShapes.Keys) view.AddStrokeNumbers(s);
                    else
                        view.HideElementsByTag(ElementType.StrokeNumber);
                }
            }
        }

        protected abstract void ProcessStrokes(List<Stroke> strokes);

        #endregion

        #region Shape addition & removal

        protected void AddStrokesDirectionShapes(Stroke stroke)
        {
            // first check if we already created a direction shape for this stroke. if so, display it and we're done
            Shape shape = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Direction);
            if (shape != null)
                shape.Visibility = Visibility.Visible;

            else
            {
                bool isServiceStroke = stroke.Number == 1;
                bool isNetOrOut = stroke.EnumCourse == Models.Util.Enums.Stroke.Course.NetOut;

                try
                {
                    if (!PlacementValuesValid(stroke.Placement) && !isNetOrOut)
                    {
                        Debug.WriteLine("Direction: invalid Placement of stroke {0} in rally {3}: x={1} y={2}", stroke.Number, stroke.Placement != null ? stroke.Placement.WX.ToString() : "[n/a]", stroke.Placement != null ? stroke.Placement.WY.ToString() : "[n/a]", stroke.Rally.Number);
                        return;
                    }

                    double X1, X2, Y1, Y2;

                    // here we figure out where the stroke should start, i.e. its X1 and Y1 coordinate
                    if (isServiceStroke)
                    {
                        // service strokes get special treatment because they
                        // a. generally start from the bottom in most views (small/large)
                        // b. don't have any preceding strokes to base their starting point on

                        if (stroke.Playerposition.Equals(double.NaN))
                            return;

                        // double.MinValue as Playerposition indicates a service stroke in the Legend view
                        // since there the arrows are painted horizontally, we have to set special values here
                        if (stroke.Playerposition == double.MinValue)
                        {
                            X1 = 0;
                            Y1 = stroke.Placement.WY;
                        }
                        // other than that, service strokes start from the bottom (height of the grid they will be added to)
                        // and their position is determined by Playerposition
                        else
                        {
                            X1 = GetAdjustedX(stroke, stroke.Playerposition);
                            Y1 = View_InnerFieldBehindGrid.ActualHeight;
                        }
                    }
                    else
                    {
                        // we don't have a service stroke on our hands, that means we base this strokes starting point
                        // on one or more of his predecessors

                        var precedingStroke = stroke.Rally.Strokes[stroke.Number - 2];
                        double precedingStartX, precedingStartY, precedingEndX, precedingEndY;

                        if (precedingStroke.Number == 1)
                        {
                            // the immediate predecessor is a service stroke.
                            // again, this calls for special treatment
                            precedingStartX = stroke.Playerposition.Equals(double.NaN) ? 0 : GetAdjustedX(stroke, precedingStroke.Playerposition);
                            precedingStartY = GetSecondStrokePrecedingStartY(); // this cannot be generalized, both small and large view return different (even opposing) values
                        }
                        else
                        {
                            // the preceding stroke was not a service stroke, i.e. it has at least one other preceding stroke
                            // whose placement (WX, WY) we can take for an approximation of this strokes starting point
                            precedingStartX = GetAdjustedX(stroke, stroke.Rally.Strokes[stroke.Number - 3].Placement.WX);
                            precedingStartY = GetAdjustedY(stroke, stroke.Rally.Strokes[stroke.Number - 3].Placement.WY);
                        }

                        // the end point for this strokes starting point approximation is always the preceding stroke's placement
                        precedingEndX = GetAdjustedX(stroke, precedingStroke.Placement.WX);
                        precedingEndY = GetAdjustedY(stroke, precedingStroke.Placement.WY);


                        if (ShowDebug)
                            AddDebugLine(stroke, precedingStartX, precedingStartY, precedingEndX, precedingEndY, false);

                        // depending on whether we draw the stroke from top to bottom or vice versa, the stroke will start at Y '0' (top)
                        // or the height of the grid the stroke is going to be added to (bottom).
                        // in the case that its point of contact is OVER the table, we need the height of the previous stroke (we add/subtract arbitrarilly '30' for realism)
                        if (precedingStartY > precedingEndY)    // bottom -> top
                            Y1 = stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? precedingEndY - 30 : 0;
                        else if (precedingStartY < precedingEndY)
                            Y1 = stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? precedingEndY + 30 : GetGridForStroke(stroke).ActualHeight;
                        else
                            Y1 = precedingEndY; // means the preceding stroke didn't change the Y coordinate -> reserved for special cases (e.g. horizontal strokes in Legend)

                        // now we have the starting and end point (both X and Y) of the previous stroke and the height at which this stroke should end
                        // -> extrapolate its X-coordinate
                        if (precedingStartY != precedingEndY)
                            X1 = GetLinearContinuationX(precedingStartX, precedingStartY, precedingEndX, precedingEndY, Y1);
                        else
                            X1 = precedingEndX > precedingStartX ? GetGridForStroke(stroke).ActualWidth : 0;    // equal preceding stroke's Y coordinate -> horizontal

                        if (ShowDebug)
                            AddDebugLine(stroke, precedingEndX, precedingEndY, X1, Y1, true);
                    }

                    if (isNetOrOut)
                    {
                        X2 = X1;
                        if (Y1 > 137)
                            Y2 = stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? Y1 - 40 : GetGridForStroke(stroke).ActualHeight - 40;
                        else
                            Y2 = stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? Y1 + 40 : 40;
                    }
                    else
                    {
                        // this stroke's end is always the exact position of its placement
                        X2 = GetAdjustedX(stroke, stroke.Placement.WX);
                        Y2 = GetAdjustedY(stroke, stroke.Placement.WY);
                    }

                    // now we have this stroke's start and end => get the shape (depends mostly on stroke technique)
                    if (isServiceStroke || isNetOrOut)
                        shape = GetLineShape(stroke, X1, Y1, X2, Y2);
                    else
                        if (stroke.Stroketechnique.EnumType == Models.Util.Enums.Stroke.Technique.Flip || stroke.Stroketechnique.Option == StrokeAttrTechniqueOptionBanana)
                        shape = GetBananaShape(stroke, X1, Y1, X2, Y2);
                    else if (stroke.Stroketechnique.EnumType == Models.Util.Enums.Stroke.Technique.Topspin)
                        shape = GetTopSpinShape(stroke, X1, Y1, X2, Y2);
                    else if (stroke.Stroketechnique.EnumType == Models.Util.Enums.Stroke.Technique.Chop)
                        shape = GetChopShape(stroke, X1, Y1, X2, Y2);
                    else if (stroke.Stroketechnique.EnumType == Models.Util.Enums.Stroke.Technique.Lob)
                        shape = GetLobShape(stroke, X1, Y1, X2, Y2);
                    else
                        shape = GetLineShape(stroke, X1, Y1, X2, Y2);

                    // tag is needed to identify this shape in our map of strokes to shapes
                    shape.Tag = ShapeType.Direction;

                    shape.StrokeThickness = StrokeThickness;

                    // if this view needs to receive event notifications when user interacts with the stroke shape
                    AttachEventHandlerToShape(shape, stroke);

                    // add the stroke to our map so we can find it later
                    StrokeShapes[stroke].Add(shape);

                    // apply style (also depends mostly on stroke technique, but also side - forehand/backhand - and other)
                    ApplyStyle(stroke, shape);

                    // now add it to the respective grid
                    if (isServiceStroke)
                        View_InnerFieldBehindGrid.Children.Add(shape);
                    else
                    {
                        if (stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Behind)
                            View_InnerFieldBehindGrid.Children.Add(shape);
                        else if (stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.HalfDistance)
                            View_InnerFieldHalfDistanceGrid.Children.Add(shape);
                        else
                            View_InnerFieldGrid.Children.Add(shape);
                    }

                    // hide it immediately, if the "Direction" checkbox is unchecked
                    if (!ShowDirection)
                        shape.Visibility = Visibility.Hidden;
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        protected void AddInterceptArrows(Stroke stroke)
        {
            List<Shape> interceptShapes = StrokeShapes[stroke].FindAll(s => (ShapeType)s.Tag == ShapeType.Intercept);
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
                        if ((ShapeType)shape.Tag == ShapeType.Direction)
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
                                yE = followingStroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? y2 - 30 : 0;
                            else if (y1 < y2)
                                yE = followingStroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? y2 + 30 : followingStrokeGrid.ActualHeight;
                            else
                                yE = y2;

                            if (y1 != y2)
                                xE = GetLinearContinuationX(x1, y1, x2, y2, yE);
                            else
                                xE = x2 > x1 ? followingStrokeGrid.ActualWidth : 0;

                            if (xE.Equals(double.NaN) || yE.Equals(double.NaN))
                            {
                                Debug.WriteLine("Intercept: NaN extrapolated values (xE={2} yE={3}) for stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number, xE, yE);
                                return;
                            }

                            // line
                            Line interceptLine = new Line();
                            interceptLine.X1 = x2;
                            interceptLine.Y1 = y2;
                            interceptLine.X2 = xE;
                            interceptLine.Y2 = yE;

                            interceptLine.Tag = ShapeType.Intercept;
                            interceptLine.StrokeThickness = StrokeThicknessIntercept;

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
                            interceptArrowTip.Tag = ShapeType.Intercept;
                            interceptArrowTip.StrokeThickness = StrokeThicknessIntercept;

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
                l.StrokeThickness = StrokeThicknessPreceding_Debug;
                l.Tag = ShapeType.Debug_preceding;
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
            Shape strokeArrowTip = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Arrowtip);
            if (strokeArrowTip != null)
                strokeArrowTip.Visibility = Visibility.Visible;

            else
            {
                bool isServiceStroke = stroke.Number == 1;
                bool isNetOrOut = stroke.EnumCourse == Models.Util.Enums.Stroke.Course.NetOut;
                if (PlacementValuesValid(stroke.Placement) || isNetOrOut)
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
                        Shape shape = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Direction);
                        if (shape == null) return;
                        GetPointForShapeRelativeToGrid(shape, PointType.Middle, gridOfStroke, out X1, out Y1);
                    }

                    if (isNetOrOut)
                    {
                        Shape shape = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Direction);
                        if (shape == null) return;
                        GetPointForShapeRelativeToGrid(shape, PointType.End, gridOfStroke, out X2, out Y2);
                    }
                    else
                    {
                        X2 = GetAdjustedX(stroke, stroke.Placement.WX);
                        Y2 = GetAdjustedY(stroke, stroke.Placement.WY);
                    }

                    PathGeometry arrowTipGeometry = new PathGeometry();
                    PathFigure pathFigure = new PathFigure();

                    if (isNetOrOut)
                    {
                        pathFigure.StartPoint = new Point(X2, Y2 - 6);

                        LineSegment ttb = new LineSegment(new Point(X2, Y2 + 6), true);
                        pathFigure.Segments.Add(ttb);

                        arrowTipGeometry.Figures.Add(pathFigure);

                        pathFigure = new PathFigure();
                        pathFigure.StartPoint = new Point(X2 - 6, Y2);

                        LineSegment ltr = new LineSegment(new Point(X2 + 6, Y2), true);
                        pathFigure.Segments.Add(ltr);

                        arrowTipGeometry.Figures.Add(pathFigure);

                        RotateTransform transform = new RotateTransform();
                        transform.Angle = 45;
                        transform.CenterX = X2;
                        transform.CenterY = Y2;
                        arrowTipGeometry.Transform = transform;
                    }
                    else
                    {
                        pathFigure = new PathFigure();
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

                    }

                    arrowTipGeometry.Figures.Add(pathFigure);

                    strokeArrowTip = new Path();
                    ((Path)strokeArrowTip).Data = arrowTipGeometry;
                    strokeArrowTip.Tag = ShapeType.Arrowtip;
                    strokeArrowTip.StrokeThickness = StrokeThickness;

                    ApplyStyle(stroke, strokeArrowTip);

                    AttachEventHandlerToShape(strokeArrowTip, stroke);

                    StrokeShapes[stroke].Add(strokeArrowTip);

                    gridOfStroke.Children.Add(strokeArrowTip);
                }
                else
                {
                    Debug.WriteLine("Arrowtip: invalid Placement of stroke {0} in rally {3}: x={1} y={2}", stroke.Number, stroke.Placement != null ? stroke.Placement.WX.ToString() : "[n/a]", stroke.Placement != null ? stroke.Placement.WY.ToString() : "[n/a]", stroke.Rally.Number);
                }
            }
        }

        protected void AddServiceStrokesSpinShapes(Stroke stroke)
        {
            if (stroke.Number != 1)
                return;

            Shape spinArrow = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.SpinShape);
            if (spinArrow != null)
                spinArrow.Visibility = Visibility.Visible;

            else
            {
                if (PlacementValuesValid(stroke.Placement))
                {
                    double X1, Y1;

                    if (stroke.Playerposition == double.MinValue)   // service stroke in legend
                        X1 = 0;
                    else
                        X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : GetAdjustedX(stroke, stroke.Playerposition);

                    Y1 = View_InnerFieldSpinGrid.ActualHeight - 1;

                    Geometry spinGeometry;

                    if (stroke.Spin == null || stroke.Spin.No == "1" || (stroke.Spin.SL == "1" && stroke.Spin.SR == "1"))
                    {
                        spinGeometry = new EllipseGeometry();
                        EllipseGeometry ellipseGeometry = (EllipseGeometry)spinGeometry;
                        ellipseGeometry.Center = new Point(X1, Y1 - 4);
                        ellipseGeometry.RadiusX = 3;
                        ellipseGeometry.RadiusY = 3;
                    }
                    else
                    {
                        spinGeometry = new PathGeometry();

                        RotateTransform transform = new RotateTransform();
                        transform.Angle = GetRotationAngleForSpin(stroke.Spin);
                        transform.CenterX = X1;
                        transform.CenterY = Y1 - 4;
                        spinGeometry.Transform = transform;

                        PathFigure pathFigure = new PathFigure();
                        pathFigure.StartPoint = new Point(X1 - 3, Y1 - 5);

                        LineSegment btt = new LineSegment(new Point(X1, Y1 - 8), true);
                        pathFigure.Segments.Add(btt);

                        LineSegment ttl = new LineSegment(new Point(X1 + 3, Y1 - 5), true);
                        pathFigure.Segments.Add(ttl);

                        ((PathGeometry)spinGeometry).Figures.Add(pathFigure);

                        pathFigure = new PathFigure();
                        pathFigure.StartPoint = new Point(X1, Y1);

                        LineSegment ttr = new LineSegment(new Point(X1, Y1 - 8), true);
                        pathFigure.Segments.Add(ttr);

                        ((PathGeometry)spinGeometry).Figures.Add(pathFigure);
                    }

                    spinArrow = new Path();
                    ((Path)spinArrow).Data = spinGeometry;
                    spinArrow.Tag = ShapeType.SpinShape;
                    spinArrow.Stroke = Brushes.Blue;
                    spinArrow.StrokeThickness = StrokeThicknessSpinArrow;

                    AttachEventHandlerToShape(spinArrow, stroke);

                    StrokeShapes[stroke].Add(spinArrow);

                    if (!ShowSpin)
                        spinArrow.Visibility = Visibility.Hidden;

                    View_InnerFieldSpinGrid.Children.Add(spinArrow);
                }
                else
                {
                    Debug.WriteLine("SpinArrow: invalid Placement of stroke {0} in rally {3}: x={1} y={2}", stroke.Number, stroke.Placement != null ? stroke.Placement.WX.ToString() : "[n/a]", stroke.Placement != null ? stroke.Placement.WY.ToString() : "[n/a]", stroke.Rally.Number);
                }
            }
        }

        protected void AddStrokeNumbers(Stroke stroke)
        {
            FrameworkElement strokeNumber = StrokeElements[stroke].Find(s => (ElementType)s.Tag == ElementType.StrokeNumber);
            if (strokeNumber != null)
                strokeNumber.Visibility = Visibility.Visible;

            else
            {
                bool isNetOrOut = stroke.EnumCourse == Models.Util.Enums.Stroke.Course.NetOut;
                if (PlacementValuesValid(stroke.Placement) || isNetOrOut)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = stroke.Number.ToString();

                    textBlock.Width = 10;
                    textBlock.Height = 15;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                    textBlock.VerticalAlignment = VerticalAlignment.Top;

                    double X1, Y1;

                    Grid gridOfStroke = GetGridForStroke(stroke);

                    if (stroke.Number == 1)
                    {
                        X1 = GetAdjustedX(stroke, stroke.Playerposition);
                        Y1 = View_InnerFieldBehindGrid.ActualHeight - textBlock.Height;
                    }
                    else
                    {
                        Shape shape = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Direction);
                        if (shape == null) return;
                        GetPointForShapeRelativeToGrid(shape, PointType.Start, gridOfStroke, out X1, out Y1);
                    }

                    X1 = (X1.Equals(double.NaN) ? 0 : X1) + 5;
                    Y1 = (Y1.Equals(double.NaN) ? 0 : Y1);

                    Thickness margin = new Thickness(
                        Math.Min(X1, gridOfStroke.ActualWidth - textBlock.Width),
                        Math.Min(Y1, gridOfStroke.ActualHeight - textBlock.Height), 
                        0, 
                        0);
                    textBlock.Margin = margin;

                    textBlock.FontWeight = FontWeights.Bold;
                    textBlock.Foreground = isNetOrOut ? Brushes.DarkRed : Brushes.DarkOliveGreen;

                    textBlock.Tag = ElementType.StrokeNumber;

                    StrokeElements[stroke].Add(textBlock);

                    gridOfStroke.Children.Add(textBlock);

                    if (!ShowNumbers)
                        textBlock.Visibility = Visibility.Hidden;
                }
                else
                {
                    Debug.WriteLine("StrokeNumber: invalid Placement of stroke {0} in rally {3}: x={1} y={2}", stroke.Number, stroke.Placement != null ? stroke.Placement.WX.ToString() : "[n/a]", stroke.Placement != null ? stroke.Placement.WY.ToString() : "[n/a]", stroke.Rally.Number);
                }
            }
        }

        private void HideShapesByTag(ShapeType tag)
        {
            foreach (List<Shape> shapes in StrokeShapes.Values)
            {
                foreach (Shape shape in shapes)
                {
                    if ((ShapeType)shape.Tag == tag)
                    {
                        shape.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        private void HideElementsByTag(ElementType tag)
        {
            foreach (List<FrameworkElement> elements in StrokeElements.Values)
            {
                foreach (FrameworkElement element in elements)
                {
                    if ((ElementType)element.Tag == tag)
                    {
                        element.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        #endregion

        #region Shape creation

        protected virtual Shape GetLobShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineTwoThirdsX, lineTwoThirdsY;

            lineTwoThirdsY = Y1 - LobCurveRation * (Y1 - Y2);
            if ((Y1 > Y2 && stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Backhand) || (Y1 <= Y2 && stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Forehand))
            {
                m = (X2 - X1) / (Y1 - Y2);
                lineTwoThirdsX = X1 + LobCurveRation * (X2 - X1);
                cpx = lineTwoThirdsX - 0.7 * Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y1 - Y2, 2));
                cpy = lineTwoThirdsY - m * (lineTwoThirdsX - cpx);
            }
            else
            {
                m = (X1 - X2) / (Y1 - Y2);
                lineTwoThirdsX = X1 - LobCurveRation * (X1 - X2);
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

            if (stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Backhand)
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                lobPath.StrokeDashArray = dashes;
            }
            return lobPath;
        }

        protected virtual Shape GetChopShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineNineteenTwentiethsX, lineNineteenTwentiethsY;

            lineNineteenTwentiethsY = Y1 - ChopCurveRation * (Y1 - Y2);
            if ((Y1 > Y2 && stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Backhand) || (Y1 <= Y2 && stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Forehand))
            {
                m = (X2 - X1) / (Y1 - Y2);
                lineNineteenTwentiethsX = X1 + ChopCurveRation * (X2 - X1);
                cpx = lineNineteenTwentiethsX - 0.2 * Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y1 - Y2, 2));
                cpy = lineNineteenTwentiethsY - m * (lineNineteenTwentiethsX - cpx);
            }
            else
            {
                m = (X1 - X2) / (Y1 - Y2);
                lineNineteenTwentiethsX = X1 - ChopCurveRation * (X1 - X2);
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

            if (stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Backhand)
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                chopPath.StrokeDashArray = dashes;
            }
            return chopPath;
        }

        protected virtual Shape GetTopSpinShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineFourFifthsX, lineFourFifthsY;

            lineFourFifthsY = Y1 - TopspinCurveRation * (Y1 - Y2);
            if ((Y1 > Y2 && stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Backhand) || (Y1 <= Y2 && stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Forehand))
            {
                m = (X2 - X1) / (Y1 - Y2);
                lineFourFifthsX = X1 + TopspinCurveRation * (X2 - X1);
                cpx = lineFourFifthsX - 0.25 * Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y1 - Y2, 2));
                cpy = lineFourFifthsY - m * (lineFourFifthsX - cpx);
            }
            else
            {
                m = (X1 - X2) / (Y1 - Y2);
                lineFourFifthsX = X1 - TopspinCurveRation * (X1 - X2);
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

            if (stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Backhand)
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                topSpinPath.StrokeDashArray = dashes;
            }
            return topSpinPath;
        }

        protected virtual Shape GetBananaShape(Stroke stroke, double X1, double Y1, double X2, double Y2)
        {
            double m, cpx, cpy, lineMiddleX;

            // banana is always backhand -> left curvature (but only on large table, on small table it depends on Y1, Y2)
            m = (X2 - X1) / (Y1 - Y2);
            lineMiddleX = (X2 + X1) * BananaCurveRation;
            cpx = lineMiddleX + (Y1 > Y2 ? -1 : 1) * 0.25 * Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y1 - Y2, 2));
            cpy = (Y1 + Y2) * BananaCurveRation - m * (lineMiddleX - cpx);

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

            if (stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Backhand)
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

            if (stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Backhand)
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
                if (stroke.Spin != null && stroke.Spin.SL == "1" && stroke.Spin.SR == "1")    // special cases (e.g. Legend)
                {
                    shape.Stroke = Brushes.Black;
                    shape.Fill = Brushes.Black;
                }
                else
                {
                    shape.Stroke = Brushes.SaddleBrown;
                    shape.Fill = Brushes.SaddleBrown;
                }

            }
            else if (stroke.Stroketechnique != null)
            {
                switch (stroke.Stroketechnique.EnumType)
                {
                    case Models.Util.Enums.Stroke.Technique.Push:
                    case Models.Util.Enums.Stroke.Technique.Chop:
                        shape.Stroke = Brushes.Red;
                        if (shape is Path && (ShapeType)shape.Tag != ShapeType.Direction)
                            shape.Fill = Brushes.Red;
                        break;
                    case Models.Util.Enums.Stroke.Technique.Flip:
                    case Models.Util.Enums.Stroke.Technique.Banana:
                        shape.Stroke = Brushes.Yellow;
                        if (shape is Path && (ShapeType)shape.Tag != ShapeType.Direction)
                            shape.Fill = Brushes.Yellow;
                        break;
                    case Models.Util.Enums.Stroke.Technique.Smash:
                        shape.Stroke = Brushes.Blue;
                        shape.Fill = Brushes.Blue;
                        shape.StrokeThickness = StrokeThicknessSmash;
                        break;
                    case Models.Util.Enums.Stroke.Technique.Block:
                    case Models.Util.Enums.Stroke.Technique.Counter:
                        shape.Stroke = Brushes.Blue;
                        shape.Fill = Brushes.Blue;
                        break;
                    case Models.Util.Enums.Stroke.Technique.Miscellaneous:
                        shape.Stroke = Brushes.Green;
                        if (shape is Path && (ShapeType)shape.Tag != ShapeType.Direction)
                            shape.Fill = Brushes.Green;
                        break;
                    default:
                        shape.Stroke = Brushes.Black;
                        if (shape is Path && (ShapeType)shape.Tag != ShapeType.Direction)
                            shape.Fill = Brushes.Black;
                        break;
                }
            }

            if (stroke.EnumSide == Models.Util.Enums.Stroke.Hand.Backhand && (ShapeType)shape.Tag != ShapeType.Arrowtip && (ShapeType)shape.Tag != ShapeType.Intercept)
            {
                DoubleCollection dashes = new DoubleCollection();
                dashes.Add(2);
                shape.StrokeDashArray = dashes;
            }
        }

        #endregion Style

        #region Helper methods

        protected abstract void AttachEventHandlerToShape(Shape shape, Stroke stroke);

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
            else if (stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over)
                return View_InnerFieldGrid;
            else if (stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Behind)
                return View_InnerFieldBehindGrid;
            else if (stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.HalfDistance)
                return View_InnerFieldHalfDistanceGrid;
            else
                return View_InnerFieldGrid;
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
            if (parent != null && parent.Name != grid.Name)
                y -= grid.Margin.Top - parent.Margin.Top;            
        }

        private double GetLinearContinuationX(double x1, double y1, double x2, double y2, double targetY)
        {
            return ((targetY - y1) / (y2 - y1)) * (x2 - x1) + x1;
        }

        protected static bool PlacementValuesValid(Placement placement)
        {
            return placement != null && placement.WX != double.NaN && placement.WX > 0 && placement.WY != double.NaN && placement.WY > 0;
        }

        protected bool IsStrokeBottomToTop(Stroke stroke)
        {
            Stroke firstStrokeOfRally = stroke.Rally.Strokes[0];
            if (firstStrokeOfRally.Placement.WY.Equals(double.NaN))
            {
                Debug.WriteLine("first stroke of rally {0} has NaN Y-placement. assuming bottom-to-top direction.", stroke.Rally.Number);
                return true;
            }
            return stroke.Number % 2 == (firstStrokeOfRally.Placement.WY < 137 ? 1 : 0);
        }

        #endregion

        protected enum PointType { Start, Middle, End }
        protected enum ShapeType { Direction, Arrowtip, Intercept, SpinShape, Debug_preceding}
        protected enum ElementType { StrokeNumber }
    }
}
