using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using UnityEngine;


namespace Assets.Scripts
{
    class Triangle
    {
        Point leftEye, rightEye, mouth;
        double area;
        
        public Triangle(List<Point> blobs)
        {
			leftEye = blobs[0];
			rightEye = blobs[1];
			mouth = blobs[2];
        }
        
        public double PointDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2));
        }

        public Point GetDirectionFromAToB(Point a, Point b)
        {
            return new Point((b.X - a.X), (b.Y - a.Y));
        }
        
        public void SetAreaHeron()
        {
            double a = PointDistance(leftEye, rightEye);
            double b = PointDistance(leftEye, mouth);
            double c = PointDistance(rightEye, mouth);
            double s = (a + b + c) / 2;

            area = Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }

        public double GetArea()
        {
            return area;
        }
        
        public Point GetHeightBase()
        {
            double a1 = rightEye.Y - leftEye.Y;
            double b1 = leftEye.X - rightEye.X;
            double c1 = a1 * leftEye.X + b1 * leftEye.Y;

            Point normalVector = new Point(rightEye.X - leftEye.X, rightEye.Y - leftEye.Y);
            Point dirVector = new Point(normalVector.Y * (-1), normalVector.X);
            Point mouthPlusDir = new Point(mouth.X + dirVector.X, mouth.Y + dirVector.Y);

            double a2 = mouthPlusDir.Y - mouth.Y;
            double b2 = mouth.X - mouthPlusDir.X;
            double c2 = a2 * mouth.X + b2 * mouth.Y;

            double det = a1 * b2 - a2 * b1;
            if (det == 0)
            {
                return new Point(0, 0); // points are parallel
            }
            else
            {
                double x = (b2 * c1 - b1 * c2) / det;
                double y = (a1 * c2 - a2 * c1) / det;
                return new Point((int)x, (int)y);
            }
        }

        public double[] GetEyeToBase()
        {
            Point HeightBase = GetHeightBase();
            double leftToBase = PointDistance(leftEye, HeightBase);
            double baseToRight = PointDistance(HeightBase, rightEye);
            double[] result = { leftToBase, baseToRight };
            return result;
        }

        public double GetTangent()
        {
            return ((rightEye.Y - leftEye.Y) / (rightEye.X - leftEye.X));
        }

		public Point GetMouth () {
			return mouth;
		}

		public Point[] GetEyes () {
			Point[] eyes = { leftEye, rightEye};
			return eyes;
		}
    }
}
