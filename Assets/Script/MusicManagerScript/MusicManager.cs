using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Default Audio (Main Menu / Map)")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Level Music Settings")]
    [Tooltip("Assign background music per level scene name.")]
    [SerializeField] private LevelMusic[] levelMusicList;

    private AudioSource audioSource;

    [Header("UI Reference")]
    [SerializeField] private Slider musicSlider;

    [Header("Scenes where default music plays")]
    [SerializeField] private string[] allowedScenes;

    private const string VolumeKey = "MusicVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.loop = true;
            audioSource.playOnAwake = false;

            float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
            audioSource.volume = savedVolume;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (backgroundMusic != null)
            PlayBackgroundMusic(false, backgroundMusic);

        BindSlider();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        AudioClip newClip = GetMusicForScene(scene.name);

        if (newClip != null)
        {
            PlayBackgroundMusic(true, newClip);
        }
        else if (allowedScenes != null && allowedScenes.Contains(scene.name))
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }
        else
        {
            audioSource.Stop();
        }

        // Delay slider binding to ensure UI is loaded
        StartCoroutine(DelayedBindSlider());
    }

    private AudioClip GetMusicForScene(string sceneName)
    {
        foreach (var entry in levelMusicList)
        {
            if (entry.sceneName == sceneName)
                return entry.musicClip;
        }
        return null;
    }

    public void BindSlider()
    {
        StartCoroutine(FindSliderWhenReady());
    }

    private System.Collections.IEnumerator FindSliderWhenReady()
    {
        Slider found = null;

        // Wait until slider object becomes active in the hierarchy
        for (int i = 0; i < 30; i++) // up to ~3 seconds
        {
            found = GameObject.FindGameObjectWithTag("MusicSlider")?.GetComponent<Slider>();
            if (found != null && found.gameObject.activeInHierarchy)
                break;

            yield return new WaitForSeconds(0.1f);
        }

        if (found != null)
        {
            musicSlider = found;
            Debug.Log($"✅ Music Slider finally found: {musicSlider.gameObject.name}");
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            musicSlider.value = audioSource.volume;
        }
        else
        {
            Debug.LogWarning("⚠️ Music Slider could not be found after waiting!");
        }
    }


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
    private System.Collections.IEnumerator DelayedBindSlider()
    {
        // Wait a bit to make sure scene UI (like MusicSlider) is loaded and active
        yield return null; // wait 1 frame
        yield return null; // optional extra frame for safety

        BindSlider();
    }

}

[System.Serializable]
public class LevelMusic
{
    public string sceneName;
    public AudioClip musicClip;
}
