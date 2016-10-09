using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using TT.Models;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaction logic for LargeTableView.xaml
    /// </summary>
    public partial class LargeTableView : TableView
    {

        private const double StrokeOpacity = 1.0;
        private const double StrokeOpacityHover = 0.7;
        private const double StrokeOpacityDisabled = 0.18;

        public override Grid View_InnerFieldGrid { get { return InnerFieldGrid; } }
        public override Grid View_InnerFieldBehindGrid { get { return InnerFieldBehindGrid; } }
        public override Grid View_InnerFieldHalfDistanceGrid { get { return InnerFieldHalfDistanceGrid; } }
        public override Grid View_InnerFieldSpinGrid { get { return InnerFieldSpinGrid; } }

        private Stroke SelectedStroke;

        public LargeTableView()
        {
            InitializeComponent();            
        }

        #region Event handlers

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
                        s.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)s.Tag, stroke.Stroketechnique, true);
                        if (stroke != SelectedStroke)
                        {
                            s.Opacity = StrokeOpacityHover;
                        }
                    }
                    else
                    {
                        s.Opacity = StrokeOpacityDisabled;
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
                        s.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)s.Tag, stroke.Stroketechnique, false);
                        if (stroke != SelectedStroke)
                        {
                            s.Opacity = StrokeOpacityDisabled;
                        }
                    }
                    else
                    {
                        s.Opacity = SelectedStroke != null ? StrokeOpacityDisabled : StrokeOpacity;
                    }
                }
            }
        }

        private void Stroke_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            Shape shape = sender as Shape;
            Stroke stroke = shape.DataContext as Stroke;

            //Debug.WriteLine("mouse down on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);

            SelectedStroke = stroke;

            foreach (Shape s in StrokeShapes[stroke])
            {
                s.Opacity = StrokeOpacity;
            }
        }

        protected override void AttachEventHandlerToShape(Shape shape, Stroke stroke)
        {
            shape.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
            shape.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
            shape.MouseDown += new MouseButtonEventHandler(Stroke_MouseDown);
            shape.DataContext = stroke;
            Message.SetAttach(shape, "StrokeSelected($DataContext)");
        }

        #endregion

        #region Helper methods
        
        protected override bool ShowDirectionForStroke(Stroke stroke)
        {
            return ShowDirection;
        }

        protected override bool ShowSpinForStroke(Stroke stroke)
        {
            return ShowSpin;
        }

        protected override bool ShowInterceptForStroke(Stroke stroke)
        {
            return ShowIntercept;
        }

        private double GetStrokeThicknessForStroke(ShapeType type, Stroketechnique technique, bool hover)
        {
            if (type == ShapeType.SpinShape)
                return hover ? StrokeThicknessSpinArrowHover : StrokeThicknessSpinArrow;
            else if (type == ShapeType.Intercept)
                return hover ? StrokeThicknessInterceptHover : StrokeThicknessIntercept;
            else if (type == ShapeType.Debug_preceding)
                return hover ? StrokeThicknessPrecedingHover_Debug : StrokeThicknessPreceding_Debug;
            else if (technique != null && technique.EnumType == Models.Util.Enums.Stroke.Technique.Smash)
                return hover ? StrokeThicknessSmashHover : StrokeThicknessSmash;
            else
                return hover ? StrokeThicknessHover : StrokeThickness;
        }

        protected override double GetAdjustedX(Stroke stroke, double oldX)
        {
            Grid grid = GetGridForStroke(stroke);
            //Debug.WriteLine("{0} : aw={1} mr={2} ml={3}", stroke.Rally.Number, grid.ActualWidth, grid.Margin.Right, grid.Margin.Left);
            if (IsStrokeBottomToTop(stroke))
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
            if (IsStrokeBottomToTop(stroke))
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

            // add new lists of shapes for each stroke and also the strokes themselves 
            // (at this point the actual size of this view should be set)
            foreach (Stroke s in strokes)
            {
                StrokeShapes[s] = new List<Shape>();

                if (!PlacementValuesValid(s.Placement))
                    continue;

                AddServiceStrokesSpinShapes(s);
                AddStrokesDirectionShapes(s);
                AddInterceptArrows(s);
                AddStrokesArrowtips(s);
            }
        }
    }
}
