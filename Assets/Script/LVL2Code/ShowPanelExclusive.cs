using UnityEngine;

public class ShowPanelExclusive : MonoBehaviour
{
    [Header("Panel Settings")]
    [Tooltip("The panel to show when this prefab spawns or becomes a child.")]
    public GameObject panelToShow;

    [Tooltip("List of other panels to hide when this one activates.")]
    public GameObject[] panelsToHide;

    [Header("Parent Detection (Optional)")]
    [Tooltip("Only trigger when this prefab is a child of this Transform. Leave empty to always trigger.")]
    public Transform expectedParent;

    [Header("Other Settings")]
    public float showDelay = 0f; // optional delay before showing panel

    void Start()
    {
        TryShowPanel();
    }

    void OnTransformParentChanged()
    {
        // Trigger again when moved under new parent
        TryShowPanel();
    }

    private void TryShowPanel()
    {
        if (panelToShow == null) return;

        // Only trigger if under expected parent (if set)
        if (expectedParent != null && transform.parent != expectedParent)
            return;

        if (showDelay > 0)
            Invoke(nameof(ShowExclusivePanel), showDelay);
        else
            ShowExclusivePanel();
    }

    private void ShowExclusivePanel()
    {
        // Hide all other panels first
        foreach (var panel in panelsToHide)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        // Then show the correct one
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
            Debug.Log($"✅ Showing {panelToShow.name} and hiding others for {name}");
        }
    }
}
