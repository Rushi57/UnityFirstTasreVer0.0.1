using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SaveLoadSlot : MonoBehaviour
{
    [Tooltip("0-based slot index")]
    public int slotIndex = 0;

    [Tooltip("TextMeshProUGUI label inside the button that shows 'No Save Data' or the timestamp")]
    public TextMeshProUGUI label;

    [Tooltip("Set true if this slot is used on the MainMenu Load panel (Load-only). Set false if used on the Map Save panel (Save/Overwrite).")]
    public bool isLoadMenu = true;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        // auto-hook the button to this script's OnClick
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnEnable()
    {
        Refresh();
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnClick);
    }

    public void Refresh()
    {
        SaveData data = SaveSystem.Load(slotIndex);
        if (data == null)
        {
            label.text = "No Save Data";
            if (isLoadMenu)
                button.interactable = false; // disable empty slot in load menu
            else
                button.interactable = true;  // in save panel it's always clickable (overwrite)
        }
        else
        {
            label.text = $"Saved: {data.saveTime}";
            button.interactable = true;
        }
    }

    // called when button clicked (wired automatically)
    public void OnClick()
    {
        if (isLoadMenu) LoadGame();
        else SaveGame();
    }

    private void SaveGame()
    {
        var data = new SaveData
        {
            lastScene = SceneManager.GetActiveScene().name,
            tutorialDone = PlayerPrefs.GetInt("TutorialDone", 0) == 1,
            saveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        // NEW: collect scores/stars from PlayerPrefs
        for (int i = 0; i < 5; i++) // assume 5 levels, change as needed
        {
            int score = PlayerPrefs.GetInt($"Level{i}_Score", 0);
            int stars = PlayerPrefs.GetInt($"Level{i}_Stars", 0);

            data.levelProgressList.Add(new LevelProgress
            {
                levelIndex = i,
                score = score,
                stars = stars
            });
        }
        SaveSystem.Save(data, slotIndex);
        Refresh();
    }

    private void LoadGame()
    {
        var data = SaveSystem.Load(slotIndex);
        if (data == null)
        {
            Debug.LogWarning($"No save in slot {slotIndex}");
            return;
        }

        // restore tutorial flag
        PlayerPrefs.SetInt("TutorialDone", data.tutorialDone ? 1 : 0);

        // NEW: restore scores/stars to PlayerPrefs
        foreach (var level in data.levelProgressList)
        {
            PlayerPrefs.SetInt($"Level{level.levelIndex}_Score", level.score);
            PlayerPrefs.SetInt($"Level{level.levelIndex}_Stars", level.stars);
        }

        PlayerPrefs.Save();

        SceneManager.LoadScene(data.lastScene);
    }

}
