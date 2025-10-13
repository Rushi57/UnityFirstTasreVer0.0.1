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
    public GameObject pepperobj;
    public GameObject pigbloodObj;

    [Header("Animation Durations")]
    public float vinegarDuration = 2.0f;
    public float oilDuration = 2.0f;
    public float soyDuration = 2.0f;
    public float waterDuration = 2.0f;
    public float saltDuration = 2.0f;
    public float pepperDuration = 2.0f;
    public float pigBloodDuration = 2.0f;

    [Header("Spawnable Prefabs")]
    [Tooltip("Prefab for the water bottle to spawn on the table.")]
    public GameObject waterBottlePrefab;
    [Tooltip("Where the spawned bottle should appear on the table.")]
    public Transform tableSpawnPoint;

    // ============================================================

    public void ShowShelf()
    {
        shelfCanvas.SetActive(true);
    }

    // ============================================================
    // Called when player selects a condiment (triggered by button)
    // ============================================================
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
                StartCoroutine(PlayAndHide(soyobj, soyDuration, "Soy"));
                break;
            case "Water":
                StartCoroutine(PlayAndSpawn(waterobj, waterDuration, "Water"));
                break;
            case "Salt":
                StartCoroutine(PlayAndHide(saltobj, saltDuration, "Salt"));
                break;
            case "Pepper":
                StartCoroutine(PlayAndHide(pepperobj, pepperDuration, "Pepper"));
                break;
            case "Pig":
                StartCoroutine(PlayAndHide(pigbloodObj, pigBloodDuration, "Pig"));
                break;
        }
    }

    // ============================================================
    // 🧂 Default coroutine for normal condiments (no prefab spawn)
    // ============================================================
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

    // ============================================================
    // 💧 Special coroutine for Water (spawns a bottle prefab)
    // ============================================================
    IEnumerator PlayAndSpawn(GameObject obj, float duration, string actionName)
    {
        if (obj == null)
        {
            Debug.LogWarning($"[ShelfManager] Missing object for {actionName}");
            yield break;
        }

        // ✅ Play shelf pouring animation
        obj.SetActive(true);
        Animator anim = obj.GetComponent<Animator>();
        if (anim != null)
            anim.Play("Pour");

        // ✅ Spawn the physical water bottle on the table
        if (waterBottlePrefab != null && tableSpawnPoint != null)
        {
            GameObject bottle = Instantiate(waterBottlePrefab, tableSpawnPoint.position, Quaternion.identity);

            // Optional: play its own animation
            Animator bottleAnim = bottle.GetComponent<Animator>();
            if (bottleAnim != null)
                bottleAnim.Play("Pour");
        }

        yield return new WaitForSeconds(duration);

        obj.SetActive(false);

        // ✅ Notify CookingStepManager
        bool correct = CookingStepManager.Instance.OnActionPerformed(actionName);

        // ✅ Find cookware and update sprite if correct
        PanStateHandler panHandler = FindObjectOfType<PanStateHandler>();
        if (panHandler == null)
        {
            Debug.LogWarning("[ShelfManager] No active PanStateHandler found!");
            yield break;
        }

        if (correct)
        {
            panHandler.AdvanceStep();
            Debug.Log($"[ShelfManager] Updated pan sprite with {actionName}");
        }
    }
}
