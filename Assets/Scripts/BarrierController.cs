using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public GameObject parentSwitchBlock;
    public AudioSource audioSource;

    private void OnDisable()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
