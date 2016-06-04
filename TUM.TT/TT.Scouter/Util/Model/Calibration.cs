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

            foreach (Line l in Lines)
                l.Stroke = System.Windows.Media.Brushes.Black;
            
            Line closestHorizontalLine =new Line();
            Line closestVerticalLine = new Line();

            if (Geometry.LineToPointDistance2D(Lines[0], p, true) < Geometry.LineToPointDistance2D(Lines[2], p, true))
            {
                closestHorizontalLine.X1 = Lines[0].X1;
                closestHorizontalLine.Y1 = Lines[0].Y1;
                closestHorizontalLine.X2 = Lines[0].X2;
                closestHorizontalLine.Y2 = Lines[0].Y2;
            }
            else
            {
                closestHorizontalLine.X1 = Lines[2].X2;
                closestHorizontalLine.Y1 = Lines[2].Y2;
                closestHorizontalLine.X2 = Lines[2].X1;
                closestHorizontalLine.Y2 = Lines[2].Y1;
            }
            if (Geometry.LineToPointDistance2D(Lines[1], p, true) < Geometry.LineToPointDistance2D(Lines[3], p, true))
            {
                closestVerticalLine.X1 = Lines[1].X1;
                closestVerticalLine.Y1 = Lines[1].Y1;
                closestVerticalLine.X2 = Lines[1].X2;
                closestVerticalLine.Y2 = Lines[1].Y2;
            }
            else
            {
                closestVerticalLine.X1 = Lines[3].X2;
                closestVerticalLine.Y1 = Lines[3].Y2;
                closestVerticalLine.X2 = Lines[3].X1;
                closestVerticalLine.Y2 = Lines[3].Y1;
            }

            double lengthHorizontalLine = Geometry.LengthLine(closestHorizontalLine);
            double lengthVerticalLine = Geometry.LengthLine(closestVerticalLine);

            double distanceToStartHorizontal = Geometry.Distance(new Point(closestHorizontalLine.X1, closestHorizontalLine.Y1), Geometry.Project(closestHorizontalLine,p));
            double distanceToStartVertical = Geometry.Distance(new Point(closestVerticalLine.X1, closestVerticalLine.Y1), Geometry.Project(closestVerticalLine,p));

            double percentageX = distanceToStartHorizontal / lengthHorizontalLine;
            double percentageY = distanceToStartVertical / lengthVerticalLine;

            OnStrokePositionCalculated(new Point(percentageX * 152.5, percentageY * 274));
        }

        protected virtual void OnStrokePositionCalculated(Point p)
        {
            if (StrokePositionCalculated != null)
            {
                StrokePositionCalculated(this, new StrokePositionCalculatedEventArgs(p));
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
