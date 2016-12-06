using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIscript : MonoBehaviour {

    public GameObject moustache;
    public Material mat;
    public GameObject panel;
    static Object moustache_active;
    public Renderer[] polymesh;

    public float UIWidth;
    public float UIHeight;

    bool UIActive = false;

    public void displayUI() {
        UIActive = !UIActive;
        UIWidth = getCanvasSize.canvasWidth;
        UIHeight = getCanvasSize.canvasHeight;
        //Debug.Log(UIWidth);
    }

    public void canvasClick() {
        UIActive = false;
    }

    public void instantiateMoustache() {
        if (moustache_active != null) {
            Destroy(moustache_active);
        }

        moustache_active = Instantiate(moustache, new Vector3(LiveSCS.place.GetLocation().X, LiveSCS.place.GetLocation().Y, -20), Quaternion.identity);
        polymesh = GetComponentsInChildren<Renderer>();
        foreach (Renderer mesh in polymesh) {
            mesh.material = mat;
        }
    }

    void Update() {
        if (UIActive) {
            panel = GameObject.Find("UIPanel");
            if (panel.transform.localPosition.x > UIWidth - 860) {
                Vector3 transitionTemp = new Vector3(20.0f, 0, 0);
                panel.transform.localPosition -= transitionTemp;
			}
			
        }
        else {
            panel = GameObject.Find("UIPanel");
            if (panel.transform.localPosition.x < UIWidth - 740) {
                Vector3 transitionTemp = new Vector3(20.0f, 0, 0);
                panel.transform.localPosition += transitionTemp;
			}
        }
    }
}
