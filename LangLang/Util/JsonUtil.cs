using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;


namespace LangLang.Util
{
    public static class JsonUtil
    {
        public static void WriteToFile<T>(Dictionary<string, T> objects, string path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            options.Converters.Add(new TimeOnlyConverter());
            string jsonString = JsonSerializer.Serialize(objects, options);

            try
            {
                File.WriteAllText(path, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while writing to file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static Dictionary<string, T> ReadFromFile<T>(string path)
        {
            string jsonString = File.ReadAllText(path);
            Dictionary<string, T>? items = null;
            try
            {
                var options = new JsonSerializerOptions();
                options.Converters.Add(new TimeOnlyConverter());
                items = JsonSerializer.Deserialize<Dictionary<string, T>>(jsonString, options);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            }
            return items ?? new Dictionary<string, T>();
        }
        public class TimeOnlyConverter : JsonConverter<TimeOnly>
        {
            public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException();
                }

                return TimeOnly.Parse(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

    }

}
