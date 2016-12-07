using System;
using AForge.Imaging;
using System.Drawing;
using UnityEngine;
using AForge;
using AForge.Math.Geometry;
using System.Drawing.Imaging;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Assets.Scripts {
	public class BlobDetection {
		Bitmap image;
		Texture2D tex;
		Blob[] blobs;
		GrahamConvexHull hullFinder;
		BitmapData data;
		List<System.Drawing.Point> blobCentres = new List<System.Drawing.Point>();

		BlobCounter blobCounter = new BlobCounter();

		public BlobDetection () { }

		public BlobDetection(Image<Gray, byte> inputImg) {
			image = inputImg.ToBitmap();

			tex = new Texture2D(image.Width, image.Height);

			
		}

		public void DetectBlobs() {
			
		// set filtering options
		blobCounter.FilterBlobs = true;
	
		blobCounter.MinWidth = 20;
		blobCounter.MinHeight = 10;
		blobCounter.MaxHeight = 100;
		blobCounter.MaxWidth = 220;
		

			blobCounter.ProcessImage(image);
			Blob[] blobs = blobCounter.GetObjectsInformation( );

			// create convex hull searching algorithm
			GrahamConvexHull hullFinder = new GrahamConvexHull( );

			// lock image to draw on it
			BitmapData data = image.LockBits(new Rectangle( 0, 0, image.Width, image.Height ), ImageLockMode.ReadWrite, image.PixelFormat );

			// process each blob
			foreach (Blob blob in blobs) {

				float blobRatio = (float)blob.Rectangle.Width / (float)blob.Rectangle.Height;

				bool mouthRatio = ((float)3.5 < blobRatio && (float)8 > blobRatio);
				bool eyeRatio = ((float)1.2 < blobRatio && (float) 3.5 >= blobRatio);

				if (mouthRatio || eyeRatio) {
					List<IntPoint> leftPoints, rightPoints, edgePoints = new List<IntPoint>();
					System.Drawing.Point blobCenter;

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

					//Debug.Log("X: " + avgX + ", Y: " + avgY + ", Ratio: " + blobRatio);
					blobCenter = new System.Drawing.Point(avgX, avgY);

					blobCentres.Add(blobCenter);

					//Draw the edges to image
					Drawing.Polygon(data, hull, System.Drawing.Color.Red);
				}
			}
			//Unlock databits
			image.UnlockBits(data);

			//image.Save("Assets/Resouces/test.jpg",ImageFormat.Jpeg);

		
		}
		
		public List<System.Drawing.Point> returnBlobCentres() {
			return blobCentres;
		}
	}
}
