using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public string CheckPointName;
    public Transform lightTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCore pc = other.gameObject.GetComponent<PlayerCore>();
            PlayerLightController plc = other.gameObject.GetComponent<PlayerLightController>();
            if(pc.HasLight)plc.PutLight(lightTransform.position, lightTransform.rotation, true);
        }
    }
}