using System.Drawing;
using System.Drawing.Imaging;

namespace StegoSystem.GeneralLogic.Common
{
    public static class ImageExtensions
    {
        public static string GetImageExtension(this Image image)
        {
            if (image.RawFormat.Equals(ImageFormat.Bmp))
            {
                return "bmp";
            }
            else if (image.RawFormat.Equals(ImageFormat.MemoryBmp))
            {
                return "bmp";
            }
            else if (image.RawFormat.Equals(ImageFormat.Emf))
            {
                return "emf";
            }
            else if (image.RawFormat.Equals(ImageFormat.Wmf))
            {
                return "wmf";
            }
            else if (image.RawFormat.Equals(ImageFormat.Gif))
            {
                return "gif";
            }
            else if (image.RawFormat.Equals(ImageFormat.Jpeg))
            {
                return "jpeg";
            }
            else if (image.RawFormat.Equals(ImageFormat.Png))
            {
                return "png";
            }
            else if (image.RawFormat.Equals(ImageFormat.Tiff))
            {
                return "tiff";
            }
            else if (image.RawFormat.Equals(ImageFormat.Exif))
            {
                return "exif";
            }
            else if (image.RawFormat.Equals(ImageFormat.Icon))
            {
                return "ico";
            }

            return string.Empty;
        }        
    }
}
