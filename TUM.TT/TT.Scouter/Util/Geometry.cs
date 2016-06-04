using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace TT.Scouter.Util
{
    static class Geometry
    {
        public static Point Project(Line l, Point toProject)
        {
            Point pointA = new Point(l.X1, l.Y1);
            Point pointB = new Point(l.X2, l.Y2);

            double m = (double)(pointB.Y - pointA.Y) / (pointB.X - pointA.X);
            double b = (double)pointA.Y - (m * pointA.X);

            double x = (m * toProject.Y + toProject.X - m * b) / (m * m + 1);
            double y = (m * m * toProject.Y + m * toProject.X + b) / (m * m + 1);

            return new Point((int)x, (int)y);
        }

        //Compute the distance from AB to C
        //if isSegment is true, AB is a segment, not a line.
        public static double LineToPointDistance2D(Line l, Point p,
            bool isSegment)
        {
            double[] pointA = new double[] { l.X1, l.Y1 };
            double[] pointB = new double[] { l.X2, l.Y2 };
            double[] pointC = new double[] { p.X, p.Y };
            double dist = CrossProduct(pointA, pointB, pointC) / Distance(pointA, pointB);
            if (isSegment)
            {
                double dot1 = DotProduct(pointA, pointB, pointC);
                if (dot1 > 0)
                    return Distance(pointB, pointC);

                double dot2 = DotProduct(pointB, pointA, pointC);
                if (dot2 > 0)
                    return Distance(pointA, pointC);
            }
            return Math.Abs(dist);
        }

        //Compute the distance from A to B
        public static double Distance(double[] pointA, double[] pointB)
        {
            double d1 = pointA[0] - pointB[0];
            double d2 = pointA[1] - pointB[1];

            return Math.Sqrt(d1 * d1 + d2 * d2);
        }

        public static double Distance(Point pointA, Point pointB)
        {
            return Distance(new double[] { pointA.X, pointA.Y }, new double[] { pointB.X, pointB.Y });
        }

        public static double LengthLine(Line line)
        {
            return Geometry.Distance(new Point(line.X1, line.Y1), new Point(line.X2, line.Y2));
        }

        //Compute the dot product AB . AC
        private static double DotProduct(double[] pointA, double[] pointB, double[] pointC)
        {
            double[] AB = new double[2];
            double[] BC = new double[2];
            AB[0] = pointB[0] - pointA[0];
            AB[1] = pointB[1] - pointA[1];
            BC[0] = pointC[0] - pointB[0];
            BC[1] = pointC[1] - pointB[1];
            double dot = AB[0] * BC[0] + AB[1] * BC[1];

            return dot;
        }

        //Compute the cross product AB x AC
        private static double CrossProduct(double[] pointA, double[] pointB, double[] pointC)
        {
            double[] AB = new double[2];
            double[] AC = new double[2];
            AB[0] = pointB[0] - pointA[0];
            AB[1] = pointB[1] - pointA[1];
            AC[0] = pointC[0] - pointA[0];
            AC[1] = pointC[1] - pointA[1];
            double cross = AB[0] * AC[1] - AB[1] * AC[0];

            return cross;
        }
    }
}
