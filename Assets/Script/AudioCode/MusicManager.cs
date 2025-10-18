using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MusicManager : MonoBehaviour
{
    [Header("-------AudioSource-----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [Header("-------AudioClip-----")]
    public AudioClip background;
    public AudioClip StoveOn;
    public AudioClip StoveOff;
    public AudioClip Simmer;
    public AudioClip Mixing;
    public AudioClip choppingSound;
    public AudioClip ButtonClick;


    public void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void PlaySFX()
    {
        SFXSource.clip = background;
        SFXSource.Play();
    }
}
