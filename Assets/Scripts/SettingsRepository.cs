using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SettingsRepository
{
    private static readonly string FilePath = Application.persistentDataPath + "/settings.dat";

    public static void SaveSettings(GameSettings data)
    {
        var formatter = new BinaryFormatter();
        using (var fileStream = new FileStream(FilePath, FileMode.Create))
        {
            formatter.Serialize(fileStream, data);
        }
        Debug.Log($"Settings saved to {FilePath}");
    }

    public static GameSettings LoadSettings()
    {
        if (File.Exists(FilePath))
        {
            var formatter = new BinaryFormatter();
            using var fileStream = new FileStream(FilePath, FileMode.Open);
            var data = (GameSettings)formatter.Deserialize(fileStream);
            Debug.Log("Loaded settings");
            return data;
        }

        Debug.LogWarning("Settings not found");
        return new GameSettings();
    }
}