using Caliburn.Micro;
using System.Collections.ObjectModel;
using TT.Lib.Events;
using TT.Models;
using System;
using System.Windows.Shapes;
using TT.Scouter.Util.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TT.Scouter.ViewModels
{
    public class RemotePositionsRallyViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private RemoteViewModel remoteViewModel;

        private bool isEllipseDragged = false;
        private Calibration cal;

        public string ToogleCalibrationButtonImage { get; private set; }

        private ObservableCollection<DrawElement> _drawnStrokes;

        public ObservableCollection<DrawElement> DrawnStrokes
        {
            get { return _drawnStrokes; }
            set
            {
                if (_drawnStrokes != value)
                {
                    _drawnStrokes = value;

                    NotifyOfPropertyChange("DrawnStrokes");
                }
            }
        }

        private bool _showTopRightArrow;
        public bool showTopRightArrow
        {
            get { return _showTopRightArrow; }
            set
            {
                _showTopRightArrow = value;
                NotifyOfPropertyChange("showTopRightArrow");
            }
        }

        private bool _showBottomRightArrow;
        public bool showBottomRightArrow
        {
            get { return _showBottomRightArrow; }
            set
            {
                _showBottomRightArrow = value;
                NotifyOfPropertyChange("showBottomRightArrow");
            }
        }

        private bool _showBottomLeftArrow;
        public bool showBottomLeftArrow
        {
            get { return _showBottomLeftArrow; }
            set
            {
                _showBottomLeftArrow = value;
                NotifyOfPropertyChange("showBottomLeftArrow");
            }
        }

        private bool _showTopLeftArrow;
        public bool showTopLeftArrow
        {
            get { return _showTopLeftArrow; }
            set
            {
                _showTopLeftArrow = value;
                NotifyOfPropertyChange("showTopLeftArrow");
            }
        }

        private int _maxVisibleStrokes;
        public string maxVisibleStrokes
        {
            get { return _maxVisibleStrokes.ToString(); }
            set
            {
                if (value == null || value.Equals("ALL"))
                {
                    _maxVisibleStrokes = int.MaxValue;
                }
                else
                {
                    _maxVisibleStrokes = int.Parse(value);
                }
            }
        }

        public Stroke CurrentStroke
        {
            get { return remoteViewModel.SchlagView.CurrentStroke; }
            set { remoteViewModel.SchlagView.CurrentStroke = value; }
        }

        public ObservableCollection<Stroke> Strokes
        {
            get
            {
                return remoteViewModel.SchlagView.Strokes;
            }
        }



        public RemotePositionsRallyViewModel(RemoteViewModel remoteViewModel, Calibration cal)
        {
            this.remoteViewModel = remoteViewModel;

            this.cal = cal;
            cal.StrokePositionCalculated += OnStrokePositionCalculated;
            cal.PointAdded += OnPointAdded;


            ToogleCalibrationButtonImage = "/resources/visible.png";
            DrawnStrokes = new ObservableCollection<DrawElement>();

            showBottomLeftArrow = false;
            showBottomRightArrow = false;
            showTopLeftArrow = false;
            showTopRightArrow = false;

            maxVisibleStrokes = "ALL";
        }

        public void OnNewStrokes()
        {
            DrawnStrokes.Clear();
            foreach(Stroke s in Strokes)
            {
                DrawElement dE = createDrawElement(Visibility.Hidden);
                dE.text = s.Number.ToString();
                s.StrokePlacementChanged += S_StrokePlacementChanged;
                if (s.Placement != null && s.Placement.WX >= 0 && s.Placement.WY >= 0)
                {
                    dE.g.Visibility = Visibility.Visible;
                    putGridToPosition(new Point(s.Placement.WX, s.Placement.WY), dE);
                }
                DrawnStrokes.Add(dE);
            }
        }

        public void OnCurrentStrokeChanged()
        {
            CurrentStroke.StrokePlacementChanged += S_StrokePlacementChanged;


            showCorrectStrokes();
            try
            {
                for (int i = 0; i < DrawnStrokes.Count; i++)
                    DrawnStrokes[i].e.Fill = System.Windows.Media.Brushes.Transparent;
                DrawnStrokes[CurrentStroke.Number - 1].e.Fill = System.Windows.Media.Brushes.Black;
            }
            catch (Exception e) { Console.Write(e.Message); }
        }

        private void S_StrokePlacementChanged(object source, EventArgs args)
        {
            Stroke s = ((Stroke)source);
            if (!s.Course.Equals("Net/Out"))
            {
                if (s.Placement != null && s.Placement.WX >= 0 && s.Placement.WY >= 0)
                {
                    Point pos = new Point(s.Placement.WX, s.Placement.WY);
                    OnStrokePositionCalculated(source, new StrokePositionCalculatedEventArgs(pos));
                } else {
                    OnStrokePositionDeleted(source, new EventArgs());
                }
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        public void CalibrateTable()
        {
            resetCalibrationStatus();
            remoteViewModel.CalibrateTable();

            if (ToogleCalibrationButtonImage.Equals("/resources/visible.png"))
                ToogleCalibrationButtonImage = "/resources/hidden.png";
            NotifyOfPropertyChange("ToogleCalibrationButtonImage");
        }

        public void ToogleCalibration()
        {
            remoteViewModel.ToogleCalibration();

            if (ToogleCalibrationButtonImage.Equals("/resources/hidden.png"))
                ToogleCalibrationButtonImage = "/resources/visible.png";
            else
                ToogleCalibrationButtonImage = "/resources/hidden.png";
            NotifyOfPropertyChange("ToogleCalibrationButtonImage");
        }

        private void resetCalibrationStatus()
        {
            showTopLeftArrow = true;
            showTopRightArrow = false;
            showBottomLeftArrow = false;
            showBottomRightArrow = false;
        }

        public void DeleteStroke()
        {
            CurrentStroke.Placement = null;
        }

        public void toggleMidlines()
        {
            switch (cal.MidLines.Count)
            {
                case 0:
                    cal.drawMidlines();
                    break;
                case 2:
                    cal.toggleMidlines();
                    break;
                default:
                    break;
            }
        }

        public void toggleGridlines()
        {
            switch (cal.GridLines.Count)
            {
                case 0:
                    cal.drawGridlines();
                    break;
                case 7:
                    cal.toggleGridlines();
                    break;
                default:
                    break;
            }
        }

        private void OnStrokePositionCalculated(object source, StrokePositionCalculatedEventArgs args)
        {
            if (!CurrentStroke.Course.Equals("Net/Out"))
            {
                if (CurrentStroke.Number > DrawnStrokes.Count)
                {
                    while (CurrentStroke.Number > DrawnStrokes.Count)
                    {
                        DrawnStrokes.Add(createDrawElement(Visibility.Hidden));
                    }
                }

                DrawElement dE = DrawnStrokes[CurrentStroke.Number - 1];
                dE.text = CurrentStroke.Number.ToString();
                putGridToPosition(args.Position, dE);
                dE.g.Visibility = Visibility.Visible;

                showCorrectStrokes();
            }
        }

        private void OnPointAdded(object source, PointAddedEventArgs args)
        {
            if (args.NumberOfPoints == 1)
            {
                showTopLeftArrow = false;
                showTopRightArrow = true;
            } else if (args.NumberOfPoints == 2)
            {
                showTopRightArrow = false;
                showBottomRightArrow = true;
            } else if (args.NumberOfPoints == 3)
            {
                showBottomRightArrow = false;
                showBottomLeftArrow = true;
            } else if (args.NumberOfPoints == 4)
            {
                showBottomLeftArrow = false;
            }
        }

        private void OnStrokePositionDeleted(object source, EventArgs args)
        {
            Stroke s = (Stroke)source;
            DrawElement dE = DrawnStrokes[s.Number - 1];
            dE.g.Visibility = Visibility.Hidden;
        }

        private DrawElement putGridToPosition(Point Position, DrawElement drawElement)
        {
            double x = Position.X * ((double)305 / 152.5);
            double y = Position.Y * ((double)548 / (double)274);
            double left = x - (drawElement.g.Width / 2);
            double top = y - (drawElement.g.Height / 2);
            drawElement.g.Margin = new Thickness(left, top, 0, 0);

            return drawElement;
        }

        private DrawElement createDrawElement(Visibility visibility)
        {
            Ellipse e = new Ellipse();
            e.Stroke = System.Windows.Media.Brushes.Black;
            e.Fill = System.Windows.Media.Brushes.Transparent;
            e.StrokeThickness = 5;
            Grid g = new Grid();
            g.Height = 40;
            g.Width = 40;
            g.IsHitTestVisible = true;
            g.Background = System.Windows.Media.Brushes.Transparent;
            g.Visibility = visibility;

            DrawElement ele = new DrawElement();
            ele.e = e;
            ele.g = g;
            return ele;
        }

        public void GridUnclicked(object sender, MouseButtonEventArgs e)
        {
            if (isEllipseDragged)
            {
                isEllipseDragged = false;
                changePlacementOfCurrentStroke();
            }
        }

        private void changePlacementOfCurrentStroke()
        {
            Grid draggedGrid = DrawnStrokes[CurrentStroke.Number - 1].g;

            Placement p = new Placement();

            // reversed the Method putEllipseToPosition()
            double x = draggedGrid.Margin.Left + (draggedGrid.Width / 2);
            double y = draggedGrid.Margin.Top + (draggedGrid.Height / 2);
            p.WX = x / ((double)305 / 152.5);
            p.WY = y / ((double)548 / (double)274);
            CurrentStroke.Placement = p;
        }

        public void MouseMoved(object sender, MouseEventArgs e)
        {
            if (isEllipseDragged == true)
            {
                Grid g = DrawnStrokes[CurrentStroke.Number - 1].g;

                Point p = e.GetPosition((IInputElement)sender);
                ItemsControl ele = (ItemsControl)sender;

                if (p.X <= ele.ActualWidth && p.Y <= ele.ActualHeight && p.X >= 0 && p.Y >= 0)
                {
                    double x = p.X;
                    double y = p.Y;
                    double left = x - (g.Width / 2);
                    double top = y - (g.Height / 2);
                    g.Margin = new Thickness(left, top, 0, 0);
                    changePlacementOfCurrentStroke();
                }
                else
                {
                    GridUnclicked(null, null);
                }

            }
        }

        public void GridClicked(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            string number = "";
            foreach (Object t in grid.Children)
            {
                if (t.GetType() == typeof(TextBlock)) number = ((TextBlock)t).Text;
            }

            // if clicked on another stroke change current Stroke
            if (int.Parse(number) != CurrentStroke.Number)
            {
                CurrentStroke = Strokes[int.Parse(number) - 1];
            }

            // when left mouse button is clicked then drag ellipse, when right button is clicked delete placement
            if (e.ChangedButton == MouseButton.Left)
            {
                isEllipseDragged = true;
            } else if(e.ChangedButton== MouseButton.Right)
            {
                DeleteStroke();
            }
        }

        public void OnMaxStrokesChanged()
        {
            showCorrectStrokes();
        }

        private void showCorrectStrokes()
        {
            for (int i = 0; i < DrawnStrokes.Count; i++)
            {
                if (Math.Abs((CurrentStroke.Number - 1 - i)) <= (_maxVisibleStrokes - 1) / 2)
                {
                    if (Strokes.Count > 0 && Strokes[i].Placement != null && Strokes[i].Placement.WX >= 0 && Strokes[i].Placement.WY >= 0) DrawnStrokes[i].g.Visibility = Visibility.Visible;
                } else
                {
                    DrawnStrokes[i].g.Visibility = Visibility.Hidden;
                }
            }
        }

    }

    public class DrawElement
    {
        public Grid g { get; set; }
        public Ellipse e { get; set; }
        public string text { get; set; }
    }
}