using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string lastSaved;
    public bool tutorialDone;
    public string lastScene;
    public string saveTime;

    public static SaveData NewGame(string startScene)
    {
        return new SaveData
        {
            lastScene = startScene,
            tutorialDone = false,
            saveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm")

        };

    }
    
}
