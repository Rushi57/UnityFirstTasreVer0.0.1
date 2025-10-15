using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public int lastScore;
    public int lasStars;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ResetData()
    {
        lastScore = 0;
        lasStars = 0;
    }
}
