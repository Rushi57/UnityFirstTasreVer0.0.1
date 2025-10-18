using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    private AudioSource audioSource;
    private const string VolumeKey = "SFXVolume";
    private float sfxVolume = 1f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = true;

            //Load saved Volume
            sfxVolume = PlayerPrefs.GetFloat(VolumeKey,1f);
            audioSource.volume = sfxVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if(clip != null)
            audioSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        audioSource.volume = sfxVolume;
        PlayerPrefs.SetFloat(VolumeKey,sfxVolume);
    }
    public float GetVolume()
    {
        return sfxVolume;
    }
}
