using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using AForge;
using AForge.Imaging;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using UnityEngine;


namespace Assets.Scripts {
	class IOHandler {

		Texture2D inputTex;
		Image<Gray, byte> workImg;

		IOHandler(Texture2D aquiredImage) {
			inputTex = aquiredImage;

		}

		public Texture2D returnAsTexture (Image<Gray, byte> imgInput) {
			Texture2D img = new Texture2D(imgInput.Width, imgInput.Height);
			for (int y = 0; y < img.height; y++) {
				for (int x = 0; x < img.width; x++) {
					UnityEngine.Color color = new UnityEngine.Color((float)imgInput[y,x].Intensity/256, (float)imgInput[y,x].Intensity/256, (float)imgInput[y,x].Intensity/256);
					img.SetPixel(x, y, color);
				}
			}

			img.Apply();
			return img;
		}
	}
}
