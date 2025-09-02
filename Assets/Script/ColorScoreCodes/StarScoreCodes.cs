using UnityEngine;
using UnityEngine.UI;


public class StarScoreCodes : MonoBehaviour
{
    public Image star1;
    public Image star2;
    public Image star3;

    public void SetStarRating(float percentage)
    {
        Color gold;
        Color gray;

        ColorUtility.TryParseHtmlString("#FFD700", out gold);
        ColorUtility.TryParseHtmlString("#808080", out gray);

        star1.color = gray;
        star2.color = gray;
        star3.color = gray;


        if(percentage >= 100f)
        {
            star1.color = gold;
            star2.color = gold;
            star3.color = gold;
        }
        else if(percentage >=75f )
        {
            star1.color = gold;
            star2.color = gold;
        }
        else if (percentage >= 50f)
        {
             star1.color= gold;
        }
    }
    private void Start()
    {
        SetStarRating(75f);
    }
}
