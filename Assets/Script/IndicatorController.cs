using UnityEngine;
using UnityEngine.UI;

public class IndicatorController : MonoBehaviour
{
    [Header("Level Information")]
    public RecipeSO recipe;        // Recipe data for this level
    public int levelIndex;         // 0-based index of this level

    [Header("Info Panel Prefab")]
    public GameObject infoPanelPrefab;
    private GameObject spawnedInfoPanel;

    [Header("Button and Visuals")]
    public Image buttonImage;
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.white;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        SetupIndicator();
    }

    private void OnEnable()
    {
        SetupIndicator(); // refresh when re-entering the map
    }

    private void SetupIndicator()
    {
        bool isUnlocked = false;
        int prevScore = 0;
        int prevTarget = 0;

        if (levelIndex == 0)
        {
            // First level is always unlocked
            isUnlocked = true;
        }
        else
        {
            // Get previous level’s data
            prevScore = PlayerPrefs.GetInt($"Level{levelIndex - 1}_Score", 0);
            prevTarget = PlayerPrefs.GetInt($"Level{levelIndex - 1}_TargetScore", 0);

            // Unlock if previous score met or exceeded its target
            isUnlocked = prevScore >= prevTarget;
        }

        // Update visuals
        if (buttonImage != null)
            buttonImage.color = isUnlocked ? unlockedColor : lockedColor;

        if (button != null)
            button.interactable = isUnlocked;

        Debug.Log($"[IndicatorController] Level {levelIndex} {(isUnlocked ? "Unlocked" : "Locked")} (Prev Score: {prevScore}, Target: {prevTarget})");
    }

    public void ShowDishInfo()
    {
        if (recipe == null || infoPanelPrefab == null)
            return;

        // Destroy old panel
        if (spawnedInfoPanel != null)
            Destroy(spawnedInfoPanel);

        // Find main canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[IndicatorController] No Canvas found in scene!");
            return;
        }

        // Spawn info panel
        spawnedInfoPanel = Instantiate(infoPanelPrefab, canvas.transform, false);
        spawnedInfoPanel.SetActive(true);

        // Pass data
        var panelRefs = spawnedInfoPanel.GetComponent<InfoPanelController>();
        if (panelRefs != null)
            panelRefs.Setup(recipe, levelIndex);

        // Trigger animation if any
        Animator anim = spawnedInfoPanel.GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("Opened");
    }

    // ✅ Save level score + targetScore for unlocking logic
    public static void SaveLevelProgress(int levelIndex, int score, int targetScore)
    {
        PlayerPrefs.SetInt($"Level{levelIndex}_Score", score);
        PlayerPrefs.SetInt($"Level{levelIndex}_TargetScore", targetScore);
        PlayerPrefs.Save();

        Debug.Log($"[IndicatorController] Saved Level {levelIndex} → Score: {score}, Target: {targetScore}");
    }
}
