namespace Saeed.Utilities.Extensions.Units
{
    public static class UnitConverter
    {
        public static double ConvertBytesToBits(this long bytes)
        {
            return bytes * 1024.0;
        }

        /// <summary>
        /// convert bytes to megabytes with floating points.
        /// ex: 7859666 bytes => 7.495562 mb
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static double ConvertBytesToMegabytes(this long bytes)
        {
            return bytes / 1024f / 1024f; // .ToString("0.00") to short
        }
        /// <summary>
        /// convert bytes to megabytes without floating points and in short ver. ( for ex: 7.524925231933594 = > 7.5)
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ConvertBytesToShortMegabytes(this long bytes)
        {
            return (bytes / 1024f / 1024f).ToString("0.0");
        }

        public static double ConvertBytesToGigabytes(long bytes)
        {
            return bytes / (1024.0 * 1024.0);
        }
        public static double ConvertBytesToTerabytes(long bytes)
        {
            return bytes / (1024.0 * 1024.0 * 1024);
        }
        public static double ConvertBytesToPetabytes(long bytes)
        {
            return bytes / (1024.0 * 1024.0 * 1024);
        }

        public static double ConvertKilobytesToMegabytes(long kilobytes)
        {
            return kilobytes / 1024f;
        }

        public static double ConvertMegabytesToGigabytes(double megabytes) // SMALLER
        {
            // 1024 megabyte in a gigabyte
            return megabytes / 1024.0;
        }

        public static double ConvertMegabytesToTerabytes(double megabytes) // SMALLER
        {
            // 1024 * 1024 megabytes in a terabyte
            return megabytes / (1024.0 * 1024.0);
        }

        public static double ConvertGigabytesToMegabytes(double gigabytes) // BIGGER
        {
            // 1024 gigabytes in a terabyte
            return gigabytes * 1024.0;
        }

        public static double ConvertGigabytesToTerabytes(double gigabytes) // SMALLER
        {
            // 1024 gigabytes in a terabyte
            return gigabytes / 1024.0;
        }

        public static double ConvertTerabytesToMegabytes(double terabytes) // BIGGER
        {
            // 1024 * 1024 megabytes in a terabyte
            return terabytes * (1024.0 * 1024.0);
        }

        public static double ConvertTerabytesToGigabytes(double terabytes) // BIGGER
        {
            // 1024 gigabytes in a terabyte
            return terabytes * 1024.0;
        }
    }
}