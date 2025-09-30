using UnityEngine;

public class MiniGameCLoser : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject miniGamePanel;
    public GameObject mainCanvas;
    public GameObject miniGameMech;

    [Header("Step Setting")]
    public bool completeStepOnClose = true;

    public void CloseMinigame()
    {
        if(miniGamePanel != null)
            miniGamePanel.SetActive(false);
        if(miniGameMech != null)
            miniGameMech.SetActive(false);
        if(mainCanvas != null)
            mainCanvas.SetActive(true);
        if(completeStepOnClose && CookingStepManager.Instance != null)
        {
            CookingStepManager.Instance.NextStep();
        }
    }
}
