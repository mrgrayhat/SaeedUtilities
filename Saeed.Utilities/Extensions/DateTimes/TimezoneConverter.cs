using System;
using System.Globalization;

namespace Saeed.Utilities.Extensions.DateTimes
{
    public static class TimezoneConverter
    {
        public const string IranTimeZone_UNIX = "Asia/Tehran";
        public const string IranTimeZone_NT = "Iran Standard Time";
        public static string PlatformTimezoneId
        {
            get
            {
                return OperatingSystem.IsWindows()
                    ? IranTimeZone_NT
                    : IranTimeZone_UNIX;
            }
        }

        private static TimeZoneInfo _platformTimeZone = null;
        public static TimeZoneInfo PlatformTimeZone => _platformTimeZone ??= TimeZoneInfo.FindSystemTimeZoneById(PlatformTimezoneId);

        /// <summary>
        /// convert a datetime to utc, from specific source timezone
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="fromTimeZone">datetime tz id</param>
        /// <returns></returns>
        public static DateTime? ConvertDateTimeToUtc(this DateTime dateTime, string fromTimeZone)
        {
            try
            {
                var from = TimeZoneInfo.FindSystemTimeZoneById(fromTimeZone);
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, from);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// convert a datetime from source timezone to specific destination timezone
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="fromTimeZone">source tz id</param>
        /// <param name="toTimezone">destinationtz id</param>
        /// <returns></returns>
        public static DateTime? ConvertTimezoneDateTime(this DateTime dateTime, string fromTimeZone, string toTimezone)
        {
            try
            {
                var from = TimeZoneInfo.FindSystemTimeZoneById(fromTimeZone);
                var to = TimeZoneInfo.FindSystemTimeZoneById(toTimezone);
                return TimeZoneInfo.ConvertTime(dateTime, from, to);
            }
            catch
            {
                return null;

            }
        }

        public static DateTime ConvertTimezoneDateTime(this DateTimeOffset dateTimeOffset, string timeZone)
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            return TimeZoneInfo.ConvertTime(dateTimeOffset, info).DateTime;
        }
        /// <summary>
        /// convert a datetime to specified timezone (from utc time to iran standard time, for example), if datetime isn't utc, conversion use offset to convert.
        /// </summary>
        /// <param name="dateTimeoffset"></param>
        /// <param name="timeZone">tz id</param>
        /// <param name="isUtc"></param>
        /// <returns></returns>
        public static DateTime? ConvertTimezoneDateTime(this DateTimeOffset dateTimeoffset, string timeZone, bool isUtc = true)
        {
            try
            {
                var toZoneinfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                return isUtc ? TimeZoneInfo.ConvertTimeFromUtc(dateTimeoffset.DateTime, toZoneinfo) : TimeZoneInfo.ConvertTime(dateTimeoffset, toZoneinfo).DateTime;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// convert a datetime to specified timezone (from utc time to iran standard time, for example), if datetime isn't utc, conversion use offset to convert.
        /// </summary>
        /// <param name="dateTimeoffset"></param>
        /// <param name="timeZone">tz id</param>
        /// <returns></returns>
        public static DateTimeOffset? ConvertTimezoneDateTimeOffset(this DateTimeOffset dateTimeoffset, string timeZone)
        {
            try
            {
                var toZoneinfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                return TimeZoneInfo.ConvertTime(dateTimeoffset, toZoneinfo);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// convert a date time to iran timezone
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="isUtc"></param>
        /// <returns></returns>
        public static DateTime ConvertToIranTimeZone(this DateTime dateTime, bool isUtc = true)
        {
            return isUtc ? TimeZoneInfo.ConvertTimeFromUtc(dateTime, PlatformTimeZone) : TimeZoneInfo.ConvertTime(dateTime, _platformTimeZone);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="isUtc"></param>
        /// <param name="format">convert date time to this format, G (general long time) by default. </param>
        /// <param name="cultureInfo"> culture specific format (CultureInfo), invariant by default. <see cref="IFormatProvider"/></param>
        /// <returns></returns>
        public static string ConvertToIranTimeZoneString(this DateTime dateTime, bool isUtc = true, string format = "G", CultureInfo cultureInfo = null)
        {
            return isUtc
                ? TimeZoneInfo.ConvertTimeFromUtc(dateTime, PlatformTimeZone).ToString(format, cultureInfo ?? CultureInfo.InvariantCulture)
                : TimeZoneInfo.ConvertTime(dateTime, PlatformTimeZone).ToString(format, cultureInfo ?? CultureInfo.InvariantCulture);
        }

        public static DateTimeOffset ConvertToIranTimeZone(this DateTimeOffset dateTimeOffset)
        {
            return TimeZoneInfo.ConvertTime(dateTimeOffset, PlatformTimeZone);
        }

        public static DateTime ConvertPersianToGregorian(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return new DateTime(dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute, 0, pc);
        }

        public static DateTime ConvertGregorianToPersian(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            var date = new DateTime(pc.GetYear(dateTime),
                pc.GetMonth(dateTime),
                pc.GetDayOfMonth(dateTime),
                pc.GetHour(dateTime),
                pc.GetMinute(dateTime), 0);
            return date;
        }
    }

    public sealed class UtcTimeZoneResolver //: ITimeZoneResolver
    {
        public TimeZoneInfo GetTimeZoneById(string timeZoneId)
        {
            return TimeZoneInfo.Utc;
            //return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
    }
}