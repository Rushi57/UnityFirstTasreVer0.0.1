using UnityEngine;

public class KnifeAnimatorController : MonoBehaviour
{
    [Header("Animator")]
    public Animator KnifeAnimator;

    [Header("Settings")]
    [Range(0.1f, 3f)]
    public float playSpeed = 1f;

    private void OnEnable()
    {
        if (KnifeAnimator != null)
        {
            KnifeAnimator.speed = playSpeed;
            KnifeAnimator.Play("KnifeAnimator", -1, 0f); // restart from beginning
        }
    }

    public void SetSpeed(float newSpeed)
    {
        playSpeed = newSpeed;
        if (KnifeAnimator != null)
            KnifeAnimator.speed = playSpeed;
    }
}
