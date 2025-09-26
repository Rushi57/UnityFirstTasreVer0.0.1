using System.Collections;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    [Header("Canvas Reference")]
    public GameObject shelfCanvas;
    public GameObject vinegarobj;
    public GameObject oilobj;
    public GameObject soyobj;

    [Header("Animation Durations")]
    public float vinegarDuration = 2.0f;
    public float oilDuration = 2.0f;
    public float soyDuration = 2.0f;

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
                StartCoroutine(PlayAndHide(soyobj, soyDuration, "Soy"));
                break;
        }
    }

    IEnumerator PlayAndHide(GameObject obj, float duration, string actionName)
    {
        obj.SetActive(true);
        Animator anim = obj.GetComponent<Animator>();

        if (anim != null)
        {
            anim.Play("Pour"); // Play pouring animation
        }

        yield return new WaitForSeconds(duration);

        obj.SetActive(false);

        // ✅ Now notify CookingStepManager
        CookingStepManager.Instance.OnActionPerformed(actionName);
    }
}
