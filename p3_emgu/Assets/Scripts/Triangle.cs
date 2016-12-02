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

        public void SetEyeToBase()
        {
            eyeToBase = mouth.X - leftEye.X;
        }

        public double GetArea()
        {
            return area;
        }

        public Point GetHeightBase()
        {
            Point directionPoint = GetDirectionFromAToB(leftEye, rightEye);
            double[] directionVector = { (directionPoint.X) / 100, (directionPoint.Y) / 100 };
            double[] normalVector = {directionVector[1]*(-1), directionVector[0]};

            for(int i = 0; i<100; i++)
            {
                for(int j = 0; j<100; j++)
                {
                    if(leftEye.X+i*directionVector[0] == mouth.X+j*normalVector[0] && leftEye.Y+i*directionVector[1] == mouth.Y + j * normalVector[1])
                    {

                    }
                }
            }
        }

        public double GetEyeToBase()
        {
            return eyeToBase;
        }
    }
}
