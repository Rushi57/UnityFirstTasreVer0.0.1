using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SaveFolder => Path.Combine(Application.persistentDataPath, "saves");

    private static string PathForSlot(int slot) =>
        Path.Combine(SaveFolder, $"saveSlot{slot}.json");

    public static void Save(SaveData data, int slot)
    {
        if (!Directory.Exists(SaveFolder))
            Directory.CreateDirectory(SaveFolder);

        // Add timestamp to save
        data.lastSaved = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string path = PathForSlot(slot);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

#if UNITY_EDITOR
        Debug.Log($"[SaveSystem] Saved slot {slot} -> {path}\n{json}");
#endif
    }

    public static SaveData Load(int slot)
    {
        string path = PathForSlot(slot);
        if (!File.Exists(path))
        {
#if UNITY_EDITOR
            Debug.LogWarning($"[SaveSystem] Tried to load slot {slot}, but no file found at {path}");
#endif
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static bool Exists(int slot)
    {
        string path = PathForSlot(slot);
        return File.Exists(path);
    }

    public static void DeleteSlot(int slot)
    {
        string path = PathForSlot(slot);
        if (File.Exists(path)) File.Delete(path);
    }

    public static void DeleteAllSlots(int slots = 3)
    {
        for (int i = 0; i < slots; i++) DeleteSlot(i);
    }
}
