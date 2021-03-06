﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace StegoSystem.Common.Extensions
{
    public static class BitmapExtensions
    {
        public static (byte[] PayloadBytes, BitmapData Bitmap, int Depth) GetBitmapParts(this Bitmap bitmap, ImageLockMode imageLockMode = ImageLockMode.ReadOnly)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap));
            }

            PixelFormat pxf = bitmap.PixelFormat;
            int depth = Image.GetPixelFormatSize(pxf);

            int bytesPerPixel = depth / 8;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, imageLockMode, pxf);

            // Get the address of the first line.
            IntPtr ptr = bitmapData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int rowPayloadLength = bitmapData.Width * bytesPerPixel;
            int payloadLength = rowPayloadLength * bitmap.Height;
            byte[] payloadValues = new byte[payloadLength];

            // Copy the values into the array.
            for (var r = 0; r < bitmap.Height; r++)
            {
                Marshal.Copy(ptr, payloadValues, r * rowPayloadLength, rowPayloadLength);
                ptr += bitmapData.Stride;
            }

            return (payloadValues, bitmapData, depth);
        }

        public static void UpdateBitmapPayloadBytes(this Bitmap bitmap, byte[] bytes, BitmapData bitmapData)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap));
            }

            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (bitmapData == null)
            {
                throw new ArgumentNullException(nameof(bitmapData));
            }

            PixelFormat pxf = bitmap.PixelFormat;
            int depth = Image.GetPixelFormatSize(pxf);

            int bytesPerPixel = depth / 8;

            IntPtr ptr = bitmapData.Scan0;

            int rowPayloadLength = bitmapData.Width * bytesPerPixel;

            if (bytes.Length != bitmap.Height * rowPayloadLength)
            {
                throw new ArgumentException("Wrong bytes length.", nameof(bytes));
            }

            for (var r = 0; r < bitmap.Height; r++)
            {
                Marshal.Copy(bytes, r * rowPayloadLength, ptr, rowPayloadLength);
                ptr += bitmapData.Stride;
            }

            // Unlock the bits.
            bitmap.UnlockBits(bitmapData);
        }

        public static string Save(this Bitmap bitmap, string destinationPath, string fileName, ImageFormat imageFormat)
        {
            string fileExtension = imageFormat.ToString().ToLower();
            string baseFileName = Path.GetFileNameWithoutExtension(fileName);

            int i = 0;
            string path = Path.Combine(destinationPath, new FileInfo(Path.ChangeExtension(fileName, fileExtension)).Name);
            
            while (System.IO.File.Exists(path))
            {
                i++;
                string newFileName = $"{baseFileName}_{i}.{fileExtension}";
                path = Path.Combine(destinationPath, newFileName);
            }

            bitmap.Save(path, imageFormat);
            return path;
        }
    }
}
