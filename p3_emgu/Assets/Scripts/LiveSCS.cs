using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;
using System.Drawing;
using System.IO;
using System.Text;
using System;
using System.Linq;
using System.Runtime.InteropServices;

public class LiveSCS : MonoBehaviour {
	WebCamTexture webcamTexture;
	public RawImage rawImage;
	public int resWidth = 1600;
	public int resHeight = 900;
	public bool takeHiResShot = false;

	// Use this for initialization
	void Start () {
		webcamTexture = new WebCamTexture();
		rawImage.texture = webcamTexture;
		rawImage.material.mainTexture = webcamTexture;
		webcamTexture.Play();

	}

	// Update is called once per frame
	void Update () {
		
		
	}

	public byte[] ReturnTextureAsBytes (WebCamTexture tex) {
		Texture2D destTexture = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);

		UnityEngine.Color[] textureData = tex.GetPixels();

		destTexture.SetPixels(textureData);
		destTexture.Apply();

		byte[] bytes = destTexture.EncodeToPNG();

		return bytes;
	}

	public Texture2D ReturnAsTexture (Image<Gray, byte> imgInput) {
		Texture2D img = new Texture2D(imgInput.Width, imgInput.Height);
		for (int y = 0; y < img.height; y++) {
			for (int x = 0; x < img.width; x++) {
				UnityEngine.Color color = new UnityEngine.Color((float)imgInput[y, x].Intensity / 256, 
																(float)imgInput[y, x].Intensity / 256, 
																(float)imgInput[y, x].Intensity / 256);
				img.SetPixel(x, y, color);
			}
		}

		img.Apply();
		return img;
	}

	public Texture2D ReturnAsTexture (Image<Rgb, byte> imgInput) {
		Texture2D img = new Texture2D(imgInput.Width, imgInput.Height);
		for (int y = 0; y < img.height; y++) {
			for (int x = 0; x < img.width; x++) {
				UnityEngine.Color color = new UnityEngine.Color((float)imgInput[y, x].Blue / 256,
																(float)imgInput[y, x].Green / 256,
																(float)imgInput[y, x].Red / 256);
				img.SetPixel(x, y, color);
			}
		}

		img.Apply();
		return img;
	}

	public void TakeHiResShot () {
		takeHiResShot = true;
	}

	void LateUpdate () {
		takeHiResShot |= Input.GetKeyDown("k");
		if (takeHiResShot) {

			//byte[] bytes = ReturnTextureAsBytes(webcamTexture);
			Mat bytes = new Mat("Assets/Resources/ivan.jpg", LoadImageType.Color);

			SkinColorSegmentation scs = new SkinColorSegmentation(bytes);

			Image<Gray,byte> segmentedImage = scs.GetSkinRegion();
			
			segmentedImage = segmentedImage.ThresholdBinaryInv(new Gray(150), new Gray(255));

			EdgeDetection ed = new EdgeDetection(segmentedImage);

			ed.DetectEdges();

			BlobDetection blobDetector = new BlobDetection(ed.detectedEdges);

			blobDetector.DetectBlobs();

			Image<Rgb, byte> outputImage = bytes.ToImage<Rgb, byte>(); ;

			MoustachePlacement place = new MoustachePlacement(blobDetector.returnBlobCentres(), blobDetector.returnBlobCentres());
			place.SetMoustacheLocation();
			place.SetYRotation();
			place.SetZRotation();

			Debug.Log("Position: " + place.GetLocation() + " zRotation: " + place.GetZRotation() + " xRotation: " + place.GetXRotation());

			foreach (Point center in blobDetector.returnBlobCentres()) {

				for (int i = 0; i < 5; i++) {
					for (int j = 0; j < 5; j++) {
						outputImage[center.Y - i, center.X - j] = new Rgb(255, 0, 0);
					}
				}
			}

			
			
			GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 180));
			GetComponent<RectTransform>().sizeDelta = new Vector2(outputImage.Width, outputImage.Height);
			Texture2D tex = ReturnAsTexture(outputImage);
			rawImage.texture = tex;
			rawImage.material.mainTexture = tex;

			takeHiResShot = false;
		}

	}

}
