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

        protected new const double StrokeThickness = 4;
        protected new const double StrokeThicknessSpinArrow = 2.25;
        protected new const double StrokeThicknessSmash = 6.0;
        protected new const double StrokeThicknessIntercept = 2.75;
        protected new const double StrokeThicknessSmashIntercept = 4;
        protected const double StrokeThicknessArrowtipNetOrOut = 3;

        protected const double ArrowtipScaleNoDirection = 1.5;

        private static BrushConverter brushConverter = new BrushConverter();

        private AutoResetEvent sizeChangedWaitEvent;
        private Rally thisRally;

        public override Grid View_InnerFieldGrid { get { return InnerFieldGrid; } }
        public override Grid View_InnerFieldBehindGrid { get { return InnerFieldBehindGrid; } }
        public override Grid View_InnerFieldHalfDistanceGrid { get { return InnerFieldHalfDistanceGrid; } }
        public override Grid View_InnerFieldSpinGrid { get { return InnerFieldSpinGrid; } }

        #region Dependency properties

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

        #endregion

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

            SmallTableView smallTableView = (SmallTableView)sender;

            if (smallTableView.thisRally.Number == smallTableView.ActiveRally.Number)
            {
                ((Grid)smallTableView.Parent).Background = (Brush)brushConverter.ConvertFrom("#e2e7e0");
            }
            else
            {
                ((Grid)smallTableView.Parent).Background = Brushes.Transparent;
            }
        }

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
                    view.thisRally.Strokes.Apply(s => { if (s.Number == 1 && view.ShowStroke1) view.AddStroke(s); });
                else
                    view.HideStrokeByNumber(1);
            }
            else if (e.Property == ShowStroke2Property)
            {
                if ((bool)e.NewValue)
                    view.thisRally.Strokes.Apply(s => { if (s.Number == 2 && view.ShowStroke2) view.AddStroke(s); });
                else
                    view.HideStrokeByNumber(2);
            }
            else if (e.Property == ShowStroke3Property)
            {
                if ((bool)e.NewValue)
                    view.thisRally.Strokes.Apply(s => { if (s.Number == 3 && view.ShowStroke3) view.AddStroke(s); });
                else
                    view.HideStrokeByNumber(3);
            }
            else if (e.Property == ShowStroke4Property)
            {
                if ((bool)e.NewValue)
                    view.thisRally.Strokes.Apply(s => { if (s.Number == 4 && view.ShowStroke4) view.AddStroke(s); });
                else
                    view.HideStrokeByNumber(4);
            }
        }

        private void SmallTable_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid parentGrid = (Grid)((SmallTableView)((Grid)sender).Parent).Parent;
            parentGrid.Background = (Brush)brushConverter.ConvertFrom("#e2e7e0");
        }

        private void SmallTable_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid parentGrid = (Grid)((SmallTableView)((Grid)sender).Parent).Parent;
            if (ActiveRally == null || ActiveRally.Number != thisRally.Number)
            {
                parentGrid.Background = Brushes.Transparent;
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
                    StrokeShapes.Keys.Apply(s => AddStroke(s));                    
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
            if (!ShowNumbersForStroke(stroke))
                return;

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

                    textBlock.Width = 12;
                    textBlock.Height = 17;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                    textBlock.VerticalAlignment = VerticalAlignment.Top;

                    double X1, Y1;

                    Grid gridOfStroke = GetGridForStroke(stroke);

                    Shape shape = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Arrowtip);
                    if (shape == null) return;
                    GetPointOfShapeRelativeToGrid(shape, PointType.Start, gridOfStroke, out X1, out Y1);

                    if (X1.Equals(double.NaN) || Y1.Equals(double.NaN))
                        return;

                    // arbitrary positioning relative to stroke arrow
                    X1 = X1 + 10;
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

        protected override void ApplyStyle(Stroke stroke, Shape shape)
        {
            base.ApplyStyle(stroke, shape);
            switch ((ShapeType)shape.Tag)
            {
                case ShapeType.Arrowtip:
                    if (stroke.EnumCourse == Models.Util.Enums.Stroke.Course.NetOut)
                        shape.StrokeThickness = StrokeThicknessArrowtipNetOrOut;
                    else
                        shape.StrokeThickness = StrokeThickness;

                    ScaleTransform scaleTransform = shape.RenderTransform as ScaleTransform;
                    Shape direction = StrokeShapes[stroke].Find(s => (ShapeType)s.Tag == ShapeType.Direction);
                    scaleTransform.ScaleX = ShowDirection && direction != null ? 1.0 : ArrowtipScaleNoDirection;
                    scaleTransform.ScaleY = ShowDirection && direction != null ? 1.0 : ArrowtipScaleNoDirection;
                    break;
                case ShapeType.Intercept:
                    if (stroke.Stroketechnique != null && stroke.Stroketechnique.EnumType == Models.Util.Enums.Stroke.Technique.Smash)
                        shape.StrokeThickness = StrokeThicknessSmashIntercept;
                    else
                        shape.StrokeThickness = StrokeThicknessIntercept;
                    break;
                case ShapeType.SpinShape:
                    shape.StrokeThickness = StrokeThicknessSpinArrow;
                    break;
                default:
                    shape.StrokeThickness = StrokeThickness;
                    break;
            }
        }

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

        protected override bool ShowDirectionForStroke(Stroke stroke)
        {
            return ShowDirection && (
                stroke.Number == 1 && ShowStroke1 ||
                stroke.Number == 2 && ShowStroke2 ||
                stroke.Number == 3 && ShowStroke3 ||
                stroke.Number == 4 && ShowStroke4
                );
        }

        protected override bool ShowInterceptForStroke(Stroke stroke)
        {
            return ShowIntercept && (
                stroke.Number == 1 && ShowStroke1 ||
                stroke.Number == 2 && ShowStroke2 ||
                stroke.Number == 3 && ShowStroke3 ||
                stroke.Number == 4 && ShowStroke4
                );
        }

        protected override bool ShowSpinForStroke(Stroke stroke)
        {
            return ShowSpin && (
                stroke.Number == 1 && ShowStroke1 ||
                stroke.Number == 2 && ShowStroke2 ||
                stroke.Number == 3 && ShowStroke3 ||
                stroke.Number == 4 && ShowStroke4
                );
        }

        private bool ShowNumbersForStroke(Stroke stroke)
        {
            return ShowNumbers && (
                stroke.Number == 1 && ShowStroke1 ||
                stroke.Number == 2 && ShowStroke2 ||
                stroke.Number == 3 && ShowStroke3 ||
                stroke.Number == 4 && ShowStroke4
                );
        }

        protected override bool ShowStroke(Stroke stroke)
        {
            switch (stroke.Number)
            {
                case 1: return ShowStroke1;
                case 2: return ShowStroke2;
                case 3: return ShowStroke3;
                case 4: return ShowStroke4;
                default: return false;
            }
        }

        protected override double GetSecondStrokePrecedingStartY()
        {
            return InnerFieldBehindGrid.ActualHeight;
        }

        protected override void AttachEventHandlerToShape(Shape shape, Stroke stroke)
        {
            // no event handlers on strokes in small table view            
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

        protected override void DirectionShapesHidden()
        {
            foreach (List<Shape> shapes in StrokeShapes.Values)
            {
                foreach (Shape shape in shapes)
                {
                    if ((ShapeType)shape.Tag == ShapeType.Arrowtip)
                    {
                        ScaleTransform scaleTransform = shape.RenderTransform as ScaleTransform;
                        scaleTransform.ScaleX = ArrowtipScaleNoDirection;
                        scaleTransform.ScaleY = ArrowtipScaleNoDirection;
                    }
                }
            }
        }

        #endregion
    }
}
