using UnityEngine;

public class LevelSettingsBtn : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;

    public void OpenSettings()
    {
        settingPanel.SetActive(true);

        // 🔊 When the panel is opened, reconnect the music slider
        if (MusicManager.Instance != null)
            MusicManager.Instance.BindSlider();
    }

    public void CloseSettings()
    {
        settingPanel.SetActive(false);
    }
}
