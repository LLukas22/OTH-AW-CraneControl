using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Microsoft.Kinect;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Kinect
{
    public static class FrameExtensions
    {
        //private static IntPtr colorpointer;
        //private static IntPtr irpointer;
        //private static IntPtr depthpointer;

        public static Bitmap ReturnBitmap(this InfraredFrame frame)
        {
            if (frame != null)
            {
                int width = frame.FrameDescription.Width;
                int height = frame.FrameDescription.Height;
                PixelFormat format = PixelFormat.Format32bppRgb;

                ushort[] infraredData = new ushort[frame.FrameDescription.LengthInPixels];
                byte[] pixelData = new byte[frame.FrameDescription.LengthInPixels * 4];

                frame.CopyFrameDataToArray(infraredData);

                for (int infraredIndex = 0; infraredIndex < infraredData.Length; infraredIndex++)
                {
                    ushort ir = infraredData[infraredIndex];
                    byte intensity = (byte)(ir >> 8);

                    pixelData[infraredIndex * 4] = intensity; // Blue
                    pixelData[infraredIndex * 4 + 1] = intensity; // Green   
                    pixelData[infraredIndex * 4 + 2] = intensity; // Red
                    pixelData[infraredIndex * 4 + 3] = 255;
                }

                var bitmap = new Bitmap(width, height, format);
                var bitmapdata = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly,
                    bitmap.PixelFormat
                );
                IntPtr ptr = bitmapdata.Scan0;

                Marshal.Copy(pixelData, 0, ptr, pixelData.Length);
                bitmap.UnlockBits(bitmapdata);

                return bitmap;
            }

            return null;
        }

        public static Bitmap ReturnBitmap(this DepthFrame frame)
        {
            if (frame != null)
            {
                var width = frame.FrameDescription.Width;
                var height = frame.FrameDescription.Height;
                PixelFormat format = PixelFormat.Format32bppRgb;

                var minDepth = frame.DepthMinReliableDistance;
                var maxDepth = frame.DepthMaxReliableDistance;

                var depthData = new ushort[width * height];
                var pixelData = new byte[frame.FrameDescription.LengthInPixels * 4];

                frame.CopyFrameDataToArray(depthData);

                for (var depthIndex = 0; depthIndex < depthData.Length; ++depthIndex)
                {
                    var depth = depthData[depthIndex];
                    var intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                    pixelData[depthIndex * 4] = intensity; // Blue
                    pixelData[depthIndex * 4 + 1] = intensity; // Green
                    pixelData[depthIndex * 4 + 2] = intensity; // Red

                }

                var bitmap = new Bitmap(width, height, format);
                var bitmapdata = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly,
                    bitmap.PixelFormat
                );
                IntPtr ptr = bitmapdata.Scan0;

                Marshal.Copy(pixelData, 0, ptr, pixelData.Length);
                bitmap.UnlockBits(bitmapdata);

                return bitmap;
            }

            return null;
        }


        public static Bitmap ReturnBitmap(this ColorFrame frame)
        {
            if (frame != null)
            {
                int width = frame.FrameDescription.Width;
                int height = frame.FrameDescription.Height;
                PixelFormat format = PixelFormat.Format32bppRgb;

                var pixels = new byte[width * height * 4];

                if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
                    frame.CopyRawFrameDataToArray(pixels);
                else
                    frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);


                var bitmap = new Bitmap(width, height, format);
                var bitmapdata = bitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly,
                    bitmap.PixelFormat
                );
                IntPtr ptr = bitmapdata.Scan0;

                Marshal.Copy(pixels, 0, ptr, pixels.Length);
                bitmap.UnlockBits(bitmapdata);

                return bitmap;
            }

            return null;
        }
    }
}