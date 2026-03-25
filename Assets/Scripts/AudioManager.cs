using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public float bgmVolume;
    public float seVolume;


    void Start()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGM", 0.5f);
        seVolume = PlayerPrefs.GetFloat("SE", 0.5f);
        SetBGMVolume(bgmVolume);
        SetSEVolume(seVolume);
    }

    public void PlaySE(AudioSource seSource, AudioClip clip)
    {
        seSource.PlayOneShot(clip, seVolume);
    }

    public void SetBGMVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        float decibel = 20f * Mathf.Log10(volume);
        decibel = Mathf.Clamp(decibel, -80f, 0f);
        audioMixer.SetFloat("BGM", decibel);
    }

    public void SetSEVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        float decibel = 20f * Mathf.Log10(volume);
        decibel = Mathf.Clamp(decibel, -80f, 0f);
        audioMixer.SetFloat("SE", decibel);
    }

}