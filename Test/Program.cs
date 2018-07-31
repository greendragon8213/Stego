using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Test
{
    //To isolate testing the work with formats, img extensions
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
    }
}
