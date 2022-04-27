using System.Text.RegularExpressions;

namespace Saeed.Utilities.Extensions.Strings
{
    public static class ConvertNumbersToEnglish
    {
        public static string FaToEn(string fa)
        {
            return fa
                .Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("٤", "4")
                .Replace("۵", "5")
                .Replace("٥", "5")
                .Replace("۶", "6")
                .Replace("٦", "6")
                .Replace("v", "7")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("٩", "9")
                .Replace("۹", "9");
        }

        public static string ToEn(string input)
        {
            string EnglishNumbers = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    EnglishNumbers += char.GetNumericValue(input, i);
                }
                else
                {
                    EnglishNumbers += input[i].ToString();
                }
            }
            return EnglishNumbers;
        }

        public static bool IsStringNumber(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }
    }
}
