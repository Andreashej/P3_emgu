using UnityEngine;
using System.Collections;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using Emgu.Util;
using Emgu.CV.Features2D;


public class EdgeDetection {
	public Image<Gray, byte> srcImg;
	public Image<Gray,byte> detectedEdges;

	public EdgeDetection (string filename) {
		srcImg = new Image<Gray, byte>("Assets/Resources/Images/" + filename);
		srcImg = srcImg.Resize(256, 256,Inter.Cubic);
	}

	public void DetectEdges() {
		
		//Detect Edges
		detectedEdges = srcImg.Canny(50, 200);
	}

	public Texture2D ReturnAsTexture() {
		Texture2D img = new Texture2D(detectedEdges.Height, detectedEdges.Width);

		for (int y = 0; y < img.height-1; y++) {
			for (int x = 0; x < img.width-1; x++) {
				UnityEngine.Color color = new UnityEngine.Color((int)detectedEdges[x,y].Intensity, (int)detectedEdges[x,y].Intensity, (int)detectedEdges[x,y].Intensity);
				img.SetPixel(x, y, color);
			}
		}
		
		img.Apply();

		return img;
	}

	
}
