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
    /// Interaction logic for LargeTableView.xaml
    /// </summary>
    public partial class LargeTableView : TableView
    {

        protected const double StrokeThicknessHover = 2.5;
        protected const double StrokeThicknessSelected = 3;
        protected const double StrokeThicknessInterceptHover = 1.7;
        protected const double StrokeThicknessInterceptSelected = 2.0;
        protected const double StrokeThicknessSpinArrowHover = 2;
        protected const double StrokeThicknessSpinArrowSelected = 2.5;
        protected const double StrokeThicknessSmashHover = 5;
        protected const double StrokeThicknessSmashSelected = 6;
        protected const double StrokeThicknessSmashInterceptHover = 2.3;
        protected const double StrokeThicknessSmashInterceptSelected = 2.5;
        protected const double StrokeThicknessPrecedingSelected_Debug = 1;

        private const double StrokeOpacity = 1.0;
        private const double StrokeOpacityHover = 0.7;
        private const double StrokeOpacityHoverUnselected = 0.25;
        private const double StrokeOpacityHoverUnselectedSpin = 0.08;
        private const double StrokeOpacityDisabled = 0.1;
        private const double StrokeOpacityDisabledSpin = 0.04;

        public override Grid View_InnerFieldGrid { get { return InnerFieldGrid; } }
        public override Grid View_InnerFieldBehindGrid { get { return InnerFieldBehindGrid; } }
        public override Grid View_InnerFieldHalfDistanceGrid { get { return InnerFieldHalfDistanceGrid; } }
        public override Grid View_InnerFieldSpinGrid { get { return InnerFieldSpinGrid; } }

        private Stroke SelectedStroke;

        public LargeTableView()
        {
            InitializeComponent();
            AddHandler(MouseDownEvent, new MouseButtonEventHandler(Background_MouseDown));
        }

        #region Event handlers

        private void Stroke_MouseEnter(Object sender, MouseEventArgs e)
        {
            Shape hoveredShape = sender as Shape;
            Stroke stroke = hoveredShape.DataContext as Stroke;

            //Debug.WriteLine("mouse enter on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);

            foreach (var strokeShape in StrokeShapes)
            {
                foreach (var shape in strokeShape.Value)
                {
                    if (SelectedStroke == null)
                    {
                        if (strokeShape.Key.Equals(stroke))
                        {
                            shape.Opacity = StrokeOpacity;
                            shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Hover);
                        }
                        else
                        {
                            shape.Opacity = (ShapeType) shape.Tag == ShapeType.SpinShape ? StrokeOpacityHoverUnselectedSpin : StrokeOpacityHoverUnselected;
                            shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
                        }
                    }
                    else
                    {
                        if (strokeShape.Key.Equals(stroke))
                        {
                            if (!SelectedStroke.Equals(strokeShape.Key))
                            {
                                shape.Opacity = StrokeOpacityHover;
                                shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Hover);
                            }
                        }
                        else
                        {
                            if (SelectedStroke.Equals(strokeShape.Key))
                            {
                                shape.Opacity = StrokeOpacity;
                                shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Selected);
                            }
                            else
                            {
                                shape.Opacity = (ShapeType)shape.Tag == ShapeType.SpinShape ? StrokeOpacityDisabledSpin : StrokeOpacityDisabled;
                                shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
                            }
                        }
                    }
                }
            }
        }

        private void Stroke_MouseLeave(Object sender, MouseEventArgs e)
        {
            Shape hoveredShape = sender as Shape;
            Stroke stroke = hoveredShape.DataContext as Stroke;

            //Debug.WriteLine("mouse leave on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);

            foreach (var strokeShape in StrokeShapes)
            {
                foreach (var shape in strokeShape.Value)
                {
                    if (SelectedStroke == null)
                    {
                        shape.Opacity = StrokeOpacity;
                        shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
                    }
                    else
                    {
                        if (!strokeShape.Key.Equals(SelectedStroke))
                        {
                            shape.Opacity = (ShapeType)shape.Tag == ShapeType.SpinShape ? StrokeOpacityDisabledSpin : StrokeOpacityDisabled;
                            shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
                        }
                    }
                }
            }
        }

        private void Stroke_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            Shape hoveredShape = sender as Shape;
            Stroke stroke = hoveredShape.DataContext as Stroke;

            //Debug.WriteLine("mouse down on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);

            foreach (var strokeShape in StrokeShapes)
            {
                foreach (var shape in strokeShape.Value)
                {
                    if (strokeShape.Key.Equals(stroke))
                    {
                        shape.Opacity = StrokeOpacity;
                        shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Selected);
                    }
                    else
                    {
                        shape.Opacity = (ShapeType)shape.Tag == ShapeType.SpinShape ? StrokeOpacityDisabledSpin : StrokeOpacityDisabled;
                        shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
                    }
                }
            }

            SelectedStroke = stroke;

            e.Handled = true;
        }

        private void Background_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (SelectedStroke != null)
            {
                SelectedStroke = null;
                foreach (var strokeShape in StrokeShapes)
                {
                    foreach (var s in strokeShape.Value)
                    {
                        s.Opacity = StrokeOpacity;
                        s.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)s.Tag, ((Stroke)s.DataContext).Stroketechnique, StrokeInteraction.Normal);
                    }
                }
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

        private double GetStrokeThicknessForStroke(ShapeType type, Stroketechnique technique, StrokeInteraction thicknessType)
        {
            if (type == ShapeType.SpinShape)
                switch(thicknessType) {
                    default:
                    case StrokeInteraction.Normal: return StrokeThicknessSpinArrow;
                    case StrokeInteraction.Hover: return StrokeThicknessSpinArrowHover;
                    case StrokeInteraction.Selected: return StrokeThicknessSpinArrowSelected;
                }
            else if (type == ShapeType.Intercept)
                switch (thicknessType)
                {
                    default:
                    case StrokeInteraction.Normal:
                        return technique != null && technique.EnumType == Models.Util.Enums.Stroke.Technique.Smash ? 
                            StrokeThicknessSmashIntercept : StrokeThicknessIntercept;
                    case StrokeInteraction.Hover:
                        return technique != null && technique.EnumType == Models.Util.Enums.Stroke.Technique.Smash ? 
                            StrokeThicknessSmashInterceptHover : StrokeThicknessInterceptHover;
                    case StrokeInteraction.Selected:
                        return technique != null && technique.EnumType == Models.Util.Enums.Stroke.Technique.Smash ? 
                            StrokeThicknessSmashInterceptSelected : StrokeThicknessInterceptSelected;
                }
            else if (type == ShapeType.Debug_preceding)
                switch (thicknessType)
                {
                    default:
                    case StrokeInteraction.Normal: return StrokeThicknessPreceding_Debug;
                    case StrokeInteraction.Hover: return StrokeThicknessPrecedingHover_Debug;
                    case StrokeInteraction.Selected: return StrokeThicknessPrecedingSelected_Debug;
                }
            else if (technique != null && technique.EnumType == Models.Util.Enums.Stroke.Technique.Smash)
                switch (thicknessType)
                {
                    default:
                    case StrokeInteraction.Normal: return StrokeThicknessSmash;
                    case StrokeInteraction.Hover: return StrokeThicknessSmashHover;
                    case StrokeInteraction.Selected: return StrokeThicknessSmashSelected;
                }
            else
                switch (thicknessType)
                {
                    default:
                    case StrokeInteraction.Normal: return StrokeThickness;
                    case StrokeInteraction.Hover: return StrokeThicknessHover;
                    case StrokeInteraction.Selected: return StrokeThicknessSelected;
                }
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
            SelectedStroke = null;

            var brusConverter = new BrushConverter();
            Brush player1brush = (Brush)brusConverter.ConvertFromString("#804F81BD");
            Brush player2brush = (Brush)brusConverter.ConvertFromString("#80C0504D");

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

                foreach (Shape shape in StrokeShapes[s])
                {
                    ToolTip tt = new ToolTip();
                    tt.Background = s.Rally.Winner == MatchPlayer.First ? player1brush : player2brush;
                    tt.Content = "#" + s.Rally.Number + " " +
                        s.Rally.CurrentRallyScore.First + ":" + s.Rally.CurrentRallyScore.Second + " " +
                        "(" + s.Rally.CurrentSetScore.First + ":" + s.Rally.CurrentSetScore.Second + ")";
                    shape.ToolTip = tt;
                }
            }
        }
    }
}
