using UnityEngine;
using UnityEngine.UI;
public class SFXSlider : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (sfxSlider == null)
            sfxSlider = GetComponent<Slider>();

        sfxSlider.value = SFXManager.Instance.GetVolume();
        sfxSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void OnVolumeChanged(float value)
    {
        SFXManager.Instance.SetVolume(value);
    }
}
