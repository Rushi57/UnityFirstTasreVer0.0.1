using UnityEngine;
using UnityEngine.UI;

public class MusicSliderAssigner : MonoBehaviour
{
    public Slider musicSlider;

    private void Start()
    {
        if (MusicManager.Instance != null && musicSlider != null)
        {
            MusicManager.Instance.AssignSlider(musicSlider);
        }
    }
}
