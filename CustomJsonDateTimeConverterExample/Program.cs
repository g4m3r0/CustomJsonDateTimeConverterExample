using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomJsonDateTimeConverterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = @"{""createdAt"": ""2022-07-29 14:00:26""}";

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new CustomDateTimeJsonConverter());

            MyObject obj = JsonSerializer.Deserialize<MyObject>(json, options);
            Console.WriteLine($"CreatedAt: {obj.CreatedAt}");
        }
    }

    public class MyObject
    {
        [JsonPropertyName("createdAt")]
        [JsonConverter(typeof(CustomDateTimeJsonConverter))]
        public DateTime CreatedAt { get; set; }
    }

    public class CustomDateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                DateTime.ParseExact(reader.GetString()!,
                    "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(dateTimeValue.ToString(
                    "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
    }
}
