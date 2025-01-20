using System.IO;
using UnityEngine;

public static class SaveRepository
{
    private static readonly string FilePath = Application.persistentDataPath + "/save.json";

    public static void SaveGame(SaveData data)
    {
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(FilePath, json);
        Debug.Log($"Game saved to {FilePath}");
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(FilePath))
        {
            var json = File.ReadAllText(FilePath);
            var data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded");
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found");
            return new SaveData();
        }
    }
}