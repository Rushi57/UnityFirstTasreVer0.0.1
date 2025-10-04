using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndicatorController : MonoBehaviour
{
    public RecipeSO recipe;   // <-- Use RecipeSO instead of DishData
    public int levelIndex;    // 0-based

    [Header("Info Panel Prefab")]
    public GameObject infoPanelPrefab;
    private GameObject spawnedInfoPanel;

    [Header("Button and Visual")]
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
        int previousScore = PlayerPrefs.GetInt($"Level{levelIndex - 1}_Score", 0);
        bool isUnlocked = (levelIndex == 0) || (previousScore >= 75);

        if (buttonImage != null) buttonImage.color = isUnlocked ? unlockedColor : lockedColor;
        if (button != null) button.interactable = isUnlocked;
    }

    public void ShowDishInfo()
    {
        if (recipe == null || infoPanelPrefab == null) return;

        // Destroy old panel if one exists
        if (spawnedInfoPanel != null) Destroy(spawnedInfoPanel);

        // Spawn prefab under canvas
        // Find the canvas and spawn inside it
        Canvas canvas = FindObjectOfType<Canvas>();
        spawnedInfoPanel = Instantiate(infoPanelPrefab, canvas.transform, false);

        spawnedInfoPanel.SetActive(true);

        // Pass recipe + levelIndex
        var panelRefs = spawnedInfoPanel.GetComponent<InfoPanelController>();
        if (panelRefs != null)
        {
            panelRefs.Setup(recipe, levelIndex);
        }

        // Trigger open animation
        Animator anim = spawnedInfoPanel.GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("Opened");
    }
}
