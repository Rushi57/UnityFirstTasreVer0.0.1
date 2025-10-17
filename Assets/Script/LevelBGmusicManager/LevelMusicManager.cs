using UnityEngine;
using UnityEngine.UI;

public class LevelMusicManager : MonoBehaviour
{
    public static LevelMusicManager Instance;

    [Header("Audio Settings")]

    [SerializeField] private AudioClip backgroundMusic;
    private AudioSource audioSource;

    [Header("UI Reference")]
    [SerializeField] private Slider musicSlider;

    private string VolumeKey = "MusicVolume";

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //Setup
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        //Load saved Volume
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0.5f);

    }


}
