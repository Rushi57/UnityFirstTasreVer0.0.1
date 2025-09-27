using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ClockZoneRandomizer : MonoBehaviour
{
    public Image redZone;
    public Image yellowZone;
    public Image greenZone;


    [Header("Zone Size (0-1 = 360)")]
    [Range(0f, 1f)] public float redSize = 0.7f; //70% of Cirle
    [Range(0f, 1f)] public float yellowSize = 0.2f; //20% of Cirle
    [Range(0f, 1f)] public float greenSize = 0.1f; //10% of Cirle

    void Start()
    {
        RandomizeZone(); 
    }

    public void RandomizeZone()
    {
        // Random starting angle for Red
        int startAngle = UnityEngine.Random.Range(0, 360);

        // --- Red Zone ---
        redZone.fillAmount = redSize;
        redZone.fillOrigin = (startAngle / 90) % 4;
        redZone.transform.localEulerAngles = new Vector3(0, 0, -startAngle);

        // --- Yellow Zone ---
        int yellowStart = (startAngle + Mathf.RoundToInt(redSize * 360)) % 360;
        yellowZone.fillAmount = yellowSize;
        yellowZone.fillOrigin = (yellowStart / 90) % 4;
        yellowZone.transform.localEulerAngles = new Vector3(0, 0, -yellowStart);

        // --- Green Zone ---
        int greenStart = (yellowStart + Mathf.RoundToInt(yellowSize * 360)) % 360;
        greenZone.fillAmount = greenSize;
        greenZone.fillOrigin = (greenStart / 90) % 4;
        greenZone.transform.localEulerAngles = new Vector3(0, 0, -greenStart);

    }
}
