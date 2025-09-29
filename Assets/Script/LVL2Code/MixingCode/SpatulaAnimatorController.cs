using UnityEngine;

public class SpatulaAnimatorController : MonoBehaviour
{
    [Header("Animator")]
    public Animator spatulaAnimator;

    [Header("Settings")]
    [Range(0.1f, 3f)]
    public float playSpeed = 1f;

    private void OnEnable()
    {
        if (spatulaAnimator != null)
        {
            spatulaAnimator.speed = playSpeed;
            spatulaAnimator.Play("SpatulaAnimator", -1, 0f); // restart from beginning
        }
    }

    public void SetSpeed(float newSpeed)
    {
        playSpeed = newSpeed;
        if (spatulaAnimator != null)
            spatulaAnimator.speed = playSpeed;
    }
}
