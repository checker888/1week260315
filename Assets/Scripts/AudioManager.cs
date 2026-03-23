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
        bgmVolume = PlayerPrefs.GetFloat("BGM", -40f);
        seVolume = PlayerPrefs.GetFloat("SE", -40f);
        SetBGMVolume(bgmVolume);
        SetSEVolume(seVolume);
    }

    public void PlaySE(AudioSource seSource, AudioClip clip)
    {
        seSource.PlayOneShot(clip, seVolume);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", volume);
    }

    public void SetSEVolume(float volume)
    {
        audioMixer.SetFloat("SE", volume);
    }

}