using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MusicManagerLevel : MonoBehaviour
{
    public static MusicManagerLevel Instance;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip levelbackgroundMusic;
    private AudioSource levelaudioSource;

    [Header("UI Reference")]
    [SerializeField] private Slider levelmusicSlider;

    [Header("Scenes where music plays")]
    [SerializeField] private string[] allowedScenes; // ← Add scene names here in the Inspector

    private const string VolumeKey = "MusicVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            levelaudioSource = GetComponent<AudioSource>();
            if (levelaudioSource == null)
                levelaudioSource = gameObject.AddComponent<AudioSource>();

            levelaudioSource.loop = true;
            levelaudioSource.playOnAwake = false;

            float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
            levelaudioSource.volume = savedVolume;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (levelbackgroundMusic != null)
            PlayBackgroundMusic(false, levelbackgroundMusic);

        BindSlider();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if current scene is allowed to have music
        if (allowedScenes != null && allowedScenes.Length > 0)
        {
            if (!allowedScenes.Contains(scene.name))
            {
                levelaudioSource.Stop(); // Stop if not in allowed scene
                return;
            }
            else if (!levelaudioSource.isPlaying)
            {
                PlayBackgroundMusic(false, levelbackgroundMusic);
            }
        }

        BindSlider();
    }

    private void BindSlider()
    {
        if (levelmusicSlider != null)
        {
            levelmusicSlider.onValueChanged.RemoveAllListeners();
            levelmusicSlider.onValueChanged.AddListener(SetMusicVolume);
            levelmusicSlider.value = levelaudioSource.volume;
        }
    }

    public void AssignSlider(Slider newSlider)
    {
        levelmusicSlider = newSlider;
        BindSlider();
    }

    public void SetMusicVolume(float value)
    {
        if (levelaudioSource != null)
        {
            levelaudioSource.volume = value;
            PlayerPrefs.SetFloat(VolumeKey, value);
            PlayerPrefs.Save();
        }
    }

    public void PlayBackgroundMusic(bool resetSong, AudioClip clip = null)
    {
        if (clip != null)
            levelaudioSource.clip = clip;

        if (levelaudioSource.clip != null)
        {
            if (resetSong)
                levelaudioSource.Stop();

            if (!levelaudioSource.isPlaying)
                levelaudioSource.Play();
        }
    }
}
