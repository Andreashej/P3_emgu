using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
	class ImageAcquisition {
		WebCamTexture webcamTexture;
		public RawImage rawImage;
		public int resWidth = 1600;
		public int resHeight = 900;

		private bool takeHiResShot = false;

		public ImageAcquisition() {
			webcamTexture = new WebCamTexture();
			rawImage.texture = webcamTexture;
			rawImage.material.mainTexture = webcamTexture;
			webcamTexture.Play();
		}

		public static string ScreenShotName () {
			return string.Format("{0}/Resources/photo_{1}.png",
				Application.dataPath,
				System.DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss"));
		}

		public byte[] ReturnTextureAsBytes(Texture2D tex) {
			Texture2D destTexture = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);

			Color[] textureData = tex.GetPixels();

			destTexture.SetPixels(textureData);
			destTexture.Apply();

			byte[] bytes = destTexture.EncodeToPNG();

			string filename = ScreenShotName();

			System.IO.File.WriteAllBytes(filename, bytes);
			Debug.Log(string.Format("Took screenshot to: {0}", filename));

			return bytes;
		}

	}
}
