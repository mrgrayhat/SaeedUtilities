using System.Drawing;
using System.Drawing.Imaging;

namespace Saeed.Utilities.Extensions.Images.Options
{
    /// <summary>
    /// Detect image color format.
    /// <para>https://stackoverflow.com/a/9899904/5519026</para>
    /// </summary>
    public static class ImageColorFormat
    {
        /// <summary>
        /// Get image color format
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static ImageColorFormatTypes GetColorFormat(this Bitmap bitmap)
        {
            // Check image flags
            var flags = (ImageFlags)bitmap.Flags;
            if (flags.HasFlag(ImageFlags.ColorSpaceCmyk) || flags.HasFlag(ImageFlags.ColorSpaceYcck))
            {
                return ImageColorFormatTypes.Cmyk;
            }
            else if (flags.HasFlag(ImageFlags.ColorSpaceGray))
            {
                return ImageColorFormatTypes.Grayscale;
            }

            // Check pixel format
            var pixelFormat = (int)bitmap.PixelFormat;
            if (pixelFormat == (int)ImagePixelFormatTypes.PixelFormat32bppCMYK)
            {
                return ImageColorFormatTypes.Cmyk;
            }
            else if ((pixelFormat & (int)ImagePixelFormatTypes.PixelFormatIndexed) != 0)
            {
                return ImageColorFormatTypes.Indexed;
            }
            else if (pixelFormat == (int)ImagePixelFormatTypes.PixelFormat16bppGrayScale)
            {
                return ImageColorFormatTypes.Grayscale;
            }

            // Default to RGB
            return ImageColorFormatTypes.Rgb;
        }
    }
}
