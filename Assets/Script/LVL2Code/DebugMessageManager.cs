using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;


public class DebugMessageManager : MonoBehaviour
{
   public static DebugMessageManager Instance;

    [Header("UI Reference")]
    public GameObject debugPanel;
    public TextMeshProUGUI debugText;

    [Header("Settings")]
    public float autoHideDelay = 2.5f;

    private Coroutine hideRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if(debugPanel != null)
            debugPanel.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if(debugPanel != null) debugPanel.SetActive(true);
        if(debugText != null) debugText.text = message;

        if(hideRoutine != null) StopCoroutine(hideRoutine);
        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    public void HideMessage()
    {
        if(debugPanel != null) debugPanel.SetActive(false);
        if(hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(autoHideDelay);
        HideMessage();
    }
}
