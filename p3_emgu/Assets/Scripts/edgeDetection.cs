using UnityEngine;
using System.Collections;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;


public class edgeDetection {
	public Mat src;
	public Mat detectedEdges;
	public Mat dst;

	public edgeDetection (string filename) {
		src = CvInvoke.Imread("../Resources/img/" + filename, LoadImageType.Grayscale);
		dst.Create(src.Height, src.Width, src.Depth, src.NumberOfChannels);
	}

	public void detectEdges() {
		//Perform Gaussian-blur
		CvInvoke.GaussianBlur(src, detectedEdges, new Size(3, 3), 50);

		//Detect Edges
		CvInvoke.Canny(detectedEdges, dst, 50, 200);
	}

	public Texture2D returnAsTexture() {
		Texture2D img = new Texture2D(dst.Height, dst.Width);

		Image<Gray, byte> image = dst.ToImage<Gray, byte>();
		Bitmap bmp = image.ToBitmap();

		for(int y = 0; y < img.height; y++) {
			for(int x = 0; x < img.width; x++) {
				UnityEngine.Color color = new UnityEngine.Color(bmp.GetPixel(x,y).R, bmp.GetPixel(x,y).G, bmp.GetPixel(x,y).B);
				img.SetPixel(x, y, color);
			}
		}
		img.Apply();

		return img;
	}

	
}
