using UnityEngine;
using TMPro;

public class ClearTimeController : MonoBehaviour
{
    public GameObject timeTextObj;
    private TextMeshProUGUI timeText;
    private void Start()
    {
        timeText = timeTextObj.GetComponent<TextMeshProUGUI>();
        float clearTime = PlayerPrefs.GetFloat("NowTime", -1f);

        if (clearTime < 0f || clearTime == float.MaxValue)
        {
            timeTextObj.SetActive(false);
        }
        else
        {
            timeTextObj.SetActive(true);
            timeText.text =  "クリアタイム：" + FormatTime(clearTime);
        }


        PlayerPrefs.SetFloat("NowTime", 0f);
        PlayerPrefs.Save();
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        return $"{minutes:00}分{seconds:00}秒{milliseconds:000}";
    }
}