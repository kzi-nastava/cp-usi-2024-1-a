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

    public static Dictionary<string, Student> loadAllStudents(string path)
    {
        string jsonString = File.ReadAllText(path);
        Dictionary<string, Student>? students = null;
        try
        {
            students = JsonSerializer.Deserialize<Dictionary<string, Student>>(jsonString);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing student JSON: {ex.Message}");
        }
        return students ?? new Dictionary<string, Student>();
    }
    /*
    public static Dictionary<string, Tutor> loadAllTutors(string path)
    {
        string jsonString = File.ReadAllText(path);
        Dictionary<string, Tutor>? students = null;
        try
        {
            tutors = JsonSerializer.Deserialize<Dictionary<string, Tutor>>(jsonString);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing tutor JSON: {ex.Message}");
        }
        return tutors ?? new Dictionary<string, Tutor>();
    }

    public static Dictionary<string, Director> loadAllDirectors(string path)
    {
        string jsonString = File.ReadAllText(path);
        Dictionary<string, Director>? students = null;
        try
        {
            directors = JsonSerializer.Deserialize<Dictionary<string, Director>>(jsonString);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing director JSON: {ex.Message}");
        }
        return directors ?? new Dictionary<string, Director>();
    }
    */


}