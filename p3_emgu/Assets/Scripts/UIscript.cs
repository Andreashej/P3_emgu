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
        moustache_active = Instantiate(moustache, new Vector3(0, 0, 0), Quaternion.identity);
        polymesh = GetComponentsInChildren<Renderer>();
        foreach (Renderer mesh in polymesh) {
            mesh.material = mat;
        }
    }

    void Update() {
        if (UIActive) {
            panel = GameObject.Find("UIPanel");
            if (panel.transform.position.x > UIWidth - 60) {
                Vector3 transitionTemp = new Vector3(20.0f, 0, 0);
                panel.transform.position -= transitionTemp;
            }
        }
        else {
            panel = GameObject.Find("UIPanel");
            if (panel.transform.position.x < UIWidth + 20) {
                Vector3 transitionTemp = new Vector3(20.0f, 0, 0);
                panel.transform.position += transitionTemp;
            }
        }
    }
}
