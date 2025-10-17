#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public static class ItemSOExporter
{
    [System.Serializable]
    public class JsonItemData
    {
        public string itemID;
        public string itemName;
        public string itemType;
        public string spritePath;
        public string choppedSpritePath;
        public string prefabPath;
    }

    [System.Serializable]
    public class JsonItemDataList
    {
        public JsonItemData[] items;
    }

    [MenuItem("Tools/Export All Items To JSON")]
    public static void ExportAllItemsToJson()
    {
        // Find all ItemSO assets
        string[] guids = AssetDatabase.FindAssets("t:ItemSO");
        List<JsonItemData> items = new List<JsonItemData>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemSO so = AssetDatabase.LoadAssetAtPath<ItemSO>(path);
            if (!so) continue;

            items.Add(new JsonItemData
            {
                itemID = so.itemID,
                itemName = so.itemName,
                itemType = so.itemType.ToString(),
                spritePath = so.itemSprite ? AssetDatabase.GetAssetPath(so.itemSprite) : "",
                choppedSpritePath = so.choppedSprite ? AssetDatabase.GetAssetPath(so.choppedSprite) : "",
                prefabPath = so.spawnPrefab ? AssetDatabase.GetAssetPath(so.spawnPrefab) : ""
            });
        }

        JsonItemDataList wrapper = new JsonItemDataList { items = items.ToArray() };
        string json = JsonUtility.ToJson(wrapper, true);

        // Choose where to save it
        string folder = EditorUtility.SaveFolderPanel("Select export folder", Application.dataPath, "");
        if (string.IsNullOrEmpty(folder))
            return;

        string outPath = Path.Combine(folder, "item_data.json");
        File.WriteAllText(outPath, json);

        Debug.Log($"✅ Exported {items.Count} ItemSO assets to:\n{outPath}");
        EditorUtility.RevealInFinder(outPath);
    }
}
#endif
