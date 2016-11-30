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
		/*
		byte[] byteImg = ReturnTextureAsBytes(webcamTexture);

		SkinColorSegmentation scs = new SkinColorSegmentation(byteImg);

		Image<Gray,byte> segmentedImage = scs.GetSkinRegion();

		Texture2D tex = returnAsTexture(segmentedImage);
		rawImage.texture = tex;
		rawImage.material.mainTexture = tex;
		*/
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

	public Texture2D returnAsTexture (Image<Gray, byte> imgInput) {
		Texture2D img = new Texture2D(imgInput.Width, imgInput.Height);
		for (int y = 0; y < img.height; y++) {
			for (int x = 0; x < img.width; x++) {
				UnityEngine.Color color = new UnityEngine.Color((float)imgInput[y, x].Intensity / 256, (float)imgInput[y, x].Intensity / 256, (float)imgInput[y, x].Intensity / 256);
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

			byte[] bytes = ReturnTextureAsBytes(webcamTexture);

			SkinColorSegmentation scs = new SkinColorSegmentation(bytes);

			Image<Gray,byte> segmentedImage = scs.GetSkinRegion();

			Texture2D tex = returnAsTexture(segmentedImage);
			rawImage.texture = tex;
			rawImage.material.mainTexture = tex;

			takeHiResShot = false;
		}
	}
}
