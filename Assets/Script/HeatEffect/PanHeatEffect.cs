using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanHeatEffect : MonoBehaviour
{
    [Header("References")]
    public Image panImage; // Assign in prefab inspector
    public AudioSource sizzleAudio; // optional (drag in prefab if you have sfx)

    [Header("Heat Effect Settings")]
    public Color heatedColor = new Color(1f, 0.75f, 0.55f, 1f);
    public float heatUpSpeed = 1.5f;
    public float coolDownSpeed = 1f;

    private Color originalColor;
    private Coroutine heatRoutine;

    void Awake()
    {
        if (panImage == null)
            panImage = GetComponent<Image>();

        originalColor = panImage.color;
    }

    public void StartHeating()
    {
        if (heatRoutine != null) StopCoroutine(heatRoutine);
        heatRoutine = StartCoroutine(HeatUpRoutine());
    }

    public void StopHeating()
    {
        if (heatRoutine != null) StopCoroutine(heatRoutine);
        heatRoutine = StartCoroutine(CoolDownRoutine());
    }

    IEnumerator HeatUpRoutine()
    {
        if (sizzleAudio != null && !sizzleAudio.isPlaying)
            sizzleAudio.Play();

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * heatUpSpeed;
            panImage.color = Color.Lerp(originalColor, heatedColor, t);
            yield return null;
        }
    }

    IEnumerator CoolDownRoutine()
    {
        if (sizzleAudio != null)
            sizzleAudio.Stop();

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * coolDownSpeed;
            panImage.color = Color.Lerp(panImage.color, originalColor, t);
            yield return null;
        }
    }
}
