using System.Drawing;
using System.Drawing.Imaging;

namespace StegoSystem.Extensions
{
    public static class ImageExtensions
    {
        public static string GetImageExtension(this Image image)
        {
            if (image.RawFormat.Equals(ImageFormat.Bmp))
            {
                return "bmp";
            }
            if (image.RawFormat.Equals(ImageFormat.MemoryBmp))
            {
                return "bmp";
            }
            if (image.RawFormat.Equals(ImageFormat.Emf))
            {
                return "emf";
            }
            if (image.RawFormat.Equals(ImageFormat.Wmf))
            {
                return "wmf";
            }
            if (image.RawFormat.Equals(ImageFormat.Gif))
            {
                return "gif";
            }
            if (image.RawFormat.Equals(ImageFormat.Jpeg))
            {
                return "jpeg";
            }
            if (image.RawFormat.Equals(ImageFormat.Png))
            {
                return "png";
            }
            if (image.RawFormat.Equals(ImageFormat.Tiff))
            {
                return "tiff";
            }
            if (image.RawFormat.Equals(ImageFormat.Exif))
            {
                return "exif";
            }
            if (image.RawFormat.Equals(ImageFormat.Icon))
            {
                return "ico";
            }

            return string.Empty;
        }        
    }
}
