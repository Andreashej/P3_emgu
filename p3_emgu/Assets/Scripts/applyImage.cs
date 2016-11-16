using UnityEngine;
using System.Collections;

public class applyImage : MonoBehaviour {

	public EdgeDetection image;
	public Renderer rend;

	// Use this for initialization
	void Start () {
		image = new EdgeDetection("andreas.jpg");
		rend = GetComponent<Renderer>();

		image.DetectEdges();

		Texture2D texture = image.ReturnAsTexture();

		rend.material.mainTexture = texture;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
