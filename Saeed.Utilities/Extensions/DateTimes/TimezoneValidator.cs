using System;
using System.Collections.Generic;
using System.Linq;

namespace Saeed.Utilities.Extensions.DateTimes
{
    public static class TimezoneValidator
    {
        private static readonly IReadOnlyCollection<TimeZoneInfo> TimeZonesCollection;

        static TimezoneValidator()
        {
            TimeZonesCollection = TimeZoneInfo.GetSystemTimeZones();
        }

        public static bool IsValidTimeZone(this string timeZone)
        {
            if (!timeZone.IsValidTimeZoneId()) // search in id's
                if (!timeZone.IsValidTimeZoneName()) // search in display names
                    return false;
            // valid
            return true;

        }
        public static TimeZoneInfo FindTimeZone(string timeZoneName)
        {
            return TimeZonesCollection.FirstOrDefault(x => x.Id == timeZoneName);
        }

        public static bool IsValidTimeZoneName(this string timeZoneName)
        {
            try
            {
                return TimeZonesCollection.Any(x => x.DisplayName == timeZoneName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public static bool IsValidTimeZoneId(this string timeZoneId)
        {
            bool isValid = false;
            try
            {
                //return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId).Id == timeZoneId;
                isValid = TimeZonesCollection.Any(x => x.Id == timeZoneId);
                if (!isValid)
                {
                    TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    isValid = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return isValid;
        }
    }
}