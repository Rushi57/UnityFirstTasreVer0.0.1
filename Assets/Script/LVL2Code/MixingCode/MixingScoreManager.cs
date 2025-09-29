using UnityEngine;

public class MixingScoreManager : MonoBehaviour
{
    public static MixingScoreManager Instance;

    [Header("Scores")]
    public int totalMixScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMixScore(int amount)
    {
        totalMixScore += amount;
    }

    public void ResetMixScore()
    {
        totalMixScore = 0;
    }
}
