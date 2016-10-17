﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TT.Lib.Converters;
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
        private int handledEventTime;

        private static MatchPlayerToBrushConverter matchPlayerToBrushConverter = new MatchPlayerToBrushConverter();
        private static ScoreToStringConverter scoreToStringConverter = new ScoreToStringConverter();

        public LargeTableView()
        {
            InitializeComponent();
            AddHandler(MouseDownEvent, new MouseButtonEventHandler(Background_MouseDown));
            Message.SetAttach(this, "[Event MouseDown] = [Action StrokeSelected(null)]");
        }

        #region Event handlers

        private void Stroke_MouseEnter(object sender, MouseEventArgs e)
        {
            Shape hoveredShape = sender as Shape;
            Stroke hoveredStroke = hoveredShape.DataContext as Stroke;

            //Debug.WriteLine("mouse enter on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);

            Brush fill;
            double opacity;
            double strokeThickness;

            foreach (var strokeShape in StrokeShapes)
            {
                foreach (var shape in strokeShape.Value)
                {
                    if (SelectedStroke == null)
                    {
                        if (strokeShape.Key.Equals(hoveredStroke))
                        {
                            //shape.Fill = GetShapeFillForInteractionType((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Hover, out fill, out opacity, out strokeThickness);
                            shape.Opacity = GetShapeOpacityForInteractionType((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Hover); //StrokeOpacity;
                            shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Hover);
                        }
                        else
                        {
                            shape.Opacity = GetShapeOpacityForInteractionType((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal); //(ShapeType) shape.Tag == ShapeType.SpinShape ? StrokeOpacityHoverUnselectedSpin : StrokeOpacityHoverUnselected;
                            shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
                        }
                    }
                    else
                    {
                        if (strokeShape.Key.Equals(hoveredStroke))
                        {
                            if (!SelectedStroke.Equals(strokeShape.Key))
                            {
                                shape.Opacity = GetShapeOpacityForInteractionType((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Hover); //StrokeOpacityHover;
                                shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Hover);
                            }
                        }
                        else
                        {
                            if (SelectedStroke.Equals(strokeShape.Key))
                            {
                                shape.Opacity = GetShapeOpacityForInteractionType((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Selected); //StrokeOpacity;
                                shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Selected);
                            }
                            else
                            {
                                shape.Opacity = GetShapeOpacityForInteractionType((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);  //(ShapeType)shape.Tag == ShapeType.SpinShape ? StrokeOpacityDisabledSpin : StrokeOpacityDisabled;
                                shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
                            }
                        }
                    }
                }
            }
        }

        private void Stroke_MouseLeave(object sender, MouseEventArgs e)
        {
            Shape leftShape = sender as Shape;
            Stroke leftStroke = leftShape.DataContext as Stroke;

            //Debug.WriteLine("mouse leave on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);

            foreach (var strokeShape in StrokeShapes)
            {
                foreach (var shape in strokeShape.Value)
                {
                    if (SelectedStroke == null)
                    {
                        shape.Opacity = GetShapeOpacityForInteractionType((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal); //StrokeOpacity;
                        shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
                    }
                    else
                    {
                        if (!strokeShape.Key.Equals(SelectedStroke))
                        {
                            shape.Opacity = GetShapeOpacityForInteractionType((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal); //(ShapeType)shape.Tag == ShapeType.SpinShape ? StrokeOpacityDisabledSpin : StrokeOpacityDisabled;
                            shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
                        }
                    }
                }
            }
        }

        private void Stroke_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Shape clickedShape = sender as Shape;
            Stroke clickedStroke = clickedShape.DataContext as Stroke;

            //Debug.WriteLine("mouse down on stroke {0} of rally {1}", stroke.Number, stroke.Rally.Number);

            foreach (var strokeShape in StrokeShapes)
            {
                foreach (var shape in strokeShape.Value)
                {
                    if (strokeShape.Key.Equals(clickedStroke))
                    {
                        shape.Opacity = GetShapeOpacityForInteractionType((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Selected); //StrokeOpacity;
                        shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Selected);
                    }
                    else
                    {
                        shape.Opacity = GetShapeOpacityForInteractionType((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);  //(ShapeType)shape.Tag == ShapeType.SpinShape ? StrokeOpacityDisabledSpin : StrokeOpacityDisabled;
                        shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
                    }
                }
            }

            SelectedStroke = clickedStroke;

            handledEventTime = e.Timestamp;
        }

        private void Background_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Timestamp == handledEventTime)
            {
                e.Handled = true;
                return;
            }

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
            shape.DataContext = stroke;
            Message.SetAttach(shape, "[Event MouseDown] = [Action StrokeSelected($DataContext)]");

            shape.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
            shape.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
            shape.MouseDown += new MouseButtonEventHandler(Stroke_MouseDown);
        }

        #endregion

        #region Helper methods
        
        protected override bool ShowDirectionForStroke(Stroke stroke)
        {
            return ShowDirection && stroke.EnumCourse != Models.Util.Enums.Stroke.Course.NetOut;
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

        private double GetShapeOpacityForInteractionType(ShapeType shapeType, Stroketechnique stroketechnique, StrokeInteraction interactionType)
        {
            if (SelectedStroke == null)
            {
                if (interactionType == StrokeInteraction.Normal)
                    if (shapeType == ShapeType.SpinShape)
                        return StrokeOpacityHoverUnselectedSpin;
                    else
                        return StrokeOpacityHoverUnselected;
                else
                    return StrokeOpacity;
            }
            else
            {
                if (interactionType == StrokeInteraction.Selected)
                    return StrokeOpacity;
                else if (interactionType == StrokeInteraction.Hover)
                    return StrokeOpacityHover;
                else if (shapeType == ShapeType.SpinShape)
                    return StrokeOpacityDisabledSpin;
                //else if (stroketechnique.EnumType == Models.Util.Enums.Stroke.Technique.Banana)
                //    return StrokeOpacityDisabledBanana;
                else
                    return StrokeOpacityDisabled;
            }
        }

        private Brush GetShapeFillForInteractionType(Shape shape, Stroketechnique stroketechnique, StrokeInteraction interactionType, out Brush fill, out double opacity, out double strokeThickness)
        {
            if (interactionType == StrokeInteraction.Normal)
            {
                if (SelectedStroke == null)
                {
                    if ((ShapeType)shape.Tag == ShapeType.SpinShape)
                    {
                        opacity = StrokeOpacityHoverUnselectedSpin;
                    }
                    else
                    {
                        opacity = StrokeOpacityHoverUnselected;
                    }
                }
                else
                {
                    if ((ShapeType)shape.Tag == ShapeType.SpinShape)
                    {
                        opacity = StrokeOpacityDisabledSpin;
                    }
                    else
                    {
                        opacity = StrokeOpacityDisabled;
                    }
                    if (stroketechnique.EnumType == Models.Util.Enums.Stroke.Technique.Banana)
                        fill = StrokeFillDisabledBanana;
                }
            }
            else if (interactionType == StrokeInteraction.Hover)
            {
                if (SelectedStroke != null)
                {
                    opacity = StrokeOpacityHover;
                }
            }
            else if (interactionType == StrokeInteraction.Selected)
            {
                if (SelectedStroke != null)
                {
                    opacity = StrokeOpacity;
                }
            }
            else
            {
                if (SelectedStroke == null)
                {
                    opacity = StrokeOpacity;
                }
            }


            // old
            if (SelectedStroke == null)
            {
                if (interactionType == StrokeInteraction.Normal)
                    if ((ShapeType)shape.Tag == ShapeType.SpinShape)
                    {
                        opacity = StrokeOpacityHoverUnselectedSpin;
                    }
                    else
                    {
                        opacity = StrokeOpacityHoverUnselected;
                    }
                else
                {
                    opacity = StrokeOpacity;
                }
            }
            else
            {
                if (interactionType == StrokeInteraction.Selected)
                {
                    opacity = StrokeOpacity;
                }
                else if (interactionType == StrokeInteraction.Hover)
                {
                    opacity = StrokeOpacityHover;
                }
                else
                {
                    if ((ShapeType)shape.Tag == ShapeType.SpinShape)
                    {
                        opacity = StrokeOpacityDisabledSpin;
                    }
                    else
                    {
                        opacity = StrokeOpacityDisabled;
                    }
                    if (stroketechnique.EnumType == Models.Util.Enums.Stroke.Technique.Banana)
                        fill = StrokeFillDisabledBanana;
                }
            }
            // ---
        }

        protected override void ApplyStyle(Stroke stroke, Shape shape)
        {
            base.ApplyStyle(stroke, shape);

            if (SelectedStroke == null)
            {
                shape.Opacity = StrokeOpacity;
                if (shape.DataContext != null)
                    shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
            }
            else if (stroke.Equals(SelectedStroke))
            {
                shape.Opacity = StrokeOpacity;
                if (shape.DataContext != null)
                    shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Selected);
            }
            else
            {
                shape.Opacity = (ShapeType)shape.Tag == ShapeType.SpinShape ? StrokeOpacityDisabledSpin : StrokeOpacityDisabled;
                if (shape.DataContext != null)
                    shape.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)shape.Tag, ((Stroke)shape.DataContext).Stroketechnique, StrokeInteraction.Normal);
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
                    tt.Background = (Brush)matchPlayerToBrushConverter.Convert(s.Rally.Winner, typeof(Brush), null, System.Globalization.CultureInfo.CurrentCulture);
                    tt.Content = "#" + s.Rally.Number + " " +
                        scoreToStringConverter.Convert(new object[] { s.Rally.CurrentRallyScore, s.Rally.CurrentSetScore }, typeof(string), false, System.Globalization.CultureInfo.CurrentCulture);
                    shape.ToolTip = tt;
                }
            }
        }
    }
}
