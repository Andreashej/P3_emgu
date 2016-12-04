using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.CV.Util;
using AForge;

public class blobextraction : MonoBehaviour {

	public RawImage rawimage;
	public Image<Gray, byte> outputImage, blobImage;

    public static string imagePath() {
		return string.Format("Assets/Resources/test2.png");
	}

	public Texture2D returnAsTexture (Image<Gray, byte> imgInput) {
		Texture2D img = new Texture2D(imgInput.Width, imgInput.Height);
		for (int y = 0; y < img.height; y++) {
			for (int x = 0; x < img.width; x++) {
				Color color = new Color((float)imgInput[y,x].Intensity/256, (float)imgInput[y,x].Intensity/256, (float)imgInput[y,x].Intensity/256);
				img.SetPixel(x, y, color);
			}
		}
		
		img.Apply();
		return img;
	}

    // Use this for initialization
    void Start () {
	
		//Read image
		string imgPath = imagePath();
		blobImage = new Image<Gray, byte>(imgPath);
		outputImage = blobImage.Clone();

		////Set keypoiny colour
		//Bgr keypointColour = new Bgr (0,0,255);
		
		////Create the new blob detector
		//SimpleBlobDetector blobs = new SimpleBlobDetector();

		////Detect blobs in image and assign to keypoint vector
		//VectorOfKeyPoint keypointVector = new VectorOfKeyPoint(blobs.Detect(blobImage));

		////Draw the keypoints to outputImage
		//Features2DToolbox.DrawKeypoints(outputImage, keypointVector, outputImage, keypointColour, Features2DToolbox.KeypointDrawType.Default); //I think the problem is that nothing is drawn on the outputimage here...
		
		////Apply the blob detected image to texture
        GetComponent<RectTransform>().sizeDelta = new Vector2(blobImage.Width, blobImage.Height);

		rawimage.texture = returnAsTexture(outputImage);
		rawimage.material.mainTexture = returnAsTexture(outputImage);

	}
	
	// Update is called once per frame
	void Update () {



	}
}
