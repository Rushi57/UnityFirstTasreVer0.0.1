using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndicatorController : MonoBehaviour
{
    public RecipeSO recipe;   // Contains recipe info
    public int levelIndex;    // 0-based index

    [Header("Level Unlock Requirements")]
    [Tooltip("Minimum score required to unlock THIS level.")]
    public int requiredScore = 22;  // default threshold, customizable per level

    [Header("Info Panel Prefab")]
    public GameObject infoPanelPrefab;
    private GameObject spawnedInfoPanel;

    [Header("Button and Visuals")]
    public Image buttonImage;
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.white;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        SetupIndicator();
    }

    void SetupIndicator()
    {
        int previousScore = 0;

        // Prevent out-of-bounds when checking the previous level
        if (levelIndex > 0)
            previousScore = PlayerPrefs.GetInt($"Level{levelIndex - 1}_Score", 0);

        // ✅ Unlock logic depends on requiredScore
        bool isUnlocked = (levelIndex == 0) || (previousScore >= requiredScore);

        if (buttonImage != null)
            buttonImage.color = isUnlocked ? unlockedColor : lockedColor;

        if (button != null)
            button.interactable = isUnlocked;

        Debug.Log($"[IndicatorController] Level {levelIndex} {(isUnlocked ? "Unlocked" : "Locked")} (Prev Score: {previousScore}, Required: {requiredScore})");
    }

    public void ShowDishInfo()
    {
        if (recipe == null || infoPanelPrefab == null) return;

        // Destroy old panel if one exists
        if (spawnedInfoPanel != null)
            Destroy(spawnedInfoPanel);

        // Find the canvas and spawn inside it
        Canvas canvas = FindObjectOfType<Canvas>();
        spawnedInfoPanel = Instantiate(infoPanelPrefab, canvas.transform, false);

        spawnedInfoPanel.SetActive(true);

        // Pass recipe + levelIndex
        var panelRefs = spawnedInfoPanel.GetComponent<InfoPanelController>();
        if (panelRefs != null)
            panelRefs.Setup(recipe, levelIndex);

        // Trigger open animation
        Animator anim = spawnedInfoPanel.GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("Opened");
    }
}
