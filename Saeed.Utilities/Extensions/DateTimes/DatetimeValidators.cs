using System;
using System.Globalization;

namespace Saeed.Utilities.Extensions.DateTimes
{
    public static class DatetimeValidatorExtensions
    {
        /// <summary>
        /// Checks if the input format is correct or not
        /// </summary>
        /// <param name="dateTimeOffset">datetime offset to validate</param>
        /// <returns>true if it's correct/valid date time, otherwise false</returns>
        public static bool IsValid(this DateTimeOffset dateTimeOffset)
        {
            return DateTimeOffset.TryParse(dateTimeOffset.ToString(CultureInfo.InvariantCulture), out _);
        }
        /// <summary>
        /// Checks if the input format is correct or not
        /// </summary>
        /// <param name="dateTime">datetime to validate</param>
        /// <returns>true if it's correct/valid date time, otherwise false</returns>
        public static bool IsValid(this DateTime dateTime)
        {
            return DateTime.TryParse(dateTime.ToString(CultureInfo.InvariantCulture), out _);
        }
        /// <summary>
        /// determine if datetime is valid and grater than Now (<see cref="DateTime.Now"/>)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>true if it's valid and greater than now, otherwise false</returns>
        public static bool IsValidNow(this DateTime dateTime)
        {
            return dateTime.IsValid() && dateTime.IsBiggerThan(DateTime.Now);
        }
        /// <summary>
        /// compare two DateTime which one is greater
        /// </summary>
        /// <param name="compareFrom">the first datetime, The one that should be greater</param>
        /// <param name="compareWith">date time that is compared to the original (compareFrom)</param>
        /// <returns>true if the source (compareFrom) is greater, otherwise false</returns>
        public static bool IsBiggerThan(this DateTime compareFrom, DateTime compareWith)
        {
            return compareFrom.CompareTo(compareWith) == 1;
        }
        /// <summary>
        /// <inheritdoc cref="IsBiggerThan(DateTime,DateTime)"/>
        /// </summary>
        /// <param name="compareFrom">the first datetime, The one that should be greater</param>
        /// <param name="compareWith">date time that is compared to the original (compareFrom)</param>
        /// <returns>true if the source (compareFrom) is greater, otherwise false</returns>
        public static bool IsBiggerThan(this DateTimeOffset compareFrom, DateTimeOffset compareWith)
        {
            return compareFrom.CompareTo(compareWith) == 1;
        }
        /// <summary>
        /// compare two DateTime Whether it is smaller or not 
        /// </summary>
        /// <param name="compareFrom">the first datetime, The one that should be smaller</param>
        /// <param name="compareWith">the second datetime to compare with first datetime</param>
        /// <returns>true if the compareFrom is less than compare with, otherwise false</returns>
        public static bool IsSmallerThan(this DateTime compareFrom, DateTime compareWith)
        {
            return compareFrom.CompareTo(compareWith) == -1;
        }
        /// <summary>
        /// check whether both date time are equal or not
        /// </summary>
        /// <param name="compareFrom">source datetime to be compared</param>
        /// <param name="compareWith">date time to be compare with source</param>
        /// <returns>true if both are equal, otherwise false</returns>
        public static bool IsEqual(this DateTime compareFrom, DateTime compareWith)
        {
            return compareFrom.CompareTo(compareWith) == 0;
        }
        /// <summary>
        /// Calculate the difference between the two date times
        /// </summary>
        /// <param name="compareFrom">first datetime to be processed</param>
        /// <param name="compareWith">second datetime to subtract with first datetime</param>
        /// <returns>Time difference between two inputs, as timespan </returns>
        public static TimeSpan CalculateDifference(this DateTime compareFrom, DateTime compareWith)
        {
            return compareFrom - compareWith;
        }
        public static TimeSpan CalculateDifference(this DateTimeOffset compareFrom, DateTimeOffset compareWith)
        {
            return compareFrom - compareWith;
        }
        /// <summary>
        /// check provided datetime is utc or not
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>true if its utc, otherwise false</returns>
        public static bool IsUtc(this DateTime dateTime)
        {
            return dateTime == dateTime.ToUniversalTime();
        }
        public static bool IsUtc(this DateTimeOffset dateTime)
        {
            return dateTime == dateTime.ToUniversalTime();
        }

    }
}