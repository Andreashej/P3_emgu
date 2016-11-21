using UnityEngine;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;


public class EdgeDetection {
	public Image<Gray, byte> srcImg;
	public Image<Gray,byte> detectedEdges;

	public EdgeDetection (string filename) {
		srcImg = new Image<Gray, byte>("Assets/Resources/Images/" + filename);
	}

	public void DetectEdges () {
		//Gaussianblur
		detectedEdges = srcImg.SmoothGaussian(15);

		//detectedEdges = detectedEdges.ThresholdBinaryInv(new Gray(120), new Gray(255));

		//Detect Edges
		detectedEdges = detectedEdges.Canny(10, 40);
	}

	public float getHeight() {
		return srcImg.Height;
	}

	public float getWidth() {
		return srcImg.Width;
	}

	public Texture2D ReturnAsTexture() {
		Texture2D img = new Texture2D(detectedEdges.Width, detectedEdges.Height);

		for (int y = 0; y < img.height; y++) {
			for (int x = 0; x < img.width; x++) {
				Color color = new Color((float)detectedEdges[y,x].Intensity/256, (float)detectedEdges[y,x].Intensity/256, (float)detectedEdges[y,x].Intensity/256);
				img.SetPixel(x, y , color);
			}
		}
		
		img.Apply();

		return img;
	}

	public Image<Gray, byte> returnToRonny() {
		return detectedEdges;
	} 

}
