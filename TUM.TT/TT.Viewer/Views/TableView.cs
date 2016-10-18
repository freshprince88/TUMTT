using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TT.Models;
using TT.Lib.Exceptions;

namespace TT.Viewer.Views
{
    //
    // Summary:
    //     Abstract class for schematic table display.
    public abstract class TableView : UserControl
    {
        #region Constants

        protected const double StrokeThickness = 1.75;
        protected const double StrokeThicknessSpinArrow = 1;
        protected const double StrokeThicknessSmash = 3.5;
        protected const double StrokeThicknessSmashIntercept = 2;
        protected const double StrokeThicknessPreceding_Debug = 0.5;
        protected const double StrokeThicknessPrecedingHover_Debug = 0.7;
        protected const double StrokeThicknessIntercept = 1.0;

        protected const double BananaCurveRation = 0.5;
        protected const double TopspinCurveRation = 0.8;
        protected const double ChopCurveRation = 0.95;
        protected const double LobCurveRation = 2d / 3d;

        protected readonly Brush StrokeBrushBanana = Brushes.Gold;
        protected readonly Brush StrokeBrushBananaDisabled = Brushes.Goldenrod;

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

        //public bool ShowDebug
        //{
        //    get { return (bool)GetValue(ShowDebugProperty); }
        //    set { SetValue(ShowDebugProperty, value); }
        //}

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

