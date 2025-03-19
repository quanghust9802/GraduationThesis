using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Common.Converter;

public class DateTimeConverter : JsonConverter<DateTime?>
{
    private const string DateFormat = "dd/MM/yyyy";

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        if (DateTime.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }
        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString(DateFormat));
    }
}