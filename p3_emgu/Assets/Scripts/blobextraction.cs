using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Emgu.CV;
using Emgu.CV.Structure;

public class blobextraction : MonoBehaviour {

	public RawImage rawimage;
	public static string imagePath() {
		return string.Format("{0}/Resources/photo.png", 
			Application.dataPath);
	}

	public Texture2D returnAsTexture (Image<Gray, byte> imgInput) {
		Texture2D img = new Texture2D(imgInput.Height, imgInput.Width);
		for (int y = 0; y < img.height - 1; y++) {
			for (int x = 0; x < img.width - 1; x++) {
				UnityEngine.Color color = new UnityEngine.Color((int)imgInput[x,y].Intensity, (int)imgInput[x,y].Intensity, (int)imgInput[x,y].Intensity);
				img.SetPixel(x, y, color);
			}
		}

		img.Apply();
		return img;
	}

	// Use this for initialization
	void Start () {
	
		string imgPath = imagePath ();
		Mat srcImage = new Mat(imgPath, 0);

		Image<Gray, byte> blobImage = srcImage.ToImage<Gray, byte>();

		rawimage.texture = returnAsTexture(blobImage);
		rawimage.material.mainTexture = returnAsTexture(blobImage);

	}
	
	// Update is called once per frame
	void Update () {



	}
}
