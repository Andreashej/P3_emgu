using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Assets.Scripts;

public class SCS_Asset : MonoBehaviour
{
    public RawImage rawImage;


    public Texture2D returnAsTexture(Image<Gray, byte> imgInput)
    {
        Texture2D img = new Texture2D(imgInput.Width, imgInput.Height);
        for (int y = 0; y < img.height; y++)
        {
            for (int x = 0; x < img.width; x++)
            {
                Color color = new Color((float)imgInput[y, x].Intensity / 256, (float)imgInput[y, x].Intensity / 256, (float)imgInput[y, x].Intensity / 256);
                img.SetPixel(x, y, color);
            }
        }

        img.Apply();
        return img;
    }

    // Use this for initialization
    void Start ()
    {
        string srcPath = "Assets/Resources/skorf.jpg";
        Mat srcMat = new Mat(srcPath, LoadImageType.Color);
        var scs = new SkinColorSegmentation(srcMat);
        Image<Gray, byte> result = scs.GetSkinRegion();
        GetComponent<RectTransform>().sizeDelta = new Vector2(result.Width, result.Height);
        rawImage.texture = returnAsTexture(result);
        rawImage.material.mainTexture = returnAsTexture(result);

        CvInvoke.Imwrite("Assets/Resources/skorfthresholded.jpg", result);

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
