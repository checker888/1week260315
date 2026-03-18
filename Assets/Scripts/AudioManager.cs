using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<AudioSource> bgmSource;

    public float bgmVolume;
    public float seVolume;

    void Awake()
    {
        Instance = this;

    }

    void Start()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGM", 0.5f);
        seVolume = PlayerPrefs.GetFloat("SE", 0.5f);
    }

    public void PlaySE(AudioSource seSource, AudioClip clip)
    {
        seSource.PlayOneShot(clip, seVolume);
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        foreach (AudioSource source in bgmSource)
        {
            source.volume = value;
        }
    }

    public void SetSEVolume(float value)
    {
        seVolume = value;
    }


}