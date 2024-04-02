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
        /**
         * Serializes the Dictionary into JSON object and writes it to a file at the specified path.
         */
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
                throw new Exception($"An error occurred while writing to file: {ex.Message}");
            }
        }

        /**
         * Reads from the JSON file at the specified path and deserializes into a dictionary.
         * The keys in the top level JSON object are the dictionary keys.
         * The values must match the structure of class T, otherwise JsonException is thrown.
         */
        public static Dictionary<string, T> ReadFromFile<T>(string path)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new TimeOnlyConverter());
            Dictionary<string, T>? items;
            try
            {
                string jsonString = File.ReadAllText(path);
                items = JsonSerializer.Deserialize<Dictionary<string, T>>(jsonString, options);
            }
            catch (FileNotFoundException)
            {
                items = new Dictionary<string, T>();
                WriteToFile(items, path);
            }
            catch (DirectoryNotFoundException)
            {
                items = new Dictionary<string, T>();
                WriteToFile(items, path);
            }
            return items ?? new Dictionary<string, T>();
        }

        private class TimeOnlyConverter : JsonConverter<TimeOnly>
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