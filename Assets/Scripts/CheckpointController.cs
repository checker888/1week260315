using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public int checkpointId; // 1,2,3...
    public Transform lightTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerCore pc = other.GetComponent<PlayerCore>();
        PlayerLightController plc = other.GetComponent<PlayerLightController>();

        // ライト初期化
        if (pc.HasLight)
        {
            plc.PutLight(lightTransform.position, lightTransform.rotation, true);
        }

        string key = checkpointId.ToString();

        // 初回到達チェック
        if (PlayerPrefs.GetInt(key, 0) == 0)
        {
            PlayerPrefs.SetInt(key, 1); // bool代わり
        }

        // 現在のチェックポイント更新（毎回更新でOK）
        PlayerPrefs.SetInt("NowCheckPoint", checkpointId);

        PlayerPrefs.Save();
    }
}