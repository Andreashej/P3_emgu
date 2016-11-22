﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.CV.Util;
using Emgu.CV.Structure;

public class blobextraction : MonoBehaviour {

	public RawImage rawimage;
	public VectorOfKeyPoint test;
	public Image<Gray, byte> outputImage;

    public static string imagePath() {
		return string.Format("Assets/Resources/rasmus.png");
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

   /* public static void DrawKeyPoints(
        IInputArray image,
        VectorOfKeyPoint keypoints,
        IInputOutputArray outImage,
        Bgr color,
        Features2DToolbox.KeypointDrawType type
        ) {    
    }*/

    // Use this for initialization
    void Start () {

		
	
		string imgPath = imagePath ();
		Mat srcImage = new Mat(imgPath, 0);

		Image<Gray, byte> blobImage = srcImage.ToImage<Gray, byte>();
        //blobImage = blobImage.Resize(256,256,Emgu.CV.CvEnum.Inter.Cubic);

        // Detect blobs
        vectorThingy = new VectorOfKeyPoint();
		bgrThingy = Bgr (0,0,0);

		Features2DToolbox.DrawKeypoints(blobImage, vectorThingy, outputImage, bgrThingy,4);

        GetComponent<RectTransform>().sizeDelta = new Vector2(blobImage.Width, blobImage.Height);

		rawimage.texture = returnAsTexture(outputImage);
		rawimage.material.mainTexture = returnAsTexture(outputImage);

	}
	
	// Update is called once per frame
	void Update () {



	}
}
