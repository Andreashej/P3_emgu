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
	public int resWidth = 1280;
	public int resHeight = 720;
	public bool takeHiResShot = false;
	public static MoustachePlacement place;
	public int firstPress = 0;
	bool webcamActive = true;

	// Use this for initialization
	void Start () {
		webcamTexture = new WebCamTexture(resWidth,resHeight);
		Reset();

	}

	// Update is called once per frame
	void Update () {
		
		
	}

	public byte[] ReturnTextureAsBytes (WebCamTexture tex) {
		Texture2D destTexture = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false);

		UnityEngine.Color[] textureData = tex.GetPixels();

		destTexture.SetPixels(textureData);
		destTexture.Apply();

		byte[] bytes = destTexture.EncodeToJPG();

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
				UnityEngine.Color color = new UnityEngine.Color((float)imgInput[y, x].Red / 256,
																(float)imgInput[y, x].Green / 256,
																(float)imgInput[y, x].Blue / 256);
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

			takePhoto();

			takeHiResShot = false;
		}

	}

	public void takePhoto() {
		//Debug.Log("Webcam Dimensions: " + webcamTexture.width + ", " + webcamTexture.height);
		byte[] bytes = ReturnTextureAsBytes(webcamTexture);
		//Mat bytes = new Mat("Assets/Resources/ivan.jpg", LoadImageType.Color);
		BlobDetection referencePicture = new BlobDetection();
		BlobDetection activePicture = new BlobDetection();
		Image<Rgb, byte> outputImage;

		if (firstPress == 0) {
			referencePicture = TakeReferencePicture(bytes);
			//Debug.Log("firstPress: " + firstPress);
			MemoryStream ms = new MemoryStream(bytes);
			Bitmap bmp = new Bitmap(ms);
			outputImage = new Image<Rgb, byte>(bmp);

			foreach (Point center in referencePicture.returnBlobCentres()) {

				for (int i = 0; i < 5; i++) {
					for (int j = 0; j < 5; j++) {
						outputImage[center.Y - i, center.X - j] = new Rgb(0, 0, 255);
					}
				}
			}
		} else {
			activePicture = TakeReferencePicture(bytes);
			MemoryStream ms = new MemoryStream(bytes);
			Bitmap bmp = new Bitmap(ms);
			outputImage = new Image<Rgb, byte>(bmp);


			if (activePicture.returnBlobCentres().Count == 3) {
				place = new MoustachePlacement(referencePicture.returnBlobCentres(), activePicture.returnBlobCentres());
				place.SetMoustacheLocation();
				place.SetYRotation();
				place.SetZRotation();

				Debug.Log("Position: " + place.GetLocation() + " zRotation: " + place.GetZRotation() + " xRotation: " + place.GetXRotation());
			}

			foreach (Point center in activePicture.returnBlobCentres()) {

				for (int i = 0; i < 5; i++) {
					for (int j = 0; j < 5; j++) {
						outputImage[center.Y - i, center.X - j] = new Rgb(0, 0, 255);
					}
				}
			}

			if (firstPress == 1) GetComponent<RectTransform>().Rotate(new Vector3(0, 180, 180));
			if (firstPress > 0) {
				GetComponent<RectTransform>().sizeDelta = new Vector2(outputImage.Width, outputImage.Height);
				Texture2D tex = ReturnAsTexture(outputImage);
				rawImage.texture = tex;
				rawImage.material.mainTexture = tex;
				webcamActive = false;
			}			
		}
		firstPress++;
		outputImage.Save(PhotoName());

	}
	
	public void Reset() {
		if (!webcamActive) {
			GetComponent<RectTransform>().Rotate(new Vector3(0, 180, 180));
			firstPress = 1;
		}
		rawImage.texture = webcamTexture;
		rawImage.material.mainTexture = webcamTexture;
		webcamTexture.Play();
		webcamActive = true;
	}

	public BlobDetection TakeReferencePicture(byte[] bytes) {
		SkinColorSegmentation scs = new SkinColorSegmentation(bytes);

		Image<Gray,byte> segmentedImage = scs.GetSkinRegion();

		segmentedImage = segmentedImage.ThresholdBinaryInv(new Gray(150), new Gray(255));

		EdgeDetection ed = new EdgeDetection(segmentedImage);

		ed.DetectEdges();

		BlobDetection blobDetector = new BlobDetection(ed.detectedEdges);

		blobDetector.DetectBlobs();

		return blobDetector;
	}

	public string PhotoName () {
		return string.Format("{0}/Resources/Evaluation/photo_{1}.png",
			Application.dataPath,
			DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss"));
	}

}
