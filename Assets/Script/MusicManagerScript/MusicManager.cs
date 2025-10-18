using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip backgroundMusic;
    private AudioSource audioSource;

    [Header("UI Reference")]
    [SerializeField] private Slider musicSlider; // Drag manually (optional)

    private const string VolumeKey = "MusicVolume";

    private void Awake()
    {
        // Singleton setup: keep one instance across all scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Ensure we have an AudioSource
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.loop = true;
            audioSource.playOnAwake = false;

            // Load saved volume
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
            audioSource.volume = savedVolume;

            // Rebind slider on scene load
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Start background music
        if (backgroundMusic != null)
            PlayBackgroundMusic(false, backgroundMusic);

        BindSlider();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to rebind new scene's slider
        BindSlider();
    }

    private void BindSlider()
    {
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            musicSlider.value = audioSource.volume;
        }
        else
        {
            Debug.Log("[MusicManager] No music slider assigned in this scene.");
        }
    }

    // Called from external scripts (e.g., MusicSliderAssigner)
    public void AssignSlider(Slider newSlider)
    {
        musicSlider = newSlider;
        BindSlider();
    }

    public void SetMusicVolume(float value)
    {
        if (audioSource != null)
        {
            audioSource.volume = value;
            PlayerPrefs.SetFloat(VolumeKey, value);
            PlayerPrefs.Save();
        }
    }

    public void PlayBackgroundMusic(bool resetSong, AudioClip clip = null)
    {
        if (clip != null)
            audioSource.clip = clip;

        if (audioSource.clip != null)
        {
            if (resetSong)
                audioSource.Stop();

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }
}
