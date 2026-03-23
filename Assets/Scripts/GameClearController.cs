using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameClearController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 3f;

    private bool isClearing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isClearing) return;

        isClearing = true;
        PlayerCore pc = other.GetComponent<PlayerCore>();
        pc.CanMove = false;
        StartCoroutine(ClearSequence());
    }

    private IEnumerator ClearSequence()
    {
        float time = 0f;
        Color color = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);

            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        // ”O‚Ì‚½‚ßŠ®‘S‚É”’‚É‚·‚é
        color.a = 1f;
        fadeImage.color = color;

        // ƒV[ƒ“‘JˆÚ
        SceneManager.LoadScene("Clear");
    }
}