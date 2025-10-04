using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string lastSaved;
    public bool tutorialDone;
    public string lastScene;
    public string saveTime;

    // NEW: Store scores and stars for each level
    public List<LevelProgress> levelProgressList = new List<LevelProgress>();

    public static SaveData NewGame(string startScene)
    {
        return new SaveData
        {
            lastScene = startScene,
            tutorialDone = false,
            saveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            levelProgressList = new List<LevelProgress>()
        };
    }
}

[Serializable]
public class LevelProgress
{
    public int levelIndex;
    public int score;
    public int stars;
}
