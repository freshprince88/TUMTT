using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace TT.Scouter.Util.Model
{
    class Calibration
    {
        // 1. Point: Topleft; 2. Point: TopRight; 3. Point: BottomRight; 4. Point: BottomLeft;
        public List<Point> Points  {get; }
        public ObservableCollection<System.Windows.Shapes.Line> Lines { get; }

        public bool isCalibrated { get; private set; }
        public bool isCalibrating { get; private set; }

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

            Line newLine = createLine(p, p2);
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

        // Source: https://social.msdn.microsoft.com/Forums/windows/en-US/95055cdc-60f8-4c22-8270-ab5f9870270a/determine-if-the-point-is-in-the-polygon-c?forum=winforms
        public bool IsPointInPolygon(Point p)
        {
            Point p1, p2;

            bool inside = false;

            if (Points.Count != 4)
            {
                throw new ArgumentNullException("Polygon does not have 4 Points but " + Points.Count );
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

        internal Point getPointPositionToTable(Point p)
        {

            foreach (Line l in Lines)
                l.Stroke = System.Windows.Media.Brushes.Black;

            //throw new NotImplementedException();
            Line closestHorizontalLine;
            Line closestVerticalLine;

            if (Geometry.LineToPointDistance2D(Lines[0], p, true) < Geometry.LineToPointDistance2D(Lines[2], p, true))
                closestHorizontalLine = Lines[0];
            else
                closestHorizontalLine = Lines[2];

            if (Geometry.LineToPointDistance2D(Lines[1], p, true) < Geometry.LineToPointDistance2D(Lines[3], p, true))
                closestVerticalLine = Lines[1];
            else
                closestVerticalLine = Lines[3];

            closestHorizontalLine.Stroke = System.Windows.Media.Brushes.Red;
            closestVerticalLine.Stroke = System.Windows.Media.Brushes.Red;

            return Geometry.Project(closestHorizontalLine, p);
        }
    }
}
