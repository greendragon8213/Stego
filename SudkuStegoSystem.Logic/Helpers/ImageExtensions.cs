using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudkuStegoSystem.Logic.Helpers
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Locks the image data in a given access mode.
        /// </summary>
        /// <param name="image">The source image containing the data.</param>
        /// <param name="lockMode">The lock mode (see <see cref="ImageLockMode"/> for more details).</param>
        /// <returns>The locked image data reference.</returns>
        public static BitmapData LockBits(this Image image, ImageLockMode lockMode)
        {
            // checks whether a source image is valid
            if (image == null)
            {
                const String message = "Cannot lock the bits for a null image.";
                throw new ArgumentNullException(message);
            }

            // determines the bounds of an image, and locks the data in a specified mode
            Bitmap bitmap = (Bitmap)image;
            Rectangle bounds = Rectangle.FromLTRB(0, 0, image.Width, image.Height);
            BitmapData result = bitmap.LockBits(bounds, lockMode, image.PixelFormat);
            return result;
        }

        /// <summary>
        /// Unlocks the data for a given image.
        /// </summary>
        /// <param name="image">The image containing the data.</param>
        /// <param name="data">The data belonging to the image.</param>
        public static void UnlockBits(this Image image, BitmapData data)
        {
            // checks whether a source image is valid
            if (image == null)
            {
                const String message = "Cannot unlock the bits for a null image.";
                throw new ArgumentNullException(message);
            }

            // checks if data to be unlocked are valid
            if (data == null)
            {
                const String message = "Cannot unlock null image data.";
                throw new ArgumentNullException(message);
            }

            // releases a lock
            Bitmap bitmap = (Bitmap)image;
            bitmap.UnlockBits(data);
        }
    }
}
