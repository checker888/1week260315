using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public int checkpointId;
    public Transform lightTransform;
    public Transform trashPos;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerCore pc = other.GetComponent<PlayerCore>();
        PlayerLightController plc = other.GetComponent<PlayerLightController>();

        // FlashLightが子にあるかチェック
        bool hasFlashLight = false;
        foreach (Transform child in lightTransform)
        {
            if (child.name == "FlashLight")
            {
                hasFlashLight = true;
                break;
            }
        }

        // ライト初期化（FlashLightがない場合のみ）
        if (pc.HasLight)
        {
            if (!hasFlashLight)
            {
                plc.PutLight(lightTransform.position, lightTransform.rotation, true);
            }
            else
            {
                plc.PutLight(trashPos.position, trashPos.rotation, true);
            }
            
        }

        string key = checkpointId.ToString();

        if (PlayerPrefs.GetInt(key, 0) == 0)
        {
            PlayerPrefs.SetInt(key, 1);
        }

        PlayerPrefs.SetInt("NowCheckPoint", checkpointId);
        PlayerPrefs.Save();
    }
}