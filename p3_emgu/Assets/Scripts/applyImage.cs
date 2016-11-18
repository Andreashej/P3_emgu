using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class applyImage : MonoBehaviour {

	public RawImage rawImage;
	public EdgeDetection image;

	// Use this for initialization
	void Start () {
		image = new EdgeDetection("rasmus.jpg");

		image.DetectEdges();

		Texture2D texture = image.ReturnAsTexture();

		GetComponent<RectTransform>().sizeDelta = new Vector2(image.getWidth(), image.getHeight());
		GetComponent<RectTransform>().Rotate(new Vector3(0, 180, 180)); 

		rawImage.texture = texture;
		rawImage.material.mainTexture = texture;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
