using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.Core.Helpers;

namespace App.Business.Converters
{
    /// <summary>
    /// JSON converter that converts UTC dates to Azerbaijan time (UTC+4) for API responses
    /// </summary>
    public class AzerbaijanDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateTimeString = reader.GetString();
            if (DateTime.TryParse(dateTimeString, out var dateTime))
            {
                // Convert incoming time to UTC
                if (dateTime.Kind == DateTimeKind.Unspecified)
                {
                    // Assume incoming time is Azerbaijan time
                    return DateTimeHelper.ToUtcFromAzerbaijan(dateTime);
                }
                return dateTime.ToUniversalTime();
            }
            return default;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Convert UTC to Azerbaijan time for API response
            var azerbaijanTime = DateTimeHelper.ToAzerbaijanTime(value);
            writer.WriteStringValue(azerbaijanTime.ToString("yyyy-MM-ddTHH:mm:ss"));
        }
    }

    /// <summary>
    /// JSON converter for nullable DateTime
    /// </summary>
    public class AzerbaijanNullableDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateTimeString = reader.GetString();
            if (string.IsNullOrEmpty(dateTimeString))
                return null;

            if (DateTime.TryParse(dateTimeString, out var dateTime))
            {
                if (dateTime.Kind == DateTimeKind.Unspecified)
                {
                    return DateTimeHelper.ToUtcFromAzerbaijan(dateTime);
                }
                return dateTime.ToUniversalTime();
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                var azerbaijanTime = DateTimeHelper.ToAzerbaijanTime(value.Value);
                writer.WriteStringValue(azerbaijanTime.ToString("yyyy-MM-ddTHH:mm:ss"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
