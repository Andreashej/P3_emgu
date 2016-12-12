using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace Assets.Scripts
{
    class SkinColorSegmentation
    {
        public Mat source = new Mat();
        Mat result;

        //Constructor that gets a Mat
        public SkinColorSegmentation(Mat src)
        {
            source = src;
        }

        //Constructor that gets a byte array
		public SkinColorSegmentation(byte[] byteImg) {
			MemoryStream ms = new MemoryStream(byteImg);
			Bitmap bmp = new Bitmap(ms);
			Image<Bgr, byte> img = new Image<Bgr, byte>(bmp);
			source = img.Mat;
		}

        //The RGB rule, it gets the R,G,B components of a pixel to work with.
        private bool RuleRGB(double R, double G, double B)
        {
            bool daylightRule = (R > 95) && (G > 20) && (Math.Max(R, Math.Max(G, B)) - Math.Min(R, Math.Min(G, B)) > 15) && (Math.Abs(R - G) > 15) && (R > G) && (R > B);
            bool flashlightRule = (R > 220) && (G > 210) && (B > 170) && (Math.Abs(R - G) <= 15) && (R > B) && (G > B);

            return daylightRule || flashlightRule;
        }

        //YCbCr rule, it thets the Y,Cb,Cr values of a pixel to work with.
        private bool RuleYCbCr(double Y, double Cb, double Cr)
        {
            bool thresholdOne = Cr <= 1.5862 * Cb + 20;
            bool thresholdTwo = Cr >= 0.3448 * Cb + 76.2069;
            bool thresholdThree = Cr >= -4.5652 * Cb + 234.5652;
            bool thresholdFour = Cr <= -1.15 * Cb + 301.75;
            bool thresholdFive = Cr <= -2.2857 * Cb + 432.85;

            return thresholdOne && thresholdTwo && thresholdThree && thresholdFour && thresholdFive;

        }

        //HSV rule, it gets the H,S,V values of a pixel.
        private bool RuleHSV(double H, double S, double V)
        {
            return (H < 25) || (H > 230);
        }

        //This algorithm creates the YCbCr and HSV copies of the pictures, then applies the rules to them.
        public Image<Gray,Byte> GetSkinRegion()
        {
            result = source.Clone();
            Image<Gray, Byte> resultImage = result.ToImage<Gray, Byte>();
            Mat srcYCbCr = new Mat();
            Mat srcHSV = new Mat();

            CvInvoke.CvtColor(source, srcYCbCr, ColorConversion.Bgr2YCrCb);

            //The hue value has to be scaled from 0 to 255
            source.ConvertTo(srcHSV, DepthType.Cv32F);
            CvInvoke.CvtColor(srcHSV, srcHSV, ColorConversion.Bgr2Hsv);
            CvInvoke.Normalize(srcHSV, srcHSV, 0.0, 255.0, NormType.MinMax, DepthType.Cv32F);

            //Image copies from Mat so EmguCV can work with them
            Image<Bgr, Byte> srcImage = source.ToImage<Bgr,Byte>();
            Image<Ycc, Byte> srcYCbCrImage = srcYCbCr.ToImage<Ycc, Byte>();
            Image<Hsv, Single> srcHSVImage = srcHSV.ToImage<Hsv, Single>();

            //Nested for loop goes through every pixel of the images and applies the rules to determine skin region
            for(int i = 0; i < source.Rows; i++)
            {
                for(int j = 0; j < source.Cols; j++)
                {
                    double R = srcImage[i, j].Red;
                    double G = srcImage[i, j].Green;
                    double B = srcImage[i, j].Blue;

                    bool rgb = RuleRGB(R, G, B);

                    double Y = srcYCbCrImage[i, j].Y;
                    double Cb = srcYCbCrImage[i, j].Cb;
                    double Cr = srcYCbCrImage[i, j].Cr;

                    bool yCbCr = RuleYCbCr(Y, Cb, Cr);

                    double H = srcHSVImage[i, j].Hue;
                    double S = srcHSVImage[i, j].Satuation;
                    double V = srcHSVImage[i, j].Value;

                    bool hsv = RuleHSV(H, S, V);

                    //If not skin region -> black pixel, if skin region -> white pixel
                    if ( !(rgb && yCbCr && hsv))
                    {
                        resultImage[i, j] = new Gray(0);
                    }
                    else
                    {
						resultImage[i, j] = new Gray(255);
                    }
                }
            }

            return resultImage;
        } 
    }
}
