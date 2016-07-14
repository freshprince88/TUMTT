using Caliburn.Micro;
using System;
using System.Collections.Generic;
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
    public partial class LargeTableView : TableView
    {
        public override Grid View_InnerFieldGrid { get { return InnerFieldGrid; } }
        public override Grid View_TableGrid { get { return TableGrid; } }
        public override Grid View_InnerFieldBehindGrid { get { return InnerFieldBehindGrid; } }
        public override Grid View_InnerFieldHalfDistanceGrid { get { return InnerFieldHalfDistanceGrid; } }
        public override Grid View_InnerFieldSpinGrid { get { return InnerFieldSpinGrid; } }
        public override Border View_TableBorder { get { return TableBorder; } }

        public LargeTableView()
        {
            InitializeComponent();            
        }

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

        protected override void AttachEventHandlerToShape(Shape shape, Stroke stroke)
        {
            shape.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
            shape.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
            shape.DataContext = stroke;
            Message.SetAttach(shape, "StrokeSelected($DataContext)");
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

        protected override double GetAdjustedX(Stroke stroke, double oldX)
        {
            Grid grid = GetGridForStroke(stroke);
            //Debug.WriteLine("{0} : aw={1} mr={2} ml={3}", stroke.Rally.Number, grid.ActualWidth, grid.Margin.Right, grid.Margin.Left);
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

        protected override double GetAdjustedY(Stroke stroke, double oldY)
        {
            Grid grid = GetGridForStroke(stroke);
            //Debug.WriteLine("{0} : ah={1} mb={2} mt={3}", stroke.Rally.Number, grid.ActualHeight, grid.Margin.Bottom, grid.Margin.Top);
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
        
        protected override double GetSecondStrokePrecedingStartY()
        {
            return 0;
        }

        protected override void ProcessStrokes(List<Stroke> strokes)
        {
            // clear all previous strokes
            foreach (UIElement p in TableGrid.Children)
            {
                if (p is Grid)
                    (p as Grid).Children.Clear();
            }
            StrokeShapes.Clear();

            // add new lists of shapes for each stroke and also the strokes themselves 
            // (at this point the actual size of this view should be set)
            foreach (Stroke s in strokes)
            {
                StrokeShapes[s] = new List<Shape>();

                if (!PlacementValuesValid(s.Placement))
                    continue;

                if (s.Number == 1)
                    AddServiceStrokesSpinArrows(s);
                AddStrokesDirectionLines(s);
                AddInterceptArrows(s);
                AddStrokesArrowtips(s);
            }
        }
    }
}
