using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KillBill.Client.Net.JSON
{
    public class ShortDateTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime)
            {
                var dateTime = (DateTime)value;
                writer.WriteValue(dateTime.ToString("yyyy-MM-dd"));
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.TokenType == JsonToken.Null ? DateTime.MinValue : DateTime.Parse(reader.Value.ToString());
        }
    }
}