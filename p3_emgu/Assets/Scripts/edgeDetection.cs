using UnityEngine;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

public class EdgeDetection {
	public Image<Gray, byte> srcImg;
	public Image<Gray,byte> detectedEdges;

	// Constructor for loading a static image
	public EdgeDetection (string filename) {
		srcImg = new Image<Gray, byte>("Assets/Resources/Images/" + filename);
	}
	
	//Constructor that passes on a grayscale image from skincolor segmentation	
	public EdgeDetection (Image<Gray, byte> img) {
		srcImg = img;
	}

	public void DetectEdges () {
		
		// Add a Gaussian blur
		detectedEdges = srcImg.SmoothGaussian(15);
		
		// Detect Edges using the Canny method
		detectedEdges = detectedEdges.Canny(10, 230);
		
		//Structuring element for closing operation
		Mat element = CvInvoke.GetStructuringElement(ElementShape.Cross, new System.Drawing.Size(3,3), new System.Drawing.Point(-1,-1));
		MCvScalar scalar = new MCvScalar(255); //Tested with value 20, but should probably be 255, since it's the pixel value.
		
		//Performs closing operation on the edge detected image.
		detectedEdges = detectedEdges.MorphologyEx(MorphOp.Close, element, new System.Drawing.Point(-1, -1), 12, BorderType.Replicate, scalar);
	}

	public float getHeight() {
		return srcImg.Height;
	}

	public float getWidth() {
		return srcImg.Width;
	}

}
