using UnityEngine;
using UnityEngine.UI;

public class MusicSliderAssignerlevel : MonoBehaviour
{
    public Slider levelmusicSlider;

    private void Start()
    {
        if (MusicManager.Instance != null && levelmusicSlider != null)
        {
            MusicManager.Instance.AssignSlider(levelmusicSlider);
        }
    }
}
