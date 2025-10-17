using UnityEngine;
using UnityEngine.UI;

public class IndicatorController : MonoBehaviour
{
    [Header("Level Information")]
    public RecipeSO recipe;        // The recipe data for this level
    public int levelIndex;         // 0-based level index (0 = first level)

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
        SetupIndicator(); // Refresh when re-entering map
    }

    private void SetupIndicator()
    {
        bool isUnlocked = false;
        int prevScore = 0;
        int prevTarget = 0;

        if (levelIndex == 0)
        {
            // ✅ First level always unlocked
            isUnlocked = true;
        }
        else
        {
            // ✅ Get previous level’s saved score and target
            prevScore = PlayerPrefs.GetInt($"Level_{levelIndex - 1}_Score", 0);
            prevTarget = PlayerPrefs.GetInt($"Level_{levelIndex - 1}_Target", 0);

            // ✅ Unlock if previous level met its target
            if (prevTarget > 0 && prevScore >= prevTarget)
                isUnlocked = true;
        }

        // ✅ Update button visuals
        if (buttonImage != null)
            buttonImage.color = isUnlocked ? unlockedColor : lockedColor;

        if (button != null)
            button.interactable = isUnlocked;

        Debug.Log($"[IndicatorController] Level {levelIndex} → {(isUnlocked ? "Unlocked" : "Locked")} | Prev Score: {prevScore}, Target: {prevTarget}");
    }

    // ---------------- SHOW INFO PANEL ----------------
    public void ShowDishInfo()
    {
        if (recipe == null || infoPanelPrefab == null)
            return;

        if (spawnedInfoPanel != null)
            Destroy(spawnedInfoPanel);

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[IndicatorController] No Canvas found in scene!");
            return;
        }

        spawnedInfoPanel = Instantiate(infoPanelPrefab, canvas.transform, false);
        spawnedInfoPanel.SetActive(true);

        var panelRefs = spawnedInfoPanel.GetComponent<InfoPanelController>();
        if (panelRefs != null)
            panelRefs.Setup(recipe, levelIndex);

        Animator anim = spawnedInfoPanel.GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("Opened");
    }

    // ---------------- SAVE PROGRESS ----------------
    public static void SaveLevelProgress(int levelIndex, int score, int targetScore)
    {
        PlayerPrefs.SetInt($"Level_{levelIndex}_Score", score);
        PlayerPrefs.SetInt($"Level_{levelIndex}_Target", targetScore);
        PlayerPrefs.Save();

        Debug.Log($"[IndicatorController] Saved progress → Level {levelIndex} | Score: {score} / Target: {targetScore}");
    }
}
