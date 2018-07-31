using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Test
{
    public static class ImageExtensions
    {
        public static Tuple<byte[], BitmapData> NewGetByteArrayByImageFile(this Bitmap bmp, ImageLockMode imageLockMode = ImageLockMode.ReadWrite)
        {
            PixelFormat pxf = bmp.PixelFormat;
            int depth = Image.GetPixelFormatSize(pxf);

            CheckImageDepth(depth);

            int bytesPerPixel = depth / 8;

            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bitmapData = bmp.LockBits(rect, imageLockMode, pxf);

            // Get the address of the first line.
            IntPtr ptr = bitmapData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int rowPayloadLength = bitmapData.Width * bytesPerPixel;
            int payloadLength = rowPayloadLength * bmp.Height;
            byte[] payloadValues = new byte[payloadLength];

            // Copy the values into the array.
            for (var r = 0; r < bmp.Height; r++)
            {
                Marshal.Copy(ptr, payloadValues, r * rowPayloadLength, rowPayloadLength);
                ptr += bitmapData.Stride;
            }
            
            return new Tuple<byte[], BitmapData>(payloadValues, bitmapData);
        }

        public static void UpdateBitmapPayloadBytes(this Bitmap bmp, byte[] bytes, BitmapData bitmapData)
        {
            PixelFormat pxf = bmp.PixelFormat;
            int depth = Image.GetPixelFormatSize(pxf);

            CheckImageDepth(depth);

            int bytesPerPixel = depth / 8;

            IntPtr ptr = bitmapData.Scan0;
            
            int rowPayloadLength = bitmapData.Width * bytesPerPixel;
            
            if(bytes.Length != bmp.Height * rowPayloadLength)
            {
                throw new ArgumentException("Wrong bytes length.", nameof(bytes));
            }
            
            for (var r = 0; r < bmp.Height; r++)
            {
                Marshal.Copy(bytes, r * rowPayloadLength, ptr, rowPayloadLength);
                ptr += bitmapData.Stride;
            }
            
            // Unlock the bits.
            bmp.UnlockBits(bitmapData);
        }

        private static void CheckImageDepth(int depth)
        {
            if (depth != 8 && depth != 24 && depth != 32)
            {
                throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
            }
        }
    }
}
