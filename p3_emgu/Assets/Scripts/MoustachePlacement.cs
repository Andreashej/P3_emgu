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
            return reference.GetEyeToBase() < workingImage.GetEyeToBase(); //if true, the rotation is positive (counterclockwise)
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
