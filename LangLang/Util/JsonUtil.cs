using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

public static class JsonUtil
{
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
            MessageBox.Show($"An error occurred while writing to file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public static Dictionary<string, T> ReadFromFile<T>(string path)
    {
        string jsonString = File.ReadAllText(path);
        Dictionary<string, T>? items = null;
        try
        {
            items = JsonSerializer.Deserialize<Dictionary<string, T>>(jsonString);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");
        }
        return items ?? new Dictionary<string, T>();
    }

}