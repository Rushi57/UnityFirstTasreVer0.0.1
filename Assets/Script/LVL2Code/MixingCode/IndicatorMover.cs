using UnityEngine;

public class IndicatorMover : MonoBehaviour
{
    public RectTransform indicator;   // The blue marker
    public RectTransform colorBar;    // The parent bar (with zones)
    public float speed = 200f;        // Pixels per second
    private bool movingRight = true;

    void Update()
    {
        if (indicator == null || colorBar == null) return;

        // Get boundaries of the bar
        float left = 0;
        float right = colorBar.rect.width;

        // Move indicator
        Vector2 pos = indicator.anchoredPosition;
        if (movingRight)
            pos.x += speed * Time.deltaTime;
        else
            pos.x -= speed * Time.deltaTime;

        // Bounce at edges
        if (pos.x >= right)
        {
            pos.x = right;
            movingRight = false;
        }
        else if (pos.x <= left)
        {
            pos.x = left;
            movingRight = true;
        }

        indicator.anchoredPosition = pos;
    }
}
