using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public void SetYRotation()
        {
            double refYRot = Math.Atan(reference.GetTangent());
            double workYRot = Math.Atan(workingImage.GetTangent());
            zRotation = workYRot - refYRot;
        }

        public void SetZRotation()
        {
            if (GetOrientation())
            {
                zRotation = Math.Acos(workingImage.GetArea() / reference.GetArea()); // if it doesn't work, change workingImage with reference
            }
            else
            {
                zRotation = Math.Acos(workingImage.GetArea() / reference.GetArea())*(-1);
            }
        }

		public void SetMoustacheLocation() 
		{
			Point mouth = workingImage.GetMouth();
			Point[] eyes = workingImage.GetEyes();
			Point eyeMiddle = new Point((eyes[0].X+eyes[1].X)/2, (eyes[0].Y+eyes[1].Y)/2);
			Point dirVector = new Point(eyeMiddle.X-mouth.X, eyeMiddle.Y-mouth.Y);
			moustacheLocation = new Point(1280-mouth.X, (dirVector.Y + (mouth.Y / 4)));
			Debug.Log(moustacheLocation);
			
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
    }
}
