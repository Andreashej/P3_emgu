using UnityEngine;
using System.Collections;

public class getCanvasSize : MonoBehaviour {

    public static float canvasWidth;
    public static float canvasHeight;

    void Start() {
		// Finding the size of the Unity Canvas
        RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
        canvasWidth = objectRectTransform.rect.width;
        canvasHeight = objectRectTransform.rect.height;
    }
}
