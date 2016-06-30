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

        private Dictionary<Stroke, List<Shape>> strokeShapes;

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

        }

        private void CheckDirection_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value)
                AddServiceStrokesDirection(new List<Stroke>(strokeShapes.Keys));
            else
                RemoveServiceStrokesDirections();
        }

        private void Stroke_MouseEnter(Object sender, MouseEventArgs e)
        {
            Console.Out.WriteLine("mouse enter on stroke of rally {0}", ((sender as Shape).DataContext as Stroke).Rally.Number);
            Shape shape = sender as Shape;
            List<Shape> shapes = strokeShapes[(shape.DataContext as Stroke)];
            foreach (var s in shapes)
            {
                s.StrokeThickness = STROKE_THICKNESS_HOVER;
            }
        }
        
        private void Stroke_MouseLeave(Object sender, MouseEventArgs e)
        {
            Console.Out.WriteLine("mouse leave on stroke of rally {0}", ((sender as Shape).DataContext as Stroke).Rally.Number);
            Shape shape = sender as Shape;
            List<Shape> shapes = strokeShapes[(shape.DataContext as Stroke)];
            foreach (var s in shapes)
            {
                s.StrokeThickness = STROKE_THICKNESS;
            }
        }

        public void Handle(StrokesPaintEvent message)
        {
            InnerFieldGrid.Children.Clear();
            strokeShapes.Clear();

            if (message.Strokes == null)
                return;

            message.Strokes.ForEach(s => { strokeShapes[s] = new List<Shape>(); });

            halfFieldWidth = InnerFieldGrid.ActualWidth / 2;
            halfFieldHeight = InnerFieldGrid.ActualHeight / 2;
            tableMarginFieldLeft = TableBorder.Margin.Left - InnerFieldGrid.Margin.Left;
            tableMarginFieldBottom = TableBorder.Margin.Bottom - InnerFieldGrid.Margin.Bottom;
            Console.Out.WriteLine("hfw: {0}, hfh: {1}", halfFieldWidth, halfFieldHeight);

            switch (message.StrokeNumber)
            {
                default:
                case 1:                    
                    AddServiceStrokesPoint(message.Strokes);
                    if (CheckDirection.IsChecked.Value)
                        AddServiceStrokesDirection(message.Strokes);
                    if (CheckSpin.IsChecked.Value)
                        AddServiceStrokesSpin(message.Strokes);
                    break;
            }
        }

        private void AddServiceStrokesSpin(List<Stroke> strokes)
        {
            foreach (var stroke in strokes)
            {
                if (PlacementValuesValid(stroke.Placement))
                {
                    SpinSmallView spinView = new SpinSmallView(stroke.Spin);
                    spinView.Width = spinView.Height = 15;
                    spinView.Margin = new Thickness(stroke.Playerposition.Equals(double.NaN) ? 0 : stroke.Playerposition, InnerFieldGrid.ActualHeight - 15, 0, 0);
                    
                    InnerFieldGrid.Children.Add(spinView);
                }
            }
        }

        private void AddServiceStrokesDirection(List<Stroke> strokes)
        {
            Console.Out.WriteLine("Strokes to paint: {0}", strokes.Count);
            foreach (var stroke in strokes)
            {
                if (PlacementValuesValid(stroke.Placement))
                {
                    Line strokeLine = new Line();

                    // always start lines at the bottom
                    strokeLine.Y1 = InnerFieldGrid.ActualHeight;

                    if (stroke.Placement.WY < 137)
                    {
                        // first stroke in the upper half of table: server was at camera bottom
                        strokeLine.X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : stroke.Playerposition + tableMarginFieldLeft;

                        strokeLine.X2 = stroke.Placement.WX + tableMarginFieldLeft;
                        strokeLine.Y2 = stroke.Placement.WY + tableMarginFieldBottom;
                    }
                    else
                    {
                        // first stroke in the lower half of table: server was at camera top
                        // => flip starting x and end x
                        strokeLine.X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : halfFieldWidth - ((stroke.Playerposition + tableMarginFieldLeft) - halfFieldWidth);

                        strokeLine.X2 = halfFieldWidth - ((stroke.Placement.WX + tableMarginFieldLeft) - halfFieldWidth);
                        strokeLine.Y2 = InnerFieldGrid.ActualHeight - tableMarginFieldBottom - stroke.Placement.WY;

                    }

                    Console.Out.WriteLine("stroke rally={4} x1={0} y1={1} x2={2} y2={3}", strokeLine.X1, strokeLine.Y1, strokeLine.X2, strokeLine.Y2, stroke.Rally.Number);

                    strokeLine.StrokeThickness = STROKE_THICKNESS;
                    strokeLine.Stroke = Brushes.Black;
                    strokeLine.Fill = Brushes.Black;

                    //DoubleCollection dashes = new DoubleCollection();
                    //dashes.Add(2);
                    //strokeLine.StrokeDashArray = dashes;

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
                    Console.Out.WriteLine("invalid Placement of stroke {0}: x={1} y={2}", stroke, stroke.Placement.WX, stroke.Placement.WY);
                }
            }
        }

        private void AddServiceStrokesPoint(List<Stroke> strokes)
        {
            foreach (var stroke in strokes)
            {
                if (PlacementValuesValid(stroke.Placement))
                {
                    double X1, X2, Y1, Y2;

                    // always start lines at the bottom
                    Y1 = InnerFieldGrid.ActualHeight;

                    if (stroke.Placement.WY < 137)
                    {
                        // first stroke in the upper half of table: server was at camera bottom
                        X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : stroke.Playerposition + tableMarginFieldLeft;

                        X2 = stroke.Placement.WX + tableMarginFieldLeft;
                        Y2 = stroke.Placement.WY + tableMarginFieldBottom;
                    }
                    else
                    {
                        // first stroke in the lower half of table: server was at camera top
                        // => flip starting x and end x
                        X1 = stroke.Playerposition.Equals(double.NaN) ? 0 : halfFieldWidth - ((stroke.Playerposition + tableMarginFieldLeft) - halfFieldWidth);

                        X2 = halfFieldWidth - ((stroke.Placement.WX + tableMarginFieldLeft) - halfFieldWidth);
                        Y2 = InnerFieldGrid.ActualHeight - tableMarginFieldBottom - stroke.Placement.WY;

                    }
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
                    strokeArrowTip.Stroke = Brushes.Black;
                    strokeArrowTip.Fill = Brushes.Black;
                    strokeArrowTip.StrokeThickness = STROKE_THICKNESS;

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
                    Console.Out.WriteLine("invalid Placement of stroke {0}: x={1} y={2}", stroke, stroke.Placement.WX, stroke.Placement.WY);
                }
            }
        }

        private void RemoveServiceStrokesDirections()
        {
            foreach (var s in strokeShapes.Values)
            {
                for (int i = 0; i < s.Count; i++)
                {
                    if (s[i] is Line)
                    {
                        InnerFieldGrid.Children.Remove(s[i]);
                        s.RemoveAt(i);
                        break;
                    }
                }                
            }
        }

        private bool PlacementValuesValid(Placement placement)
        {
            return placement.WX != double.NaN && placement.WX > 0 && placement.WY != double.NaN && placement.WY > 0;
        }
    }
}
