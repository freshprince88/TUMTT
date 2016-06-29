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
        private int clickOffsetX, clickOffsetY;

        public string ToogleCalibrationButtonText { get; private set; }

        private ObservableCollection<Ellipse> _drawnStrokes;

        public ObservableCollection<Ellipse> DrawnStrokes
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
        

        public Schlag CurrentStroke
        {
            get { return remoteViewModel.SchlagView.CurrentStroke; }
        }

        public ObservableCollection<Schlag> Strokes
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
            DrawnStrokes = new ObservableCollection<Ellipse>();
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
            if (CurrentStroke.Nummer > DrawnStrokes.Count)
            {
                while (CurrentStroke.Nummer > DrawnStrokes.Count)
                {
                    DrawnStrokes.Add(createEllipseAtPosition(Visibility.Hidden));
                }
            }

            Ellipse e = DrawnStrokes[CurrentStroke.Nummer - 1];
            putEllipseToPosition(args.Position, e);
            e.Visibility = Visibility.Visible;
        }

        private Ellipse putEllipseToPosition(Point Position, Ellipse e)
        {
            double x = Position.X * ((double)305 / 152.5);
            double y = Position.Y * ((double)548 / 274);
            double left = x - (e.Width / 2);
            double top = y - (e.Height / 2);
            e.Margin = new Thickness(left, top, 0, 0);

            return e;
        }

        private Ellipse createEllipseAtPosition(Visibility visibility)
        {
            Ellipse e = new Ellipse();
            e.Height = 10;
            e.Width = 10;
            e.Stroke = System.Windows.Media.Brushes.Black;
            e.StrokeThickness = 2;
            e.Visibility = visibility;
            e.MouseDown += new System.Windows.Input.MouseButtonEventHandler(EllipseClicked);
            e.MouseMove += new System.Windows.Input.MouseEventHandler(MouseMoved);
            e.MouseUp += new System.Windows.Input.MouseButtonEventHandler(EllipseUnclicked);
            return e;
        }

        private void EllipseUnclicked(object sender, MouseButtonEventArgs e)
        {
            isEllipseDragged = false;
        }

        private void MouseMoved(object sender, MouseEventArgs e)
        {
            if (isEllipseDragged == true)
            {
                Ellipse el = (Ellipse)sender;
               
                Point p = e.GetPosition(el);
                double px = el.Margin.Left;
                double py = el.Margin.Top;

                double x = px + p.X;
                double y = py + p.Y;
                double left = x - (el.Width / 2);
                double top = y - (el.Height / 2);
                el.Margin = new Thickness(left, top, 0, 0);
            }
        }

        private void EllipseClicked(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            isEllipseDragged = true;

            Point p = e.GetPosition(ellipse);
            clickOffsetX = (int)p.X;
            clickOffsetY = (int)p.Y;
        }

    }
}