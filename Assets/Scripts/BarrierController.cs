using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public GameObject parentSwitchBlock;
    public AudioSource audioSource;

    public void PlayDisableSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        }
    }
}