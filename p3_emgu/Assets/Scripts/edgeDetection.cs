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

	public EdgeDetection (Image<Gray, byte> img) {
		srcImg = img;
	}

	public void DetectEdges () {
		//Gaussianblur
		detectedEdges = srcImg.SmoothGaussian(15);
		
		//Detect Edges
		detectedEdges = detectedEdges.Canny(10, 230);
		
		Mat element = CvInvoke.GetStructuringElement(ElementShape.Cross, new System.Drawing.Size(3,3), new System.Drawing.Point(-1,-1));
		MCvScalar scalar = new MCvScalar(20);
		
		detectedEdges = detectedEdges.MorphologyEx(MorphOp.Close, element, new System.Drawing.Point(-1, -1), 15, BorderType.Default, scalar);

		//detectedEdges = detectedEdges.Dilate(5);
		//detectedEdges = detectedEdges.Erode(3);
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


}
