using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Assets.Scripts
{
    class MoustachePlacement
    {
        Triangle reference, workingImage;
        double zRotation, xRotation; //it's in radians


        public MoustachePlacement()
        {

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
    }
}
