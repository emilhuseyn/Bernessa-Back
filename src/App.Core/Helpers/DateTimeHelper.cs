using System;

namespace App.Core.Helpers
{
    public static class DateTimeHelper
    {
        // Azerbaijan timezone (UTC+4)
        private static readonly TimeZoneInfo AzerbaijanTimeZone = 
            TimeZoneInfo.CreateCustomTimeZone(
                "Azerbaijan Standard Time",
                TimeSpan.FromHours(4),
                "Azerbaijan Standard Time",
                "Azerbaijan Standard Time"
            );

        /// <summary>
        /// Get current Azerbaijan time (UTC+4)
        /// </summary>
        public static DateTime GetAzerbaijanNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, AzerbaijanTimeZone);
        }

        /// <summary>
        /// Convert UTC to Azerbaijan time
        /// </summary>
        public static DateTime ToAzerbaijanTime(DateTime utcDateTime)
        {
            if (utcDateTime.Kind != DateTimeKind.Utc)
            {
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            }
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, AzerbaijanTimeZone);
        }

        /// <summary>
        /// Convert Azerbaijan time to UTC
        /// </summary>
        public static DateTime ToUtcFromAzerbaijan(DateTime azerbaijanDateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(azerbaijanDateTime, AzerbaijanTimeZone);
        }

        /// <summary>
        /// Get current UTC time
        /// </summary>
        public static DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
