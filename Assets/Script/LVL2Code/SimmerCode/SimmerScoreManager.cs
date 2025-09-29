using UnityEngine;

public class SimmerScoreManager : MonoBehaviour
{
    public static SimmerScoreManager Instance;

    [Header("Scores")]
    public int simmerTotalScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddSimmerScore(int amount)
    {
        simmerTotalScore += amount;
    }

    public void ResetSimmerScore()
    {
        simmerTotalScore = 0;
    }
}
