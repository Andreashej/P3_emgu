using UnityEngine;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;


public class EdgeDetection {
	public Image<Gray, byte> srcImg;
	public Image<Gray,byte> detectedEdges;

	public EdgeDetection (string filename) {
		srcImg = new Image<Gray, byte>("Assets/Resources/Images/" + filename);
		//srcImg = srcImg.Resize(256, 256,Inter.Cubic);
	}

	public void DetectEdges() {
		//Gaussianblur
		detectedEdges = srcImg.SmoothGaussian(5);

		//Detect Edges
		detectedEdges = detectedEdges.Canny(30, 150);
	}

	public Texture2D ReturnAsTexture() {
		Texture2D img = new Texture2D(detectedEdges.Height, detectedEdges.Width);

		for (int y = 0; y < img.height-1; y++) {
			for (int x = 0; x < img.width-1; x++) {
				Color color = new Color((int)detectedEdges[x,y].Intensity, (int)detectedEdges[x,y].Intensity, (int)detectedEdges[x,y].Intensity);
				img.SetPixel(x, y, color);
			}
		}
		
		img.Apply();

		return img;
	}

	
}
