using Caliburn.Micro;
using MahApps.Metro;
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
    /// <summary>
    /// Interaction logic for SmallTableView.xaml
    /// </summary>
    public partial class SmallTableView : TableView
    {
        private const int MAX_DISPLAYED_STROKES = 4;

        private static BrushConverter brushConverter = new BrushConverter();

        private AutoResetEvent sizeChangedWaitEvent;

        public override Grid View_InnerFieldGrid { get { return InnerFieldGrid; } }
        public override Grid View_TableGrid { get { return TableGrid; } }
        public override Grid View_InnerFieldBehindGrid { get { return InnerFieldBehindGrid; } }
        public override Grid View_InnerFieldHalfDistanceGrid { get { return InnerFieldHalfDistanceGrid; } }
        public override Grid View_InnerFieldSpinGrid { get { return InnerFieldSpinGrid; } }
        public override Border View_TableBorder { get { return TableBorder; } }

        public SmallTableView()
        {            
            InitializeComponent();

            sizeChangedWaitEvent = new AutoResetEvent(false);
            SizeChangedEventHandler sizeChangedEventHandler = delegate (object sender, SizeChangedEventArgs e)
            {
                //Debug.WriteLine("size changed! {0} {1}", ActualWidth, ActualHeight);
                sizeChangedWaitEvent.Set();
            };

            SizeChanged += sizeChangedEventHandler;
        }

        #region Event handlers

        private void MySizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine(e);
        }

        private void Table_MouseEnter(object sender, MouseEventArgs e)
        {
            SmallTableViewBorder.BorderThickness = new Thickness(2);
            TableGrid.Background = (Brush) brushConverter.ConvertFrom("#e2e7e0");
        }

        private void Table_MouseLeave(object sender, MouseEventArgs e)
        {
            SmallTableViewBorder.BorderThickness = new Thickness(1);
            TableGrid.Background = Brushes.Transparent;
        }

        #endregion
        protected override void ProcessStrokes(List<Stroke> strokes)
        {
            // clear all previous strokes
            foreach (UIElement p in TableGrid.Children)
            {
                if (p is Grid)
                    (p as Grid).Children.Clear();
            }
            StrokeShapes.Clear();

            // add new lists of shapes for each stroke - up to MAX_DISPLAYED_STROKES many for small table!
            int maxStrokeDisplayCounter = 0;
            foreach (Stroke s in strokes)
            {
                maxStrokeDisplayCounter++;
                if (maxStrokeDisplayCounter > MAX_DISPLAYED_STROKES)
                    break;
                StrokeShapes[s] = new List<Shape>();
            }

            // once size of this view changed, we know it's been fully added: add the actual stroke shapes (they need the actual size)
            new Thread(new ThreadStart(() =>
            {
                sizeChangedWaitEvent.WaitOne();

                Dispatcher.Invoke(() =>
                {                    
                    foreach (Stroke stroke in StrokeShapes.Keys)
                    {
                        if (!PlacementValuesValid(stroke.Placement))
                            continue;                        

                        if (stroke.Number == 1)
                            AddServiceStrokesSpinArrows(stroke);
                        AddStrokesDirectionLines(stroke);
                        AddInterceptArrows(stroke);
                        AddStrokesArrowtips(stroke);
                    }
                });
            })).Start();
        }

        #region Helper methods

        protected override double GetAdjustedX(Stroke stroke, double oldX)
        {
            Grid grid = GetGridForStroke(stroke);
            //Debug.WriteLine("{0} : aw={1} mr={2} ml={3}", stroke.Rally.Number, grid.ActualWidth, grid.Margin.Right, grid.Margin.Left);
            if (stroke.Placement.WY < 137)
            {
                // stroke in the upper half of table
                return stroke.Number % 2 == 1 ? oldX + (TableBorder.Margin.Left - grid.Margin.Left) : grid.ActualWidth - oldX - (TableBorder.Margin.Left - grid.Margin.Left);
            }
            else
            {
                // stroke in the lower half of table => flip x
                return stroke.Number % 2 == 1 ? grid.ActualWidth - oldX - (TableBorder.Margin.Left - grid.Margin.Left) : oldX + (TableBorder.Margin.Left - grid.Margin.Left);
            }
        }

        protected override double GetAdjustedY(Stroke stroke, double oldY)
        {            
            Grid grid = GetGridForStroke(stroke);
            //Debug.WriteLine("{0} : ah={1} mb={2} mt={3}", stroke.Rally.Number, grid.ActualHeight, grid.Margin.Bottom, grid.Margin.Top);
            if (stroke.Placement.WY < 137)
            {
                // stroke in the upper half of table
                return stroke.Number % 2 == 1 ? oldY + (TableBorder.Margin.Top - grid.Margin.Top) : grid.ActualHeight - oldY - (TableBorder.Margin.Bottom - grid.Margin.Bottom);
            }
            else
            {
                // stroke in the lower half of table => flip y
                return stroke.Number % 2 == 1 ? grid.ActualHeight - oldY - (TableBorder.Margin.Bottom - grid.Margin.Bottom) : oldY + (TableBorder.Margin.Top - grid.Margin.Top);
            }
        }

        protected override double GetSecondStrokePrecedingStartY()
        {
            return InnerFieldBehindGrid.ActualHeight;
        }

        protected override void AttachEventHandlerToShape(Shape shape, Stroke stroke)
        {
            // no event handlers on strokes in small table view
            shape.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
            shape.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
            shape.DataContext = stroke;
            Message.SetAttach(shape, "StrokeSelected($DataContext)");
        }

        #endregion

        #region DEBUG
        private void Stroke_MouseEnter(Object sender, MouseEventArgs e)
        {
            Shape shape = sender as Shape;
            Stroke stroke = shape.DataContext as Stroke;

            //Debug.WriteLine("mouse enter on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);

            foreach (var strokeShape in StrokeShapes)
            {
                foreach (var s in strokeShape.Value)
                {
                    if (strokeShape.Key.Equals(stroke))
                    {
                        s.StrokeThickness = GetStrokeThicknessForStroke((string)s.Tag, stroke.Stroketechnique, true);
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

            foreach (var strokeShape in StrokeShapes)
            {
                foreach (var s in strokeShape.Value)
                {
                    if (strokeShape.Key.Equals(stroke))
                    {
                        s.StrokeThickness = GetStrokeThicknessForStroke((string)s.Tag, stroke.Stroketechnique, false);
                    }
                    else
                    {
                        s.Opacity = 1;
                    }
                }
            }
        }

        private double GetStrokeThicknessForStroke(string tag, Stroketechnique technique, bool hover)
        {
            if (tag == TAG_SPIN_ARROW)
                return hover ? STROKE_THICKNESS_SPIN_ARROW_HOVER : STROKE_THICKNESS_SPIN_ARROW;
            else if (tag == TAG_INTERCEPT)
                return hover ? STROKE_THICKNESS_INTERCEPT_HOVER : STROKE_THICKNESS_INTERCEPT;
            else if (tag == TAG_DEBUG_PRECEDING)
                return hover ? STROKE_THICKNESS_DEBUG_PRECEDING_HOVER : STROKE_THICKNESS_DEBUG_PRECEDING;
            else if (technique != null && technique.Type == STROKE_ATTR_TECHNIQUE_SMASH)
                return hover ? STROKE_THICKNESS_SMASH_HOVER : STROKE_THICKNESS_SMASH;
            else
                return hover ? STROKE_THICKNESS_HOVER : STROKE_THICKNESS;
        }

        #endregion
    }
}
