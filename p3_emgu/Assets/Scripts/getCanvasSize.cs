using UnityEngine;
using System.Collections;

public class getCanvasSize : MonoBehaviour {

    public static float canvasWidth;
    public static float canvasHeight;

    // Use this for initialization
    void Start() {
        RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
        canvasWidth = objectRectTransform.rect.width;
        canvasHeight = objectRectTransform.rect.height;
        //Debug.Log(canvasWidth);
    }
}
