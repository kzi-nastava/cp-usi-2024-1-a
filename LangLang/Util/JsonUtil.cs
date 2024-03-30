﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


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
            Dictionary<string, T>? items;
            try
            {
                string jsonString = File.ReadAllText(path);
                items = JsonSerializer.Deserialize<Dictionary<string, T>>(jsonString);
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
    }
}