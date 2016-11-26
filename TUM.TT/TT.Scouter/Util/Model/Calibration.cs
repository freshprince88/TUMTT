using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using TT.Lib.Events;

namespace TT.Scouter.Util.Model
{
    public class Calibration
    {
        // 1. Point: Topleft; 2. Point: TopRight; 3. Point: BottomRight; 4. Point: BottomLeft;
        public List<Point> Points  {get; }
        public ObservableCollection<Line> Lines { get; }

        public bool isCalibrated { get; private set; }
        public bool isCalibrating { get; private set; }

        public delegate void StrokePositionCalculatedEventHandler(object source, StrokePositionCalculatedEventArgs args);

        public event StrokePositionCalculatedEventHandler StrokePositionCalculated;

        public delegate void PointAddedEventHandler(object source, PointAddedEventArgs args);

        public event PointAddedEventHandler PointAdded;


        public Calibration()
        {
            isCalibrated = false;
            isCalibrating = false;
            Points = new List<Point>();
            Lines = new ObservableCollection<Line>();
        }

        public void startCalibrating()
        {
            isCalibrated = false;
            Points.Clear();
            Lines.Clear();
            isCalibrating = true;


        }

        public void AddPoint(Point p)
        {
            Points.Add(p);

            OnPointAdded(Points.Count);

            if (Points.Count < 2)
                return;


            Point p2 = Points[Points.Count - 2];

            Line newLine = createLine(p2, p);
            Lines.Add(newLine);

            if (Points.Count == 4)
            {
                Line endLine = createLine(p, Points[0]);

                // finish Calibrating
                isCalibrating = false;
                isCalibrated = true;
                Lines.Add(endLine);
            }
        }

        public void calcPointPositionOnTable(Point p)
        {

            if (!IsPointInPolygon(p))
                return;

            Console.WriteLine(p);

            Point a = Points[0];
            Point b = Points[1];
            Point c = Points[2];
            Point d = Points[3];

            double C = (double)(a.Y - p.Y) * (d.X - p.X) - (double)(a.X - p.X) * (d.Y - p.Y);
            double B = (double)(a.Y - p.Y) * (c.X - d.X) + (double)(b.Y - a.Y) * (d.X - p.X) - (double)(a.X - p.X) * (c.Y - d.Y) - (double)(b.X - a.X) * (d.Y - p.Y);
            double A = (double)(b.Y - a.Y) * (c.X - d.X) - (double)(b.X - a.X) * (c.Y - d.Y);

            double D = B * B - 4 * A * C;

            double u = (-B - Math.Sqrt(D)) / (2 * A);

            double p1x = a.X + (b.X - a.X) * u;
            double p2x = d.X + (c.X - d.X) * u;
            double px = p.X;

            double v = (px - p1x) / (p2x - p1x);

            OnStrokePositionCalculated(new Point(u * 152.5, v * 274));
        }

        public void reverseCalcPointPositionOnTable(Point p)
        {
            if(p == null)
            {
                return;
            }

            double u = p.X / 152.5;
            double v = p.Y / 274;

            Point a = Points[0];
            Point b = Points[1];
            Point c = Points[2];
            Point d = Points[3];

            double p1x = a.X + (b.X - a.X) * u;
            double p2x = d.X + (c.X - d.X) * u;

            double px = v * (p2x - p1x) + p1x;
            double py = (a.X * c.Y * Math.Pow(u, 2.0) - a.X * c.Y * u - a.X * d.Y * Math.Pow(u, 2.0) + 2.0 * a.X * d.Y * u - a.X * d.Y - b.X * c.Y * Math.Pow(u, 2.0) + b.X * d.Y * Math.Pow(u, 2.0) - b.X * d.Y * u - c.X * a.Y * Math.Pow(u, 2.0) + c.X * a.Y * u + c.X * b.Y * Math.Pow(u, 2.0) + d.X * a.Y * Math.Pow(u, 2.0) - 2.0 * d.X * a.Y * u + d.X * a.Y - d.X * b.Y * Math.Pow(u, 2.0) + d.X * b.Y * u + a.Y * u * p.X - a.Y * p.X - b.Y * u * p.X + c.Y * u * p.X - d.Y * u * p.X + d.Y * p.X) / (a.X * u - a.X - b.X * u + c.X * u - d.X * u + d.X);

            Point x = new Point(px, py);
            Console.WriteLine(x);
        }

        protected virtual void OnStrokePositionCalculated(Point p)
        {
            if (StrokePositionCalculated != null)
            {
                StrokePositionCalculated(this, new StrokePositionCalculatedEventArgs(p));
            }
        }

        protected virtual void OnPointAdded(int numberOfPoints)
        {
            if (PointAdded != null)
            {
                PointAdded(this, new PointAddedEventArgs(numberOfPoints));
            }
        }

        private Line createLine(Point p1, Point p2)
        {
            Line l = new Line();
            l.X1 = p1.X;
            l.Y1 = p1.Y;
            l.X2 = p2.X;
            l.Y2 = p2.Y;
            l.Stroke = System.Windows.Media.Brushes.Black;
            l.StrokeThickness = 4;

            return l;
        }

        // Source: https://social.msdn.microsoft.com/Forums/windows/en-US/95055cdc-60f8-4c22-8270-ab5f9870270a/determine-if-the-point-is-in-the-polygon-c?forum=winforms
        private bool IsPointInPolygon(Point p)
        {
            Point p1, p2;

            bool inside = false;

            if (Points.Count != 4)
            {
                throw new ArgumentNullException("Polygon does not have 4 Points but " + Points.Count);
            }

            Point oldPoint = new Point(Points[Points.Count - 1].X, Points[Points.Count - 1].Y);

            for (int i = 0; i < Points.Count; i++)
            {
                Point newPoint = new Point(Points[i].X, Points[i].Y);

                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.X < p.X) == (p.X <= oldPoint.X)
                && ((long)p.Y - (long)p1.Y) * (long)(p2.X - p1.X)
                 < ((long)p2.Y - (long)p1.Y) * (long)(p.X - p1.X))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }

    }
}