        //public static DependencyProperty ShowDebugProperty = DependencyProperty.Register(
        //    "ShowDebug", typeof(bool), typeof(TableView), new PropertyMetadata(true, new PropertyChangedCallback(OnDisplayTypePropertyChanged)));

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
                    {
                        view.HideShapesByTag(ShapeType.Direction);
                        view.DirectionShapesHidden();
                    }
                }
                else if (e.Property == ShowSpinProperty)
                {
                    if ((bool)e.NewValue)
                        foreach (Stroke s in view.StrokeShapes.Keys) view.AddServiceStrokesSpinShapes(s);
                    else
                        view.HideShapesByTag(ShapeType.SpinShape);
                }
                //else if (e.Property == ShowDebugProperty)
                //{
                //    // you'll have to reload the view to display debug lines again :(
                //    if (!(bool)e.NewValue)
                //        view.HideShapesByTag(ShapeType.Debug_preceding);
                //}
                else if (e.Property == ShowInterceptProperty)
                {
                    if ((bool)e.NewValue)
                        foreach (Stroke s in view.StrokeShapes.Keys) view.AddInterceptArrows(s);
                    else
                        view.HideShapesByTag(ShapeType.Intercept);
                }
            }
        }

        protected abstract void ProcessStrokes(List<Stroke> strokes);

        #endregion

        #region Shape addition & removal

        protected void AddStrokesDirectionShapes(Stroke stroke)
        {
            if (!ShowDirectionForStroke(stroke))
                return;

            // first check if we already created a direction shape for this stroke. if so, display it and we're done
            Shape shape = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Direction);
            if (shape != null)
            {
                shape.Visibility = Visibility.Visible;
                ApplyStyle(stroke, StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Arrowtip));
                return;
            }
            
            bool isServiceStroke = stroke.Number == 1;
            bool isNetOrOut = stroke.EnumCourse == Models.Util.Enums.Stroke.Course.NetOut;
            
            double X1, X2, Y1, Y2;
            X1 = Y1 = -1;
            try
            {
                GetStartPointOfStroke(stroke, out X1, out Y1);
                if (isNetOrOut)
                    GetEndPointOfStrokeNetOrOut(stroke, X1, Y1, out X2, out Y2);
                else
                    GetEndPointOfStroke(stroke, out X2, out Y2);
            } catch (Exception ex) when (ex is NoStrokeStartingPointException || ex is NoStrokeEndPointException)
            {
                Debug.WriteLine("Adding direction to stroke {0} of rally {1} not possible ({2}: {3})", stroke.Number, stroke.Rally.Number, ex.GetType().Name, ex.Message);
                return;
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

            // add the shape to our map so we can find it later
            StrokeShapes[stroke].Add(shape);

            // apply style (also depends mostly on stroke technique, but also side - forehand/backhand - and other)
            ApplyStyle(stroke, shape);

            // now add it to the respective grid
            GetGridForStroke(stroke).Children.Add(shape);

            // hide it immediately, if the "Direction" checkbox is unchecked
            if (!ShowDirection)
                shape.Visibility = Visibility.Hidden;
        }

        protected void AddInterceptArrows(Stroke stroke)
        {
            // if "Intercept" checkbox isn't checked (or some other reason), do nothing
            if (!ShowInterceptForStroke(stroke))
                return;

            // first look for intercept shapes already in our map, if there are some, display them and return
            List<Shape> interceptShapes = StrokeShapes[stroke].FindAll(s => (ShapeType)s.Tag == ShapeType.Intercept);
            if (interceptShapes != null && interceptShapes.Count > 0)
            {
                foreach (Shape interceptShape in interceptShapes)
                    interceptShape.Visibility = Visibility.Visible;
                return;
            }

            // no intercept shapes for stroke that went net/out or are winner strokes (have no successors)
            if (stroke.EnumCourse == Models.Util.Enums.Stroke.Course.NetOut || stroke.Number >= stroke.Rally.Strokes.Count)
                return;

            Stroke followingStroke = stroke.Rally.Strokes[stroke.Number];
            Grid followingStrokeGrid = GetGridForStroke(followingStroke);

            double X1, Y1, X2, Y2;

            // if there is already a direction shape painted, get the start and end point from it
            Shape directionShape = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Direction);
            if (directionShape != null)
            {
                GetPointOfShapeRelativeToGrid(directionShape, PointType.Start, followingStrokeGrid, out X1, out Y1);
                GetPointOfShapeRelativeToGrid(directionShape, PointType.End, followingStrokeGrid, out X2, out Y2);
            }
            else
            {
                // else try getting start and end point of this stroke 'manually'
                try
                {
                    GetStartPointOfStroke(stroke, out X1, out Y1);
                    GetEndPointOfStroke(stroke, out X2, out Y2);

                    // same adjustment as in GetPointOfShapeRelativeToGrid - prevents some intercept shapes starting from wrong y-values
                    Grid strokeGrid = GetGridForStroke(stroke);
                    if (strokeGrid != null && strokeGrid.Name != followingStrokeGrid.Name)
                        Y2 -= followingStrokeGrid.Margin.Top - strokeGrid.Margin.Top;
                } catch (Exception ex) when (ex is NoStrokeStartingPointException || ex is NoStrokeEndPointException)
                {
                    Debug.WriteLine("Adding intercept arrow to stroke {0} of rally {1} not possible ({2}: {3})", stroke.Number, stroke.Rally.Number, ex.GetType().Name, ex.Message);
                    return;
                }
            }
            
            double xE, yE;

            // account for possibly missing information on PointOfContact on the following stroke
            Models.Util.Enums.Stroke.PointOfContact realPointOfContact;
            if (followingStroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.None)
            {
                if (stroke.IsShort() || stroke.IsHalfLong())
                    realPointOfContact = Models.Util.Enums.Stroke.PointOfContact.Over;
                else
                    realPointOfContact = Models.Util.Enums.Stroke.PointOfContact.Behind;
            }
            else
            {
                realPointOfContact = followingStroke.EnumPointOfContact;
            }

            // 'extrapolate' y-coordinate (usually 0 or height of grid or some arbitrary point on the table if the contact point was 'Over')
            if (Y1 > Y2)
                yE = realPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? Y2 - 30 : 0;
            else if (Y1 < Y2)
                yE = realPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? Y2 + 30 : followingStrokeGrid.ActualHeight;
            else
                yE = Y2;    // special case: horizontal stroke (e.g. legend view)

            if (Y1 != Y2)
            {
                // extrapolate x-coordinate (linear continuation)
                xE = GetLinearContinuationX(X1, Y1, X2, Y2, yE);
                if (xE.Equals(double.NaN))
                {
                    Debug.WriteLine("Adding intercept arrow to stroke {0} of rally {1} failed (NaN extrapolated value (xE={2})", stroke.Number, stroke.Rally.Number, xE);
                    return;
                }
            }
            else
                xE = X2 > X1 ? followingStrokeGrid.ActualWidth : 0; // special case: horizontal stroke (e.g. legend view)

            // line shape part of intercept arrow
            Line interceptLine = new Line();
            interceptLine.X1 = X2;
            interceptLine.Y1 = Y2;
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

            // arrow tip part of intercept arrow
            PathGeometry arrowTipGeometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure();
            pathFigure.IsClosed = true;
            pathFigure.StartPoint = new Point(xE - 1.5, yE - 3.5);

            LineSegment ltt = new LineSegment(new Point(xE, yE), true);
            pathFigure.Segments.Add(ltt);

            LineSegment ttr = new LineSegment(new Point(xE + 1.5, yE - 3.5), true);
            pathFigure.Segments.Add(ttr);

            double theta = Math.Atan2((yE - Y2), (xE - X2)) * 180 / Math.PI;
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
            {
                strokeArrowTip.Visibility = Visibility.Visible;
                return;
            }

            Grid gridOfStroke = GetGridForStroke(stroke);
            double X1, X2, Y1, Y2;

            try
            {
                Shape shape = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Direction);
                if (stroke.Number == 1 || shape == null)
                    GetStartPointOfStroke(stroke, out X1, out Y1);
                else
                    GetPointOfShapeRelativeToGrid(shape, PointType.Middle, gridOfStroke, out X1, out Y1);
            } catch (Exception ex) when (ex is NoStrokeStartingPointException || ex is NoStrokeEndPointException)
            {
                Debug.WriteLine("Adding arrowtips to stroke {0} of rally {1} inaccurate ({2}: {3})", stroke.Number, stroke.Rally.Number, ex.GetType().Name, ex.Message);
                X1 = Y1 = -1;
            }

            bool isNetOrOut = stroke.EnumCourse == Models.Util.Enums.Stroke.Course.NetOut;
            try
            {
                if (isNetOrOut)
                    GetEndPointOfStrokeNetOrOut(stroke, X1, Y1, out X2, out Y2);
                else
                    GetEndPointOfStroke(stroke, out X2, out Y2);
            } catch (NoStrokeEndPointException ex)
            {
                Debug.WriteLine("Adding arrowtips to stroke {0} of rally {1} not possible ({2}: {3})", stroke.Number, stroke.Rally.Number, ex.GetType().Name, ex.Message);
                return;
            }

            PathGeometry arrowTipGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();

            if (isNetOrOut)
            {
                // rotated cross shape

                pathFigure.StartPoint = new Point(X2, Y2 - 6);

                LineSegment ttb = new LineSegment(new Point(X2, Y2 + 6), true);
                pathFigure.Segments.Add(ttb);

                arrowTipGeometry.Figures.Add(pathFigure);

                pathFigure = new PathFigure();
                pathFigure.StartPoint = new Point(X2 - 6, Y2);

                LineSegment ltr = new LineSegment(new Point(X2 + 6, Y2), true);
                pathFigure.Segments.Add(ltr);

                arrowTipGeometry.Figures.Add(pathFigure);

                arrowTipGeometry.Transform = new RotateTransform(45, X2, Y2);
            }
            else
            {
                // triangle shape

                pathFigure.StartPoint = new Point(X2 - 3, Y2 - 3);
                pathFigure.IsClosed = true;

                LineSegment ltt = new LineSegment(new Point(X2, Y2), true);
                pathFigure.Segments.Add(ltt);

                LineSegment ttr = new LineSegment(new Point(X2 + 3, Y2 - 3), true);
                pathFigure.Segments.Add(ttr);

                if (X1 != -1 && Y1 != -1)
                {
                    double theta = Math.Atan2((Y2 - Y1), (X2 - X1)) * 180 / Math.PI;
                    arrowTipGeometry.Transform = new RotateTransform(theta - 90, X2, Y2);
                }
                else
                    arrowTipGeometry.Transform = new RotateTransform(Y2 > 137 ? 0 : 180, X2, Y2);
            }

            arrowTipGeometry.Figures.Add(pathFigure);

            strokeArrowTip = new Path();
            ((Path)strokeArrowTip).Data = arrowTipGeometry;
            strokeArrowTip.Tag = ShapeType.Arrowtip;
            strokeArrowTip.StrokeThickness = StrokeThickness;

            // ScaleTranform as RenderTransform for changing size of arrow tips later (with correct center point)
            strokeArrowTip.RenderTransform = new ScaleTransform(1, 1, X2, Y2);

            ApplyStyle(stroke, strokeArrowTip);

            AttachEventHandlerToShape(strokeArrowTip, stroke);

            StrokeShapes[stroke].Add(strokeArrowTip);

            gridOfStroke.Children.Add(strokeArrowTip);

            if (!ShowStroke(stroke))
                strokeArrowTip.Visibility = Visibility.Hidden;
        }

        protected void AddServiceStrokesSpinShapes(Stroke stroke)
        {
            if (stroke.Number != 1 || !ShowSpinForStroke(stroke))
                return;

            Shape spinArrow = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.SpinShape);
            if (spinArrow != null)
            {
                spinArrow.Visibility = Visibility.Visible;
                return;
            }

            double X1, Y1;
            try
            {
                GetStartPointOfStroke(stroke, out X1, out Y1);
                Y1 = View_InnerFieldSpinGrid.ActualHeight - 1;
            }
            catch (NoStrokeStartingPointException ex)
            {
                Debug.WriteLine("Adding spin arrow to service stroke of rally {1} not possible ({2}: {3})", stroke.Number, stroke.Rally.Number, ex.GetType().Name, ex.Message);
                return;
            }

            if (stroke.Playerposition == double.MinValue)   // service stroke in legend
                X1 = 0;

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

        protected virtual bool ShowStroke(Stroke stroke)
        {
            return true;
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

        protected virtual void DirectionShapesHidden()
        {
            // no standard handling of hiding direction shape. override if necessary.
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

            //Debug.WriteLine("lob {7} of rally {6}: x1={0} y1={1} -> x2={2} y2={3} (cp: x={4} y={5})", X1, Y1, X2, Y2, cpx, cpy, stroke.Rally.Number, stroke.Number);

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

        protected virtual void ApplyStyle(Stroke stroke, Shape shape)
        {
            // no work to do if shape is null :O
            if (shape == null)
                return;

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
                        shape.Stroke = StrokeBrushBanana;
                        if (shape is Path && (ShapeType)shape.Tag != ShapeType.Direction)
                            shape.Fill = StrokeBrushBanana;
                        break;
                    case Models.Util.Enums.Stroke.Technique.Smash:
                        shape.Stroke = Brushes.Blue;
                        shape.Fill = Brushes.Blue;
                        shape.StrokeThickness = (ShapeType)shape.Tag == ShapeType.Intercept ? StrokeThicknessSmashIntercept : StrokeThicknessSmash;
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
        protected abstract bool ShowDirectionForStroke(Stroke stroke);
        protected abstract bool ShowSpinForStroke(Stroke stroke);
        protected abstract bool ShowInterceptForStroke(Stroke stroke);

        private void GetStartPointOfStroke(Stroke stroke, out double x, out double y)
        {
            double tempX, tempY;

            bool isServiceStroke = stroke.Number == 1;
            bool isNetOrOut = stroke.EnumCourse == Models.Util.Enums.Stroke.Course.NetOut;

            // service strokes get special treatment because they
            // a. generally start from the bottom in most views (small/large)
            // b. don't have any preceding strokes to base their starting point on
            if (isServiceStroke)
            {
                if (stroke.Playerposition.Equals(double.NaN))
                    throw new NoStrokeStartingPointException("Stroke is service and no '" + nameof(stroke.Playerposition) + "' was given");

                // double.MinValue as Playerposition indicates a service stroke in the Legend view
                // since there the arrows are painted horizontally, we have to set special values here
                if (stroke.Playerposition == double.MinValue)
                {
                    tempX = 0;
                    tempY = stroke.Placement.WY;
                }
                // other than that, service strokes start from the bottom (height of the grid they will be added to)
                // and their position is determined by Playerposition
                else
                {
                    tempX = GetAdjustedX(stroke, stroke.Playerposition);
                    tempY = View_InnerFieldBehindGrid.ActualHeight;
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
                    precedingStartX = precedingStroke.Playerposition.Equals(double.NaN) ? double.MinValue : GetAdjustedX(stroke, precedingStroke.Playerposition);
                    precedingStartY = GetSecondStrokePrecedingStartY(); // GetAdjustedY doesn't help here
                }
                else
                {
                    // the preceding stroke was not a service stroke, i.e. it has at least one other preceding stroke
                    // whose placement (WX, WY) we can take for an approximation of this strokes starting point
                    Placement placementSecondPreceding = stroke.Rally.Strokes[stroke.Number - 3].Placement;
                    if (PlacementValuesValid(placementSecondPreceding))
                    {
                        precedingStartX = GetAdjustedX(stroke, placementSecondPreceding.WX);
                        precedingStartY = GetAdjustedY(stroke, placementSecondPreceding.WY);
                    }
                    else
                    {
                        // no valid values of the second preceding stroke given - we won't be able to extrapolate beginning to end
                        precedingStartX = double.MinValue;
                        precedingStartY = double.MinValue;
                    }   
                }

                if (PlacementValuesValid(precedingStroke.Placement))
                {
                    // the end point for this stroke's starting point approximation is always the preceding stroke's placement
                    precedingEndX = GetAdjustedX(stroke, precedingStroke.Placement.WX);
                    precedingEndY = GetAdjustedY(stroke, precedingStroke.Placement.WY);
                }
                else
                    // no valid values of the immediate preceding stroke - we don't know where this stroke should start from!
                    throw new NoStrokeStartingPointException("no endpoint of preceding stroke - unable to set start of this stroke");

                //if (ShowDebug)
                //    AddDebugLine(stroke, precedingStartX, precedingStartY, precedingEndX, precedingEndY, false);

                if (precedingStartX != double.MinValue && precedingStartY != double.MinValue)
                {
                    if (precedingStartY == precedingEndY)
                        tempY = precedingEndY; // means the preceding stroke didn't change the Y coordinate -> reserved for special cases (e.g. horizontal strokes in Legend)
                    else
                    // Depending on whether we draw the stroke from top to bottom or vice versa, the stroke will start at Y '0' (top)
                    // or the height of the grid the stroke is going to be added to (bottom).
                    // in the case that its point of contact is OVER the table, we need the height of the previous stroke (we add/subtract arbitrarilly '30' for realism)
                    // At this point, precedingEndY has a sensible value, else an exception would have been thrown earlier.
                    if (precedingStartY > precedingEndY)    // preceding was bottom to top (relative to this stroke's direction). IsStrokeBottomToTop doesn't help here!
                        tempY = stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? precedingEndY - 30 : 0;
                    else
                        tempY = stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? precedingEndY + 30 : GetGridForStroke(stroke).ActualHeight;

                    // now we have the starting and end point (both X and Y) of the previous stroke and the height at which this stroke should end
                    // -> extrapolate its X-coordinate
                    if (precedingStartY != precedingEndY)
                        tempX = GetLinearContinuationX(precedingStartX, precedingStartY, precedingEndX, precedingEndY, tempY);
                    else
                        tempX = precedingEndX > precedingStartX ? GetGridForStroke(stroke).ActualWidth : 0;    // equal preceding stroke's Y coordinate -> horizontal

                    //if (ShowDebug)
                    //    AddDebugLine(stroke, precedingEndX, precedingEndY, X1, Y1, true);
                }
                else
                {
                    // we don't know where the preceding stroke originated so we base this stroke's start point on the
                    // preceding stroke's exact end point (Placement). preceding stroke's Placement is valid or else
                    // an exception would have gotten thrown earlier
                    Debug.WriteLine("Starting point of stroke {0} of rally {1} inaccurate (Start point of preceding stroke undefined - extrapolation impossible). " +
                        "Settings starting point to placement of preceding stroke.", stroke.Number, stroke.Rally.Number);
                    tempX = precedingEndX;
                    tempY = precedingEndY;
                }
            }
            x = tempX;
            y = tempY;
        }

        private void GetEndPointOfStroke(Stroke stroke, out double x, out double y)
        {
            if (PlacementValuesValid(stroke.Placement))
            {
                // this stroke's end is always the exact position of its placement
                x = GetAdjustedX(stroke, stroke.Placement.WX);
                y = GetAdjustedY(stroke, stroke.Placement.WY);                    
            }
            else
                throw new NoStrokeEndPointException("invalid Placement of stroke " + stroke.Number + " of rally " + stroke.Rally.Number + ": x=" + (stroke.Placement != null ? stroke.Placement.WX.ToString() : "[n/a]") + " y=" + (stroke.Placement != null ? stroke.Placement.WY.ToString() : "[n/a]"));
        }

        private void GetEndPointOfStrokeNetOrOut(Stroke stroke, double startPointX, double startPointY, out double x, out double y)
        {
            if (startPointX >= 0 && startPointY >= 0)
            {
                x = startPointX;
                if (startPointY > 137)
                    y = stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? startPointY - 40 : GetGridForStroke(stroke).ActualHeight - 40;
                else
                    y = stroke.EnumPointOfContact == Models.Util.Enums.Stroke.PointOfContact.Over ? startPointY + 40 : 40;
            }
            else
                throw new NoStrokeEndPointException("stroke " + stroke.Number + " of rally " + stroke.Rally.Number + " was net/out but no sensible starting point was given (x=" + startPointX + " y=" + startPointY + ")");
        }

        /// <summary>
        /// Not 100% sure why, but we need this method - small and large table need exactly opposite values,
        /// but only for second stroke.
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
            {
                if (stroke.IsShort() || stroke.IsHalfLong())
                    return View_InnerFieldGrid;
                else
                    return View_InnerFieldBehindGrid;
            }
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

        protected void GetPointOfShapeRelativeToGrid(Shape shape, PointType which, Grid grid, out double x, out double y)
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
            return placement != null && placement.WX != double.NaN && placement.WX >= 0 && placement.WY != double.NaN && placement.WY >= 0;
        }

        protected bool IsStrokeBottomToTop(Stroke stroke)
        {
            for (int i = 0; i < stroke.Rally.Strokes.Count; i++)
            {
                Stroke iThStroke = stroke.Rally.Strokes[i];
                if (!PlacementValuesValid(iThStroke.Placement))
                    continue;

                switch (i % 2)
                {
                    default:
                    case 0: return stroke.Number % 2 == (iThStroke.Placement.WY < 137 ? 1 : 0);
                    case 1: return stroke.Number % 2 == (iThStroke.Placement.WY < 137 ? 0 : 1);
                }
            }

            string direction = stroke.Number % 2 == 1 ? "bottom-to-top" : "top-to-bottom";
            Debug.WriteLine("No strokes with valid placement found in rally {0}. Assuming {1} direction of stroke {2}.", stroke.Rally.Number, direction, stroke.Number);
            return stroke.Number % 2 == 1;
        }

        #endregion

        protected enum PointType { Start, Middle, End }
        protected enum ShapeType { Direction, Arrowtip, Intercept, SpinShape, Debug_preceding, NetOut }
        protected enum StrokeInteraction { Normal, Hover, HoverOther, Selected, None }
        protected enum ElementType { StrokeNumber }
    }
}
