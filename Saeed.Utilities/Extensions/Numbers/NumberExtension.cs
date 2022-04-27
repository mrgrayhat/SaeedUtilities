namespace Saeed.Utilities.Extensions.Numbers
{
    public static class NumberExtension
    {
        public static string SeperateByComma(this decimal number)
        {
            return string.Format("{0:n0}", number);
        }

        public static string SeperateByComma(this int number)
        {
            return string.Format("{0:n0}", number);
        }
    }
}
