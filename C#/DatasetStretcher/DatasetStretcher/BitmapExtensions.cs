using System.Drawing;
using System.Drawing.Imaging;

namespace DatasetStretcher
{
	using System;
	using System.Collections.Generic;
	using System.Text;


	public unsafe static class BitmapExtensions
	{
		public static Bitmap MakeGrayscale(this Bitmap original)
		{
			//create a blank bitmap the same size as original
			Bitmap newBitmap = new Bitmap(original.Width, original.Height);

			//get a graphics object from the new image
			using (Graphics g = Graphics.FromImage(newBitmap))
			{

				//create the grayscale ColorMatrix
				ColorMatrix colorMatrix = new ColorMatrix(
					new float[][]
					{
						new float[] {.3f, .3f, .3f, 0, 0},
						new float[] {.59f, .59f, .59f, 0, 0},
						new float[] {.11f, .11f, .11f, 0, 0},
						new float[] {0, 0, 0, 1, 0},
						new float[] {0, 0, 0, 0, 1}
					});

				//create some image attributes
				using (ImageAttributes attributes = new ImageAttributes())
				{

					//set the color matrix attribute
					attributes.SetColorMatrix(colorMatrix);

					//draw the original image on the new image
					//using the grayscale color matrix
					g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
						0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
				}
			}
			return newBitmap;
		}


		public static Bitmap GenerateNoise(this Bitmap original, int intense)
		{
			
			Random r = new Random();
			int width = original.Width;
			int height = original.Height;
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					int def = r.Next(0, 100);
					if (def < intense)
					{
						int op = r.Next(0, 1);
						if (op == 0)
						{
							int num = r.Next(0, intense);
							Color clr = original.GetPixel(x, y);
							int R = (clr.R + clr.R + num) / 2;
							if (R > 255) R = 255;
							int G = (clr.G + clr.G + num) / 2;
							if (G > 255) G = 255;
							int B = (clr.B + clr.B + num) / 2;
							if (B > 255) B = 255;
							Color result = Color.FromArgb(255, R, G, B);
							original.SetPixel(x, y, result);
						}
						else
						{
							int num = r.Next(0, intense);
							Color clr = original.GetPixel(x, y);
							Color result = Color.FromArgb(255, (clr.R + clr.R - num) / 2, (clr.G + clr.G - num) / 2,
								(clr.B + clr.B - num) / 2);
							original.SetPixel(x, y, result);
						}
					}
				}
			}
			return original;
		}


		public static Bitmap Blur(this Bitmap image, Int32 blurSize)
		{
			return Blur(image, new Rectangle(0, 0, image.Width, image.Height), blurSize);
		}

		private unsafe static Bitmap Blur(Bitmap image, Rectangle rectangle, Int32 blurSize)
		{
			Bitmap blurred = new Bitmap(image.Width, image.Height);

			// make an exact copy of the bitmap provided
			using (Graphics graphics = Graphics.FromImage(blurred))
				graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
					new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

			// Lock the bitmap's bits
			BitmapData blurredData = blurred.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, blurred.PixelFormat);

			// Get bits per pixel for current PixelFormat
			int bitsPerPixel = Image.GetPixelFormatSize(blurred.PixelFormat);

			// Get pointer to first line
			byte* scan0 = (byte*)blurredData.Scan0.ToPointer();

			// look at every pixel in the blur rectangle
			for (int xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
			{
				for (int yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
				{
					int avgR = 0, avgG = 0, avgB = 0;
					int blurPixelCount = 0;

					// average the color of the red, green and blue for each pixel in the
					// blur size while making sure you don't go outside the image bounds
					for (int x = xx; (x < xx + blurSize && x < image.Width); x++)
					{
						for (int y = yy; (y < yy + blurSize && y < image.Height); y++)
						{
							// Get pointer to RGB
							byte* data = scan0 + y * blurredData.Stride + x * bitsPerPixel / 8;

							avgB += data[0]; // Blue
							avgG += data[1]; // Green
							avgR += data[2]; // Red

							blurPixelCount++;
						}
					}

					avgR = avgR / blurPixelCount;
					avgG = avgG / blurPixelCount;
					avgB = avgB / blurPixelCount;

					// now that we know the average for the blur size, set each pixel to that color
					for (int x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++)
					{
						for (int y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++)
						{
							// Get pointer to RGB
							byte* data = scan0 + y * blurredData.Stride + x * bitsPerPixel / 8;

							// Change values
							data[0] = (byte)avgB;
							data[1] = (byte)avgG;
							data[2] = (byte)avgR;
						}
					}
				}
			}

			// Unlock the bits
			blurred.UnlockBits(blurredData);

			return blurred;
		}

		public static Bitmap CropImage(this Bitmap img, Rectangle cropArea)
		{
			Bitmap target = new Bitmap(cropArea.Width, cropArea.Height);

			using (Graphics g = Graphics.FromImage(target))
			{
				g.DrawImage(img, new Rectangle(0, 0, target.Width, target.Height),
					cropArea,
					GraphicsUnit.Pixel);
			}

			return target;
		}
	}
}
