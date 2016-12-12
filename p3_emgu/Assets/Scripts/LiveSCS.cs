using Assets.Scripts;
using System;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

public class LiveSCS : MonoBehaviour {
	WebCamTexture webcamTexture;
	public RawImage rawImage;
	public int resWidth, resHeight;
	public static MoustachePlacement place;
	bool webcamActive = true;
	int firstPress = 0;
	
	BlobDetection referencePicture = new BlobDetection();
	BlobDetection activePicture = new BlobDetection();

	// Initialization
	void Start () {
		webcamTexture = new WebCamTexture(resWidth,resHeight);
		Reset();
	}

	// Function for changing a Unity Texture2D to a byte type
	public byte[] ReturnTextureAsBytes (WebCamTexture tex) {
		Texture2D destTexture = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false);

		UnityEngine.Color[] textureData = tex.GetPixels();

		destTexture.SetPixels(textureData);
		destTexture.Apply();

		byte[] bytes = destTexture.EncodeToJPG();

		return bytes;
	}

	// Function for changing a greyscale image to a Unity Texture2D type
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

	// Function for changing a RGB image to a Unity Texture2D type
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

	public void takePhoto() {
		byte[] bytes = ReturnTextureAsBytes(webcamTexture);
		Image<Rgb, byte> outputImage;

		activePicture = TakePicture(bytes);
		MemoryStream memoryStream = new MemoryStream(bytes);
		Bitmap bmp = new Bitmap(memoryStream);
		outputImage = new Image<Rgb, byte>(bmp);

		// Write to console the amount of BLOBs found
		Debug.Log(activePicture.returnBlobCentres().Count);

		if (activePicture.returnBlobCentres().Count == 3) {
			place = new MoustachePlacement(activePicture.returnBlobCentres(), activePicture.returnBlobCentres());
			place.SetMoustacheLocation();
			place.SetXRotationNoReference();

			// Write to console the location and rotation of the BLOBs
			Debug.Log("Position: " + place.GetLocation() + " xRotation: " + place.GetXRotation());
		}

		// Draw a dot on the BLOB centres
		foreach (Point center in activePicture.returnBlobCentres()) {

			for (int i = 0; i < 5; i++) {
				for (int j = 0; j < 5; j++) {
					outputImage[center.Y - i, center.X - j] = new Rgb(0, 0, 255);
				}
			}
		}
		
		//Rotate the canvas so the image is not upside-down and mirrored	
		GetComponent<RectTransform>().Rotate(new Vector3(0, 180, 180));
		
		//Set the size of the canvas to the same as the image.
		GetComponent<RectTransform>().sizeDelta = new Vector2(outputImage.Width, outputImage.Height);

		//Apply the image taken to the canvas.
		Texture2D tex = ReturnAsTexture(outputImage);
		rawImage.texture = tex;
		rawImage.material.mainTexture = tex;
		webcamActive = false;
					
		
		firstPress++;
		outputImage.Save(PhotoName());

	}

	// Function for resetting the shown image
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

	// Taking a picture
	public BlobDetection TakePicture(byte[] bytes) {
		
		// Go through the skincolor segmentation
		SkinColorSegmentation scs = new SkinColorSegmentation(bytes);
		Image<Gray,byte> segmentedImage = scs.GetSkinRegion();
		segmentedImage = segmentedImage.ThresholdBinaryInv(new Gray(150), new Gray(255));

		// Go through the edge detection
		EdgeDetection ed = new EdgeDetection(segmentedImage);
		ed.DetectEdges();

		// Go through the BLOB detection
		BlobDetection blobDetector = new BlobDetection(ed.detectedEdges);
		blobDetector.DetectBlobs();

		return blobDetector;
	}

	//Function that returns path and date - used for saving photos.
	public string PhotoName () {
		return string.Format("{0}/Resources/Evaluation/photo_{1}.png",
			Application.dataPath,
			DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss"));
	}
}
