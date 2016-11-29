using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class takePhoto : MonoBehaviour {

	WebCamTexture webcamTexture;
	public RawImage rawimage;
	public int resWidth = 1600; 
	public int resHeight = 900;

	private bool takeHiResShot = false;

	void Start () 
	{
		webcamTexture = new WebCamTexture();
		rawimage.texture = webcamTexture;
		rawimage.material.mainTexture = webcamTexture;
		webcamTexture.Play();

	}

	public static string ScreenShotName() {
		return string.Format("{0}/Resources/photo_{1}.png", 
			Application.dataPath,
			System.DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss"));
	}

	public void TakeHiResShot() {
		takeHiResShot = true;
	}

	void LateUpdate() {
		takeHiResShot |= Input.GetKeyDown("k");
		if (takeHiResShot) {

			Texture2D destTexture = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false);

			Color[] textureData = webcamTexture.GetPixels();

			destTexture.SetPixels(textureData);
			destTexture.Apply();

			byte[] bytes = destTexture.EncodeToPNG();

			//string filename = ScreenShotName();

			//System.IO.File.WriteAllBytes(filename, bytes);
			//Debug.Log(string.Format("Took screenshot to: {0}", filename));

			//takeHiResShot = false;
		}
	}
}
