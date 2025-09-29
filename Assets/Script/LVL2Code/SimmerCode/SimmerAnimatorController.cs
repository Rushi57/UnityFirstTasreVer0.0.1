using UnityEngine;

public class SimmerAnimatorController : MonoBehaviour
{
    [Header("Animator")]
    public Animator SimmerAnimator;

    [Header("Settings")]
    [Range(0.1f, 3f)]
    public float playSpeed = 1f;

    private void OnEnable()
    {
        if (SimmerAnimator != null)
        {
            SimmerAnimator.speed = playSpeed;
            SimmerAnimator.Play("SimmerAnimator", -1, 0f); // restart from beginning
        }
    }

    public void SetSpeed(float newSpeed)
    {
        playSpeed = newSpeed;
        if (SimmerAnimator != null)
            SimmerAnimator.speed = playSpeed;
    }
}
