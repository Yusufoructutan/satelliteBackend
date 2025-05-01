using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Gelen veriyi local time olarak al
        var value = reader.GetString();
        if (DateTime.TryParse(value, out DateTime result))
        {
            return DateTime.SpecifyKind(result, DateTimeKind.Local);
        }

        throw new JsonException("Geçersiz tarih formatı.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // Yazarken ISO 8601 formatı
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss"));
    }
}
