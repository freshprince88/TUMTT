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

        public string ToogleCalibrationButtonText { get; private set; }

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
        

        public Stroke CurrentStroke
        {
            get { return remoteViewModel.SchlagView.CurrentStroke; }
            set { remoteViewModel.SchlagView.CurrentStroke = value;  }
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
            
            cal.StrokePositionCalculated += OnStrokePositionCalculated;
            ToogleCalibrationButtonText = "Hide Calibration";
            DrawnStrokes = new ObservableCollection<DrawElement>();
        }

        public void OnNewStrokes()
        {
            DrawnStrokes.Clear();
            foreach(Stroke s in Strokes)
            {
                DrawElement dE = createDrawElement(Visibility.Hidden);
                dE.text = s.Number.ToString();
                if (s.Placement != null)
                {
                    dE.g.Visibility = Visibility.Visible;
                    putGridToPosition(new Point(s.Placement.WX, s.Placement.WY), dE);
                }
                DrawnStrokes.Add(dE);
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        public void CalibrateTable()
        {
            remoteViewModel.CalibrateTable();

            if (ToogleCalibrationButtonText.Equals("Show Calibration"))
                ToogleCalibrationButtonText = "Hide Calibration";
            NotifyOfPropertyChange("ToogleCalibrationButtonText");
        }

        public void ToogleCalibration()
        {
            remoteViewModel.ToogleCalibration();

            if (ToogleCalibrationButtonText.Equals("Hide Calibration"))
                ToogleCalibrationButtonText = "Show Calibration";
            else
                ToogleCalibrationButtonText = "Hide Calibration";
            NotifyOfPropertyChange("ToogleCalibrationButtonText");
        }

        private void OnStrokePositionCalculated(object source, StrokePositionCalculatedEventArgs args)
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
                Grid draggedGrid = DrawnStrokes[CurrentStroke.Number - 1].g;

                Placement p = new Placement();
                // reversed the Method putEllipseToPosition()
                double x = draggedGrid.Margin.Left + (draggedGrid.Width / 2);
                double y = draggedGrid.Margin.Top + (draggedGrid.Height / 2);
                p.WX = x / ((double)305/152.5);
                p.WY = y / ((double)548 / (double)274);
                CurrentStroke.Placement = p;
            }
        }

        public void MouseMoved(object sender, MouseEventArgs e)
        {
            if (isEllipseDragged == true)
            {
                Grid g = DrawnStrokes[CurrentStroke.Number - 1].g;

                Point p = e.GetPosition((IInputElement)sender);

                double x = p.X;
                double y = p.Y;
                double left = x - (g.Width / 2);
                double top = y - (g.Height / 2);
                g.Margin = new Thickness(left, top, 0, 0);
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

            CurrentStroke = Strokes[int.Parse(number) - 1];
            isEllipseDragged = true;
        }

    }

    public class DrawElement
    {
        public Grid g { get; set; }
        public Ellipse e { get; set; }
        public string text { get; set; }
    }
}