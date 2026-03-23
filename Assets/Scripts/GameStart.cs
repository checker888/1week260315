using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStart : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 3f;

    private void Start()
    {
        if (fadeImage == null) return;

        // Å‰‚ÍŠ®‘S‚É•‚É‚·‚é
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;
        Color color = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);

            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        // Š®‘S‚É“§–¾
        color.a = 0f;
        fadeImage.color = color;
    }
}