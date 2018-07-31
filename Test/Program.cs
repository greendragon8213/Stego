using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Test
{
    #region Doesn't work and but good
    //class Program
    //{
    //    //static void Main(string[] args)
    //    //{
    //    //    //stegotest
    //    //    //string containerFilePath = @"d:/download (1).jpg"; //works ok
    //    //    //string containerFilePath = @"d:/stegotest/2.bmp"; //works ok
    //    //    string containerFilePath = "d:/stegotest/5.bmp"; //works ok
    //    //    //string containerFilePath = "d:/stegotest/3.bmp"; //works ok
    //    //    //string containerFilePath = "d:/stegotest/2.bmp"; //works ok
    //    //    //string containerFilePath = "d:/stegotest/1.bmp"; //works ok
    //    //    //string containerFilePath = "d:/stegotest/1.jpg"; //works ok

    //    //    Bitmap containerBitmap = new Bitmap(containerFilePath);

    //    //    MakeMoreBlue(containerBitmap);

    //    //    string stegoContainerFilePath = @"d:/test-result.bmp";
    //    //    containerBitmap.Save(stegoContainerFilePath, ImageFormat.Bmp);

    //    //    ////assert 1:
    //    //    //Color newColor = Color.FromArgb(100, 100, 100);
    //    //    //int x, y;
    //    //    //Bitmap stegoContainerToAss = new Bitmap(stegoContainerFilePath);

    //    //    //int count = 0;
    //    //    //for (x = 0; x < stegoContainerToAss.Width; x++)
    //    //    //{
    //    //    //    for (y = 0; y < stegoContainerToAss.Height; y++)
    //    //    //    {
    //    //    //        Color pixelColor2 = stegoContainerToAss.GetPixel(x, y);

    //    //    //        if (newColor != pixelColor2)
    //    //    //        {
    //    //    //            count++;
    //    //    //            Debug.WriteLine($"[{x},{y}]: actual: {pixelColor2.A}, {pixelColor2.R}, {pixelColor2.G}; {pixelColor2.B}");
    //    //    //        }
    //    //    //    }
    //    //    //}

    //    //    //assert 2:
    //    //    //checking            
    //    //    var savedImage = new Bitmap(stegoContainerFilePath);
    //    //    byte[] actualBytes = savedImage.NewGetByteArrayByImageFile(ImageLockMode.ReadOnly).Item1;

    //    //    int count = 0;
    //    //    for (int i = 0; i < actualBytes.Length; i++)
    //    //    {
    //    //        if (actualBytes[i] != 100)
    //    //        {
    //    //            count++;
    //    //            Debug.WriteLine($"index: {i}. expected: {100}, but was: {actualBytes[i]};");
    //    //        }
    //    //    }

    //    //    Debug.WriteLine($"Amount of different bytes: {count}");
    //    //}

    //    private static void MakeMoreBlue(Bitmap bmp)
    //    {
    //        // Specify a pixel format.
    //        PixelFormat pxf = bmp.PixelFormat;//PixelFormat.Format24bppRgb;

    //        // Lock the bitmap's bits.
    //        Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
    //        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

    //        // Get the address of the first line.
    //        IntPtr ptr = bmpData.Scan0;

    //        // Declare an array to hold the bytes of the bitmap.
    //        // int numBytes = bmp.Width * bmp.Height * 3;
    //        int numBytes = bmpData.Stride * bmp.Height;
    //        byte[] rgbValues = new byte[numBytes];

    //        // Copy the RGB values into the array.
    //        Marshal.Copy(ptr, rgbValues, 0, numBytes);

    //        //Do the encryption here!
    //        // Manipulate the bitmap, such as changing the
    //        // blue value for every other pixel in the the bitmap.
    //        for (int counter = 0; counter < rgbValues.Length; counter++)
    //            rgbValues[counter] = 100;

    //        rgbValues[0] = 200;
    //        rgbValues[1] = 50;
    //        rgbValues[2] = 50;

    //        rgbValues[rgbValues.Length - 3] = 200;
    //        rgbValues[rgbValues.Length - 2] = 50;
    //        rgbValues[rgbValues.Length - 1] = 50;

    //        // Copy the RGB values back to the bitmap
    //        Marshal.Copy(rgbValues, 0, ptr, numBytes);

    //        // Unlock the bits.
    //        bmp.UnlockBits(bmpData);
    //    }
    //}
    #endregion

    #region Works but slow
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        //stegotest
    //        string containerFilePath = @"d:/stegotest/3.bmp"; //works ?

    //        Bitmap containerBitmap = new Bitmap(containerFilePath);
    //        Color newColor = Color.FromArgb(43, 43, 43);

    //        int x, y;

    //        // Loop through the images pixels to reset color.
    //        for (x = 0; x < containerBitmap.Width; x++)
    //        {
    //            for (y = 0; y < containerBitmap.Height; y++)
    //            {
    //                Color pixelColor = containerBitmap.GetPixel(x, y);

    //                containerBitmap.SetPixel(x, y, newColor);
    //            }
    //        }

    //        //set markers
    //        containerBitmap.SetPixel(0, 0, Color.FromArgb(200, 10, 10));
    //        containerBitmap.SetPixel(containerBitmap.Width-1, containerBitmap.Height-1, Color.FromArgb(200, 10, 10));

    //        string stegoContainerFilePath = @"d:/test-result.bmp";
    //        containerBitmap.Save(stegoContainerFilePath, ImageFormat.Bmp);

    //        //assert
    //        Bitmap conteinerToAss = new Bitmap(containerFilePath);
    //        Bitmap stegoContainerToAss = new Bitmap(stegoContainerFilePath);

    //        int count = 0;
    //        for (x = 0; x < stegoContainerToAss.Width; x++)
    //        {
    //            for (y = 0; y < stegoContainerToAss.Height; y++)
    //            {
    //                Color pixelColor2 = stegoContainerToAss.GetPixel(x, y);

    //                if(newColor != pixelColor2)
    //                {
    //                    count++;
    //                    Debug.WriteLine($"[{x},{y}]: actual: {pixelColor2.A}, {pixelColor2.R}, {pixelColor2.G}; {pixelColor2.B}");
    //                }
    //            }
    //        }



    //    }
    //}
    #endregion
        
    class Program
    {
        const int MagicNumber = 43;

        static void Main(string[] args)
        {
            //stegotest
            //string containerFilePath = "d:/stegotest/5.bmp"; //works ok
            //string containerFilePath = "d:/stegotest/3.bmp"; //works no
            //string containerFilePath = "d:/stegotest/2.bmp"; //works no
            //string containerFilePath = "d:/stegotest/6.bmp"; //works no
            //string containerFilePath = "d:/stegotest/dont-7.bmp"; //works ok
            //string containerFilePath = "d:/stegotest/8.bmp"; //works ok --no, because no pb required!
            string containerFilePath = "d:/stegotest/9.bmp"; //works ok!
            //string containerFilePath = "C:/Users/Tania/Downloads/barbara_gray.bmp"; //works ok
            //string containerFilePath = "d:/stegotest/1.jpg"; //works no
            Bitmap containerImage = new Bitmap(containerFilePath);

            Bitmap resultImage = UpdateAllBytes(containerImage);

            string resultFilePath = "d:/test_result1.bmp";
            resultImage.Save(resultFilePath, ImageFormat.Bmp);

            //checking            
            var savedImage = new Bitmap(resultFilePath);
            byte[] actualBytes = savedImage.NewGetByteArrayByImageFile(ImageLockMode.ReadOnly).Item1;

            int count = 0;
            for (int i = 0; i < actualBytes.Length; i++)
            {
                if (actualBytes[i] != MagicNumber)
                {
                    count++;
                    Debug.WriteLine($"Index: {i}. Expected value: {MagicNumber}, but was: {actualBytes[i]};");
                }
            }

            Debug.WriteLine($"Amount of different bytes: {count}");            
        }

        //private static Bitmap EncryptMock(Bitmap containerBitmap)
        private static Bitmap UpdateAllBytes(Bitmap bitmap)
        {
            Tuple<byte[], BitmapData> containerTuple = bitmap.NewGetByteArrayByImageFile(ImageLockMode.ReadWrite);
            byte[] bytes = containerTuple.Item1;
            BitmapData bitmapData = containerTuple.Item2;

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = MagicNumber;
            }
            
            bitmap.UpdateBitmapPayloadBytes(bytes, bitmapData);
            //// Copy the RGB values back to the bitmap
            //Marshal.Copy(bytes, 0, bitmapData.Scan0, bytes.Length);

            //// Unlock the bits.
            //bitmap.UnlockBits(bitmapData);

            //Marshal.Copy(coverBytes, 0, coverBitmap.Scan0, coverBytes.Length);
            //container.MyUnlockBits(coverBitmap);

            return bitmap;
        }

        /*static void Main(string[] args)
        {
            string initialFilePath = "d:/init_img.bmp";
            Image img = Image.FromFile(initialFilePath);
            var tuple = img.GetByteArrayByImageFile(ImageLockMode.ReadOnly);
            byte[] expectedBytes = tuple.Item1;
            BitmapData bitmapData = tuple.Item2;

            //just paint it gray
            for (int i = 0; i < expectedBytes.Length; i++)
            {
                expectedBytes[i] = 200;
            }

            //save to new file
            Marshal.Copy(expectedBytes, 0, bitmapData.Scan0, expectedBytes.Length);
            img.MyUnlockBits(bitmapData);


            //var tuplex = img.GetByteArrayByImageFile(ImageLockMode.ReadOnly);
            //byte[] expectedBytesx = tuplex.Item1;

            string newFilePath = "d:/new_img.bmp";
            img.Save(newFilePath, ImageFormat.Bmp);

            ////checking            
            //var savedImage = (Bitmap)Image.FromFile(newFilePath);
            //byte[] actualBytes = savedImage.GetByteArrayByImageFile(ImageLockMode.ReadOnly).Item1;

            //int count = 0;
            //for (int i = 0; i < actualBytes.Length; i++)
            //{
            //    if (actualBytes[i] != 200)
            //    {
            //        count++;
            //        //Debug.WriteLine($"{i}: expected: {expectedBytes[i]}, actual: {actualBytes[i]};");
            //        Debug.WriteLine($"{i}: actual: {actualBytes[i]};");
            //    }
            //}

            //if (count == 0)
            //    Debug.WriteLine($"Success: Saved and bytes in memory are equal.");
            //else
            //    Debug.WriteLine($"Failure: Wrong bytes count: {count}.");
        } */
    }
}
