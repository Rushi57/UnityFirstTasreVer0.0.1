using System.Collections;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    [Header("Canvas Reference")]
    public GameObject shelfCanvas;

    [Header("Condiment Objects (with Pour Animation)")]
    public GameObject vinegarobj;
    public GameObject oilobj;
    public GameObject soyobj;
    public GameObject waterobj;
    public GameObject saltobj;

    [Header("Animation Durations")]
    public float vinegarDuration = 2.0f;
    public float oilDuration = 2.0f;
    public float soyDuration = 2.0f;
    public float waterDuration = 2.0f;
    public float saltDuration = 2.0f;

    public void ShowShelf()
    {
        shelfCanvas.SetActive(true);
    }

    // Called when player selects a condiment
    public void OnCondimentSelected(string type)
    {
        shelfCanvas.SetActive(false);

        switch (type)
        {
            case "Vinegar":
                StartCoroutine(PlayAndHide(vinegarobj, vinegarDuration, "Vinegar"));
                break;
            case "Oil":
                StartCoroutine(PlayAndHide(oilobj, oilDuration, "Oil"));
                break;
            case "Soy":
                StartCoroutine(PlayAndHide(soyobj, soyDuration, "Soy")); // ✅ use same name as PanStateHandler
                break;
            case "Water":
                StartCoroutine(PlayAndHide(waterobj, waterDuration, "Water"));
                break;
            case "Salt":
                StartCoroutine(PlayAndHide(saltobj, saltDuration, "Salt"));
                break;
        }
    }

    IEnumerator PlayAndHide(GameObject obj, float duration, string actionName)
    {
        if (obj == null)
        {
            Debug.LogWarning($"[ShelfManager] Missing object for {actionName}");
            yield break;
        }

        obj.SetActive(true);

        Animator anim = obj.GetComponent<Animator>();
        if (anim != null)
            anim.Play("Pour");

        yield return new WaitForSeconds(duration);

        obj.SetActive(false);

        // ✅ Notify CookingStepManager
        bool correct = CookingStepManager.Instance.OnActionPerformed(actionName);

        // ✅ Find the currently active cookware (pan/pot/wok)
        PanStateHandler panHandler = FindObjectOfType<PanStateHandler>();
        if (panHandler == null)
        {
            Debug.LogWarning("[ShelfManager] No active PanStateHandler found!");
            yield break;
        }

        // ✅ Update cookware sprite only if action was correct
        if (correct)
        {
            panHandler.AdvanceStep();
            Debug.Log($"[ShelfManager] Updated pan sprite with {actionName}");
        }
    }
}
