using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Assets.Scripts
{
    public class MoustachePlacement
    {
        Triangle reference, workingImage;
        double zRotation, xRotation; //it's in radians
		Point moustacheLocation;

        public MoustachePlacement(List<Point> referencePoints, List<Point> workingImagePoints)
        {
			reference = new Triangle(referencePoints);
			workingImage = new Triangle(workingImagePoints);
        }
        
        public bool GetOrientation()
        {
            double[] referenceDistances = reference.GetEyeToBase();
            double[] workingDistances = workingImage.GetEyeToBase();

            if (referenceDistances[0] > workingDistances[0]) return false;
            else return true;

        }


        public void SetXRotation()
        {
            double refYRot = Math.Atan(reference.GetTangent());
            double workYRot = Math.Atan(workingImage.GetTangent());
            xRotation = RadToDeg(workYRot - refYRot);
        }

		public void SetXRotationNoReference () {
			double workYRot = Math.Atan(workingImage.GetTangent());
			xRotation = RadToDeg(workYRot);
		}

        public void SetZRotation()
        {
			double a = workingImage.GetArea();
			double b = reference.GetArea();
			double rot = Math.Acos(a/b);

			if (GetOrientation())
            {

				zRotation = RadToDeg(rot); // if it doesn't work, change workingImage with reference
            }
            else
            {
                zRotation = RadToDeg(rot*(-1));
            }
			Debug.Log(zRotation);
        }

		public void SetMoustacheLocation() 
		{
			Point mouth = workingImage.GetMouth();
			Point[] eyes = workingImage.GetEyes();
			Point eyeMiddle = new Point((eyes[0].X+eyes[1].X)/2,(eyes[0].Y+eyes[1].Y)/2);
			Point dirVector = new Point(eyeMiddle.X-mouth.X, eyeMiddle.Y-mouth.Y);
			moustacheLocation = new Point(1280-(mouth.X+dirVector.X/4), 720-(mouth.Y+dirVector.Y/4));
			
		}


		public double RadToDeg (double rad) {
			return rad * 180 / Math.PI;
		}

		public double GetXRotation() {
			return xRotation;
		}

		public double GetZRotation() {
			return zRotation;
		}

		public Point GetLocation() {
			return moustacheLocation;
		}

		public Point GetEyeLocation () {
			return new Point(1280-workingImage.GetEyes()[1].X, 720-workingImage.GetEyes()[1].Y);
		}
	}
}
