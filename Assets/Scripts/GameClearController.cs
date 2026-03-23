using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameClearController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 3f;

    private bool isClearing = false;

    private float currentTime; // 今回のプレイ時間

    public AudioSource clearSound;

    private void Start()
    {
        // 前回の途中時間を取得（なければ0）
        currentTime = PlayerPrefs.GetFloat("NowTime", 0f);
    }

    private void Update()
    {
        if (isClearing) return;

        // プレイ中は時間加算
        currentTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isClearing) return;

        isClearing = true;

        PlayerCore pc = other.gameObject.GetComponent<PlayerCore>();
        FindObjectOfType<PlayerSwitchManager>().isGameCleared = true;
        pc.CanMove = false;
        if (clearSound != null)
        {
            clearSound.Play();
        }
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

        // 完全白
        color.a = 1f;
        fadeImage.color = color;

        // ===== クリアタイム処理 =====

        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        if (currentTime < bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", currentTime);
        }

        // NowTimeはリセット（次回新規スタート用）
        PlayerPrefs.SetFloat("NowTime", 0f);
        PlayerPrefs.SetInt("NowCheckPoint", 0);
        PlayerPrefs.Save();

        // シーン遷移
        SceneManager.LoadScene("Clear");
    }

    private void OnApplicationQuit()
    {
        SaveCurrentTime();
    }

    private void OnDisable()
    {
        SaveCurrentTime();
    }

    private void SaveCurrentTime()
    {
        if (isClearing) return;

        PlayerPrefs.SetFloat("NowTime", currentTime);
        PlayerPrefs.Save();
    }
}