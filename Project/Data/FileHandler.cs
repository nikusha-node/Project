using System.Text.Json;

namespace Project.Data;

public static class FileHandler
{
    public static void SaveToFile<T>(string path, T data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(path, json);
    }

    public static T? LoadFromFile<T>(string path)
    {
        if (!File.Exists(path))
            return default;

        var json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<T>(json);
    }
}

