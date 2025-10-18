using UnityEngine;

public class CompleteDishBtnHandler : MonoBehaviour
{
    public CongratsDishPanel congratsPanel; // Drag your panel here

    public void OnButtonClick()
    {
        if (congratsPanel != null)
        {
            congratsPanel.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("CongratsDishPanel not assigned!");
        }
    }
}
