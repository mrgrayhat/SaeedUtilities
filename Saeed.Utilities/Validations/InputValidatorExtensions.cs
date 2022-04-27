using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Saeed.Utilities.Validations
{
    public static class InputValidatorExtensions
    {

        private static readonly Regex PersianDateRegex = new Regex(@"^(\d{4})\/(0?[1-9]|1[012])\/(0?[1-9]|[12][0-9]|3[01])$", RegexOptions.Compiled);
        private static readonly Regex PhoneNumberRegex = new Regex(@"^09[0|1|2|3|9][0-9]{8}$", RegexOptions.Compiled);

        public static bool IsValidPhoneNumber(this string input)
        {
            return PhoneNumberRegex.IsMatch(input);
        }

        public static bool IsValidPersianDate(this string date)
        {
            return PersianDateRegex.IsMatch(date);
        }
        #region Values & Nullable Check

        /// <summary>
        /// utility method for throwing an <see cref="ArgumentNullException"/> if the object is
        /// <c>null</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static T ThrowIfNull<T>(this T obj, string paramName)
        {
            return obj is null ? throw new ArgumentNullException($"{paramName} can not be null!") : obj;
        }

        /// <summary>
        /// utility method for checking <c>null</c> values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T obj)
        {
            return obj is null;
        }

        /// <summary>
        /// utility method for throwing exception for <c>null</c> or <c>empty</c> strings.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="paramName"></param>
        /// <param name="allowWhiteSpaces">allow white space (Like " ")</param>
        /// <returns></returns>
        public static string ThrowIfNullOrEmpty(this string str, string paramName, bool allowWhiteSpaces = false)
        {
            if (allowWhiteSpaces)
            {
                return string.IsNullOrEmpty(str) ?
                    throw new ArgumentNullException($"{paramName} can not be null, empty or contain whitespaces !") : str;
            }
            return string.IsNullOrWhiteSpace(str) ?
                throw new ArgumentNullException($"{paramName} can not be null or empty!") : str;
        }

        /// <summary>
        /// utility method for checking <c>null</c> or <c>empty</c> and whitespace strings.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="allowWhiteSpaces">allow white space (Like " ")</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str, bool allowWhiteSpaces = false)
        {
            if (allowWhiteSpaces)
                return str is null || str.Length == 0;
            else
                return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Returns <c>true</c> in case the enumerable is <c>null</c> or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || collection.Any();
        }

        #endregion
    }
}
