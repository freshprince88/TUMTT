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
    /// <summary>
    /// Interaction logic for SmallTableView.xaml
    /// </summary>
    public partial class SmallTableView : TableView
    {
        private const int MaxDisplayedStrokes = 4;

        private static BrushConverter brushConverter = new BrushConverter();

        private AutoResetEvent sizeChangedWaitEvent;
        private Rally thisRally;

        public override Grid View_InnerFieldGrid { get { return InnerFieldGrid; } }
        public override Grid View_InnerFieldBehindGrid { get { return InnerFieldBehindGrid; } }
        public override Grid View_InnerFieldHalfDistanceGrid { get { return InnerFieldHalfDistanceGrid; } }
        public override Grid View_InnerFieldSpinGrid { get { return InnerFieldSpinGrid; } }

        public Rally ActiveRally
        {
            get { return (Rally)GetValue(ActiveRallyProperty); }
            set { SetValue(ActiveRallyProperty, value); }
        }

        public bool ShowNumbers
        {
            get { return (bool)GetValue(ShowNumbersProperty); }
            set { SetValue(ShowNumbersProperty, value); }
        }

        public bool ShowStroke1
        {
            get { return (bool)GetValue(ShowStroke1Property); }
            set { SetValue(ShowStroke1Property, value); }
        }

        public bool ShowStroke2
        {
            get { return (bool)GetValue(ShowStroke2Property); }
            set { SetValue(ShowStroke2Property, value); }
        }

        public bool ShowStroke3
        {
            get { return (bool)GetValue(ShowStroke3Property); }
            set { SetValue(ShowStroke3Property, value); }
        }

        public bool ShowStroke4
        {
            get { return (bool)GetValue(ShowStroke4Property); }
            set { SetValue(ShowStroke4Property, value); }
        }

        public static DependencyProperty ActiveRallyProperty = DependencyProperty.Register(
            "ActiveRally", typeof(Rally), typeof(SmallTableView), new PropertyMetadata(default(Rally), new PropertyChangedCallback(OnActiveRallyPropertyChanged)));
        
        public static DependencyProperty ShowNumbersProperty = DependencyProperty.Register(
            "ShowNumbers", typeof(bool), typeof(SmallTableView), new PropertyMetadata(true, new PropertyChangedCallback(OnDisplayTypePropertyChanged)));

        public static DependencyProperty ShowStroke1Property = DependencyProperty.Register(
            "ShowStroke1", typeof(bool), typeof(SmallTableView), new PropertyMetadata(true, new PropertyChangedCallback(OnDisplayTypePropertyChanged)));

        public static DependencyProperty ShowStroke2Property = DependencyProperty.Register(
            "ShowStroke2", typeof(bool), typeof(SmallTableView), new PropertyMetadata(true, new PropertyChangedCallback(OnDisplayTypePropertyChanged)));

        public static DependencyProperty ShowStroke3Property = DependencyProperty.Register(
            "ShowStroke3", typeof(bool), typeof(SmallTableView), new PropertyMetadata(true, new PropertyChangedCallback(OnDisplayTypePropertyChanged)));

        public static DependencyProperty ShowStroke4Property = DependencyProperty.Register(
            "ShowStroke4", typeof(bool), typeof(SmallTableView), new PropertyMetadata(true, new PropertyChangedCallback(OnDisplayTypePropertyChanged)));

        private static void OnDisplayTypePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SmallTableView view = (SmallTableView)sender;
            if (e.Property == ShowNumbersProperty)
            {
                if ((bool)e.NewValue)
                    foreach (Stroke s in view.StrokeShapes.Keys) view.AddStrokeNumbers(s);
                else
                    view.HideElementsByTag(ElementType.StrokeNumber);
            }
            else if (e.Property == ShowStroke1Property)
            {
                if ((bool)e.NewValue)
                    view.thisRally.Strokes.Apply(s => { if (s.Number == 1) view.AddStroke(s); });
                else
                    view.HideStrokeByNumber(1);
            }
            else if (e.Property == ShowStroke2Property)
            {
                if ((bool)e.NewValue)
                    view.thisRally.Strokes.Apply(s => { if (s.Number == 2) view.AddStroke(s); });
                else
                    view.HideStrokeByNumber(2);
            }
            else if (e.Property == ShowStroke3Property)
            {
                if ((bool)e.NewValue)
                    view.thisRally.Strokes.Apply(s => { if (s.Number == 3) view.AddStroke(s); });
                else
                    view.HideStrokeByNumber(3);
            }
            else if (e.Property == ShowStroke4Property)
            {
                if ((bool)e.NewValue)
                    view.thisRally.Strokes.Apply(s => { if (s.Number == 4) view.AddStroke(s); });
                else
                    view.HideStrokeByNumber(4);
            }
        }

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

        private static void OnActiveRallyPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Debug.WriteLine("SmallTableView: new active rally! was: {0}, is: {1}", e.OldValue == null ? "[none]" : ((Rally)e.OldValue).Number.ToString(), ((Rally)e.NewValue).Number);

            SmallTableView view = (SmallTableView)sender;
            if (view.thisRally.Number == view.ActiveRally.Number)
            {
                view.SmallTableViewBorder.BorderThickness = new Thickness(2);
                view.TableGrid.Background = (Brush)brushConverter.ConvertFrom("#e2e7e0");
            }
            else
            {
                view.SmallTableViewBorder.BorderThickness = new Thickness(1);
                view.TableGrid.Background = Brushes.Transparent;
            }
        }

        private void Table_MouseEnter(object sender, MouseEventArgs e)
        {
            SmallTableViewBorder.BorderThickness = new Thickness(2);
            TableGrid.Background = (Brush) brushConverter.ConvertFrom("#e2e7e0");
        }

        private void Table_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ActiveRally == null || ActiveRally.Number != thisRally.Number)
            {
                SmallTableViewBorder.BorderThickness = new Thickness(1);
                TableGrid.Background = Brushes.Transparent;
            }
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
            
            // add new lists of shapes for each stroke - up to MaxDisplayedStrokes many for small table!
            int maxStrokeDisplayCounter = 0;
            foreach (Stroke s in strokes)
            {
                thisRally = s.Rally;

                maxStrokeDisplayCounter++;
                if (maxStrokeDisplayCounter > MaxDisplayedStrokes)
                    break;
                StrokeShapes[s] = new List<Shape>();
                StrokeElements[s] = new List<FrameworkElement>();
            }

            // once size of this view changed, we know it's been fully added: add the actual stroke shapes (they need the actual size)
            new Thread(new ThreadStart(() =>
            {
                sizeChangedWaitEvent.WaitOne();

                Dispatcher.Invoke(() =>
                {                    
                    foreach (Stroke stroke in thisRally.Strokes)
                    {
                        if (stroke.Number == 1 && ShowStroke1 || stroke.Number == 2 && ShowStroke2 ||
                            stroke.Number == 3 && ShowStroke3 || stroke.Number == 4 && ShowStroke4)
                        AddStroke(stroke);
                    }
                });
            })).Start();
        }

        private void AddStroke(Stroke stroke)
        {
            AddServiceStrokesSpinShapes(stroke);
            AddStrokesDirectionShapes(stroke);
            AddInterceptArrows(stroke);
            AddStrokesArrowtips(stroke);
            AddStrokeNumbers(stroke);
        }

        private void AddStrokeNumbers(Stroke stroke)
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

                    Shape shape = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Arrowtip);
                    if (shape == null) return;
                    GetPointForShapeRelativeToGrid(shape, PointType.Start, gridOfStroke, out X1, out Y1);

                    if (X1.Equals(double.NaN) || Y1.Equals(double.NaN))
                        return;

                    // arbitrary positioning relative to stroke arrow
                    X1 = X1 + 7;
                    Y1 = Y1 - 2;

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

        #region Helper methods

        protected override double GetAdjustedX(Stroke stroke, double oldX)
        {
            Grid grid = GetGridForStroke(stroke);
            //Debug.WriteLine("{0} : aw={1} mr={2} ml={3}", stroke.Rally.Number, grid.ActualWidth, grid.Margin.Right, grid.Margin.Left);
            if (IsStrokeBottomToTop(stroke))
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
            if (IsStrokeBottomToTop(stroke))
            {
                // stroke in the upper half of table, flip y for even-numbered strokes
                return stroke.Number % 2 == 1 ? oldY + (TableBorder.Margin.Top - grid.Margin.Top) : grid.ActualHeight - oldY - (TableBorder.Margin.Bottom - grid.Margin.Bottom);
            }
            else
            {
                // stroke in the lower half of table, flip y for odd-numbered strokes
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
            
            // DEBUG
            shape.MouseEnter += new MouseEventHandler(Stroke_MouseEnter);
            shape.MouseLeave += new MouseEventHandler(Stroke_MouseLeave);
            shape.DataContext = stroke;
            Message.SetAttach(shape, "StrokeSelected($DataContext)");
            // ---
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

        private void HideStrokeByNumber(int number)
        {
            foreach (Stroke stroke in StrokeShapes.Keys)
            {
                if (stroke.Number == number)
                {
                    foreach (Shape shape in StrokeShapes[stroke])
                    {
                        shape.Visibility = Visibility.Hidden;
                    }
                    foreach (FrameworkElement element in StrokeElements[stroke])
                    {
                        element.Visibility = Visibility.Hidden;
                    }
                }
            }
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
                        s.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)s.Tag, stroke.Stroketechnique, true);
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
                        s.StrokeThickness = GetStrokeThicknessForStroke((ShapeType)s.Tag, stroke.Stroketechnique, false);
                    }
                    else
                    {
                        s.Opacity = 1;
                    }
                }
            }
        }

        private double GetStrokeThicknessForStroke(ShapeType tag, Stroketechnique technique, bool hover)
        {
            if (tag == ShapeType.SpinShape)
                return hover ? StrokeThicknessSpinArrowHover : StrokeThicknessSpinArrow;
            else if (tag == ShapeType.Intercept)
                return hover ? StrokeThicknessInterceptHover : StrokeThicknessIntercept;
            else if (tag == ShapeType.Debug_preceding)
                return hover ? StrokeThicknessPrecedingHover_Debug : StrokeThicknessPreceding_Debug;
            else if (technique != null && technique.EnumType == Models.Util.Enums.Stroke.Technique.Smash)
                return hover ? StrokeThicknessSmashHover : StrokeThicknessSmash;
            else
                return hover ? StrokeThicknessHover : StrokeThickness;
        }

        #endregion
    }
}
