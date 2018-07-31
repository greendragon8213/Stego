using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SudkuStegoSystem.Logic.Helpers
{
    public static class BitmapExtensions
    {
        public static Tuple<byte[], BitmapData> GetByteArrayByImageFile(this Bitmap bitmap, ImageLockMode imageLockMode = ImageLockMode.ReadOnly)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap));
            }

            PixelFormat pxf = bitmap.PixelFormat;
            int depth = Image.GetPixelFormatSize(pxf);

            CheckIfDepthIsSupported(depth);

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

            return new Tuple<byte[], BitmapData>(payloadValues, bitmapData);
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

            CheckIfDepthIsSupported(depth);

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

        private static void CheckIfDepthIsSupported(int depth)
        {
            if (depth != 8 && depth != 24 && depth != 32)
            {
                throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
            }
        }
    }
}
