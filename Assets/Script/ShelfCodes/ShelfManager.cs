using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ShelfManager : MonoBehaviour
{
    [Header ("Canvas Reference")]
    public GameObject shelfCanvas;
    public GameObject vinegarobj;
    public GameObject oilobj;
    public GameObject soyobj;

    [Header("Aimation Durations")]
    public float vinegarDuration = 2.0f;
    public float oilDuration = 2.0f;
    public float soyDuration = 2.0f;

    public void ShowShelf()
    {
        shelfCanvas.SetActive (true);
    }

    //Called when player select a condiment

    public void OnCondimentSelected(string type)
    {
        shelfCanvas.SetActive(false);

        switch(type)
        {
            case "Vinegar":
                StartCoroutine(PlayAndHide(vinegarobj, vinegarDuration));
                break;

            case "Oil":
                StartCoroutine(PlayAndHide(oilobj, oilDuration));
                break;

            case "Soy":
                StartCoroutine(PlayAndHide(soyobj, soyDuration));
                break;
        }
    }
    IEnumerator PlayAndHide(GameObject obj, float duration)
    {
        obj.SetActive (true);
        Animator anim = obj.GetComponent<Animator>();

        if(anim != null )
        {
            anim.Play("Pour");
        }
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
    }
}
