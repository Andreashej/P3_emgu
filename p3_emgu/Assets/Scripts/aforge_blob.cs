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
	class BlobDetection {
		Bitmap image;
		Texture2D tex;
		Blob[] blobs;
		GrahamConvexHull hullFinder;
		BitmapData data;
		List<System.Drawing.Point> blobCentres = new List<System.Drawing.Point>();

		BlobCounter blobCounter = new BlobCounter();

		public BlobDetection(Image<Gray, byte> inputImg) {
			image = inputImg.ToBitmap();

			tex = new Texture2D(image.Width, image.Height);

			
		}

		public void DetectBlobs() {
			
		// set filtering options
		blobCounter.FilterBlobs = true;
	
		blobCounter.MinWidth = 5;
		blobCounter.MinHeight = 5;
		blobCounter.MaxHeight = 200;
		blobCounter.MaxWidth = 400;
		

			blobCounter.ProcessImage(image);
			Blob[] blobs = blobCounter.GetObjectsInformation( );

			// create convex hull searching algorithm
			GrahamConvexHull hullFinder = new GrahamConvexHull( );

			// lock image to draw on it
			BitmapData data = image.LockBits(new Rectangle( 0, 0, image.Width, image.Height ), ImageLockMode.ReadWrite, image.PixelFormat );

			// process each blob
			foreach (Blob blob in blobs) {

				float blobRatio = (float)blob.Rectangle.Width / (float)blob.Rectangle.Height;

				bool mouthRatio = ((float)2.8 < blobRatio && (float)4.5 > blobRatio);
				bool eyeRatio = ((float)1.5 < blobRatio && (float) 3 > blobRatio);

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

					Debug.Log("X: " + avgX + ", Y: " + avgY + ", Ratio: " + blobRatio);
					blobCenter = new System.Drawing.Point(avgX, avgY);

					blobCentres.Add(blobCenter);

					//Draw the edges to image
					Drawing.Polygon(data, hull, System.Drawing.Color.Red);
				}
			}
			//Unlock databits
			image.UnlockBits(data);
		
		}
		
		public List<System.Drawing.Point> returnBlobCentres() {
			return blobCentres;
		}
	}
}
