using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using UnityEngine;
using AForge;
using AForge.Math.Geometry;
using AForge.Imaging;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Assets.Scripts {
	public class BlobDetection {
		Bitmap image;
		Texture2D tex;
		Blob[] blobs;
		BitmapData data;
		List<System.Drawing.Point> blobCentres = new List<System.Drawing.Point>();
		BlobCounter blobCounter = new BlobCounter();
	
		public BlobDetection () { }

		// Creating a Texture2D element from an Image<Gray, byte> type
		public BlobDetection(Image<Gray, byte> inputImg) {
			image = inputImg.ToBitmap();
			tex = new Texture2D(image.Width, image.Height);
		}
			
		public void DetectBlobs() {
			
			// Set filtering options
			blobCounter.FilterBlobs = true;
		
			blobCounter.MinWidth = 25;
			blobCounter.MaxWidth = 220;
			blobCounter.MinHeight = 15;
			blobCounter.MaxHeight = 100;

			blobCounter.ProcessImage(image);
			Blob[] blobs = blobCounter.GetObjectsInformation( );

			// Instantiate a Convex Hull algorithm
			GrahamConvexHull hullFinder = new GrahamConvexHull( );

			// Lock image to draw on it
			BitmapData data = image.LockBits(new Rectangle( 0, 0, image.Width, image.Height ), ImageLockMode.ReadWrite, image.PixelFormat );

			// Go through each BLOB
			foreach (Blob blob in blobs) {
				//Calculate the width/height ratio of each blob
				float blobRatio = (float)blob.Rectangle.Width / (float)blob.Rectangle.Height;

				//Determine if the blob is within the allowed ratio of mouth and eyes.
				bool mouthRatio = ((float)3.5 < blobRatio && (float)8 > blobRatio);
				bool eyeRatio = ((float)1.2 < blobRatio && (float) 3.5 >= blobRatio);

				//Blobs are only processed if they are within the allowed ratios.
				if (mouthRatio || eyeRatio) {
					List<IntPoint> leftPoints, rightPoints, edgePoints = new List<IntPoint>();
					System.Drawing.Point blobCenter;

					// Get the edge points of the BLOB
					blobCounter.GetBlobsLeftAndRightEdges(blob, out leftPoints, out rightPoints);

					edgePoints.AddRange(leftPoints);
					edgePoints.AddRange(rightPoints);

					// Find center point
					int avgX = 0;
					int avgY = 0;

					//Add all edgepoints in the convex hull.
					foreach (IntPoint edge in edgePoints) {
						avgX += edge.X;
						avgY += edge.Y;
					}
					//Divide all edgepoint with the amount of edgepoint to find weigted center.
					avgX /= edgePoints.Count;
					avgY /= edgePoints.Count;

					//Create a point to return to the placement class.
					blobCenter = new System.Drawing.Point(avgX, avgY);

					blobCentres.Add(blobCenter);
				}
			}
			// Unlock databits
			image.UnlockBits(data);
		}
		
		public List<System.Drawing.Point> returnBlobCentres() {
			return blobCentres;
		}
	}
}
