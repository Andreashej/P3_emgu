using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Assets.Scripts
{
    public class MoustachePlacement
    {
        Triangle workingImage;
        double zRotation, xRotation;
		Point moustacheLocation;

        public MoustachePlacement(List<Point> referencePoints, List<Point> workingImagePoints)
        {
			workingImage = new Triangle(workingImagePoints);
        }

		public void SetXRotationNoReference () {
			double workYRot = Math.Atan(workingImage.GetTangent());
			xRotation = RadToDeg(workYRot);
		}
			
		public void SetMoustacheLocation() 
		{
			Point mouth = workingImage.GetMouth();
			Point[] eyes = workingImage.GetEyes();
			Point eyeMiddle = new Point((eyes[0].X+eyes[1].X)/2,(eyes[0].Y+eyes[1].Y)/2);
			Point dirVector = new Point(eyeMiddle.X-mouth.X, eyeMiddle.Y-mouth.Y);
			moustacheLocation = new Point(1280-(mouth.X+dirVector.X/4), 720-(mouth.Y+dirVector.Y/4));
		}

		// Changing radians to degrees
		public double RadToDeg (double rad) {
			return rad * 180 / Math.PI;
		}

		public double GetXRotation() {
			return xRotation;
		}

		public Point GetLocation() {
			return moustacheLocation;
		}

		public Point GetEyeLocation () {
			return new Point(1280-workingImage.GetEyes()[1].X, 720-workingImage.GetEyes()[1].Y);
		}
	}
}
