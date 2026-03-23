using UnityEngine;
using System.Collections;

public class BGMController : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeTime = 10f;
    private float targetVolume;

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            targetVolume = audioSource.volume;
            audioSource.volume = 0f;
            audioSource.Play();
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float currentTime = 0f;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, currentTime / fadeTime);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}