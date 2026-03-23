using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMStop : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        audioSource.volume = 0f;
    }
}
