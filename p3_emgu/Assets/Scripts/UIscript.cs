using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Assets.Scripts;

public class UIscript : MonoBehaviour {

    public GameObject moustache;
    public Material mat;
    public GameObject panel;
    static GameObject moustache_active;
    public Renderer[] polymesh;

    public float UIWidth;
    public float UIHeight;

    public bool UIActive = false;

	//Get the dimentions of the canvas
    public void displayUI() {	
		UIActive = true;
		UIWidth = getCanvasSize.canvasWidth / 2;
		UIHeight = getCanvasSize.canvasHeight / 2;
	}

    public void canvasClick() {
        UIActive = false;
		GameObject.Destroy(moustache_active);
    }

	//Function used to remove an instantiated moustache.
    public void instantiateMoustache() {
        if (moustache_active != null) {
            GameObject.Destroy(moustache_active);
        }

		//Create new moustache and the calculated position
		moustache_active = GameObject.Instantiate(moustache, new Vector3(LiveSCS.place.GetLocation().X, LiveSCS.place.GetLocation().Y, -20),Quaternion.identity) as GameObject;
		//Apply the calculated rotation to moustache.
		moustache_active.GetComponent<Transform>().Rotate(0, 0, (float)LiveSCS.place.GetXRotation());

		//Offsetting moustaches so they are centered correctly.
		switch(moustache_active.name) {
			case "monocle(Clone)":
				moustache_active.GetComponent<Transform>().position = new Vector3(LiveSCS.place.GetEyeLocation().X, LiveSCS.place.GetEyeLocation().Y, -20);
				break;
			case "chaplin(Clone)":
				moustache_active.GetComponent<Transform>().position = new Vector3(LiveSCS.place.GetLocation().X, LiveSCS.place.GetLocation().Y - 70, -20);
				break;
			case "tomSelleck(Clone)":
				moustache_active.GetComponent<Transform>().position = new Vector3(LiveSCS.place.GetLocation().X + 10, LiveSCS.place.GetLocation().Y - 5, -20);
				break;
			case "handlebar(Clone)":
				moustache_active.GetComponent<Transform>().position = new Vector3(LiveSCS.place.GetLocation().X + 10, LiveSCS.place.GetLocation().Y, -20);
				break;
			default:
				break;
		}
		//Save the picture with moustache - for documentation purposes (and fun)
		Application.CaptureScreenshot(PhotoName());
    }
	//If the UIActive variable is true, the buttons to choose moustache are shown
    void Update() {
        if (UIActive) {
            panel = GameObject.Find("UIPanel");
            if (panel.transform.localPosition.x > UIWidth - 60) {
                Vector3 transitionTemp = new Vector3(20.0f, 0, 0);
                panel.transform.localPosition -= transitionTemp;
			}
			
        }
        else {
            panel = GameObject.Find("UIPanel");
            if (panel.transform.localPosition.x < UIWidth + 60) {
                Vector3 transitionTemp = new Vector3(20.0f, 0, 0);
                panel.transform.localPosition += transitionTemp;
			}
        }
    }
	//Generate name for the photo with moustache
	public string PhotoName () {
		return string.Format("{0}/Resources/Evaluation/photo_{1}_with_moustache.png",
			Application.dataPath,
			DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss"));
	}
}
