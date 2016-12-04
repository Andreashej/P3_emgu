using UnityEngine;
using System.Collections;
using AForge.Imaging;
using AForge;
using System.Drawing;
using AForge.Math.Geometry;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class blobdetection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Bitmap image = new Bitmap("Assets/Resources/rasmus2.png");
		Texture2D tex = new Texture2D(image.Width, image.Width);
		
		
		// process image with blob counter
		BlobCounter blobCounter = new BlobCounter( );

		/*
		// set filtering options
		blobCounter.FilterBlobs = true;
		blobCounter.MinWidth = 5;
		blobCounter.MinHeight = 5;
		*/

		blobCounter.ProcessImage(image);
		Blob[] blobs = blobCounter.GetObjectsInformation( );

		// create convex hull searching algorithm
		GrahamConvexHull hullFinder = new GrahamConvexHull( );

		// lock image to draw on it
		BitmapData data = image.LockBits(new Rectangle( 0, 0, image.Width, image.Height ), ImageLockMode.ReadWrite, image.PixelFormat );

		// process each blob
		foreach (Blob blob in blobs) {
			List<IntPoint> leftPoints, rightPoints, edgePoints = new List<IntPoint>();

			// get blob's edge points
			blobCounter.GetBlobsLeftAndRightEdges(blob, out leftPoints, out rightPoints);

			edgePoints.AddRange(leftPoints);
			edgePoints.AddRange(rightPoints);


			// blob's convex hull
			List<IntPoint> hull = hullFinder.FindHull( edgePoints );

			//Find center point
			int avgX = 0;
			int avgY = 0;

			foreach (IntPoint edge in edgePoints) {
				avgX += edge.X;
				avgY += edge.Y;
			}
			avgX /= edgePoints.Count;
			avgY /= edgePoints.Count;

			Debug.Log(avgX + ", " + avgY);

			//Draw the edges to image
			Drawing.Polygon(data, hull, System.Drawing.Color.Red);
		}

		image.UnlockBits(data);

		//Save bitmap as png
		image.Save("Assets/Resources/test3.png");
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
