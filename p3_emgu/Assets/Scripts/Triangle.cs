using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Assets.Scripts
{
    class Triangle
    {
        Point leftEye, rightEye, mouth;
        double eyeToBase;
        double area;
        
        public Triangle()
        {

        }
        
        public double PointDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2));
        }
        
        public void SetAreaHeron()
        {
            double a = PointDistance(leftEye, rightEye);
            double b = PointDistance(leftEye, mouth);
            double c = PointDistance(rightEye, mouth);
            double s = (a + b + c) / 2;

            area = Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }

        public void SetEyeToBase()
        {
            eyeToBase = mouth.X - leftEye.X;
        }

        public double GetArea()
        {
            return area;
        }

        public double GetEyeToBase()
        {
            return eyeToBase;
        }
    }
}
