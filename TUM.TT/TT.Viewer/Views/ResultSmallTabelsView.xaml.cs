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
using System.Windows.Data;
using System.Globalization;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ResultSmallTablesView.xaml
    /// </summary>
    public partial class ResultSmallTablesView : UserControl,
        IHandle<StrokesPaintEvent>
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
        private int strokeNumber;

        public IEventAggregator Events { get; private set; }

        public ResultSmallTablesView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);

            strokeShapes = new Dictionary<Stroke, List<Shape>>();
        }

        #region Event handlers

        private void CheckSpin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CheckDirection_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SmallTable_MouseEnter(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("MouseEnter {0} {1}", sender, e);
            ((Border) ((SmallTableView) sender).Parent).BorderThickness = new Thickness(2);
        }

        private void SmallTable_MouseLeave(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("MouseLeave {0} {1}", sender, e);
            ((Border) ((SmallTableView)sender).Parent).BorderThickness = new Thickness(1);
        }

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
                        if ((string) s.Tag == TAG_SPIN_ARROW)
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
                        if ((string) s.Tag == TAG_SPIN_ARROW)
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

        public void Handle(StrokesPaintEvent message)
        {
            if (message.Strokes == null)
                return;

            strokeNumber = message.StrokeNumber;
           
        }

        #endregion

        #region Shape addition & removal






        private void RemoveShapesByTag(string tag)
        {
            foreach (var s in strokeShapes.Values)
            {
                for (int i = 0; i < s.Count; i++)
                {
                    if ((string)s[i].Tag == tag)
                    {
                        (s[i].Parent as Grid).Children.Remove(s[i]);
                        s.RemoveAt(i);
                        --i;
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

        #endregion

        #region Style

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

        #endregion

        #region Helper Methods
        
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

        private bool PlacementValuesValid(Placement placement)
        {
            return placement.WX != double.NaN && placement.WX > 0 && placement.WY != double.NaN && placement.WY > 0;
        }

        #endregion

    }
}
