using UnityEngine;
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
public class ItemDataList
{
    public ItemData[] items;
}
